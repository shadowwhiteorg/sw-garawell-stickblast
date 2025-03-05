using System.Collections.Generic;
using _Game.BlockSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.CoreMechanic
{
    public class MatchHandler : Singleton<MatchHandler>
    {
        public void CheckForSquares(Vector2Int linePos, bool isHorizontal)
        {
            List<Vector2Int> squaresToCheck = new();
            if (isHorizontal)
            {
                squaresToCheck.Add(new(linePos.x, linePos.y - 1)); // Square below
                squaresToCheck.Add(linePos);                      // Square current
            }
            else
            {
                squaresToCheck.Add(new(linePos.x - 1, linePos.y)); // Square left
                squaresToCheck.Add(linePos);                       // Square current
            }

            foreach (var pos in squaresToCheck) CheckSquare(pos);
        }

        private void CheckSquare(Dictionary<Vector2Int,SquareBlock> squareGrid,Dictionary<Vector2Int,SidelineBlock> sidelineGrid,Vector2Int squarePos)
        {
            int x = squarePos.x, y = squarePos.y;
            bool hasBottom = HasHorizontalLine(sidelineGrid,x, y);
            bool hasTop = HasHorizontalLine(sidelineGrid,x, y + 1);
            bool hasLeft = HasVerticalLine(sidelineGrid,x, y);
            bool hasRight = HasVerticalLine(sidelineGrid,x + 1, y);

            if (hasBottom && hasTop && hasLeft && hasRight)
                CreateSquare(squareGrid,x, y);
        }

        private bool HasHorizontalLine(Dictionary<Vector2Int,SidelineBlock> sidelineGrid,int x, int y) =>
            sidelineGrid.TryGetValue(new(x, y), out var line) && line.IsHorizontal;

        private bool HasVerticalLine(Dictionary<Vector2Int,SidelineBlock> sidelineGrid,int x, int y) =>
            sidelineGrid.TryGetValue(new(x, y), out var line) && !line.IsHorizontal;

        private void CreateSquare(Dictionary<Vector2Int,SquareBlock> squareGrid,int x, int y)
        {
            Vector2Int key = new(x, y);
            if (squareGrid.ContainsKey(key)) return;

            Vector2 worldPos = GridToWorldPosition(key);
            SquareBlock square = Instantiate(squareGrid.squareBlockPrefab, worldPos, Quaternion.identity, transform);
            square.SetPosition(x, y, worldPos.x, worldPos.y);
            squareGrid.Add(key, square);

            CheckForCompletedLines();
        }

        private void CheckForCompletedLines()
        {
            for (int y = 0; y < numberOfRows; y++) CheckRow(y);
            for (int x = 0; x < numberOfColumns; x++) CheckColumn(x);
        }

        private void CheckRow(Dictionary<Vector2Int,SquareBlock> squareGrid,int row)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                if (!squareGrid.ContainsKey(new(x, row))) return;
            }
            BlastRow(row);
        }

        private void BlastRow(Dictionary<Vector2Int,SquareBlock> squareGrid,int row)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                Vector2Int key = new(x, row);
                if (squareGrid.Remove(key, out var square)) Destroy(square.gameObject);
            }
        }

        private void CheckColumn(Dictionary<Vector2Int,SquareBlock> squareGrid,int column)
        {
            for (int y = 0; y < numberOfRows; y++)
            {
                if (!squareGrid.ContainsKey(new(column, y))) return;
            }
            BlastColumn(column);
        }

        private void BlastColumn(Dictionary<Vector2Int,SquareBlock> squareGrid,int column)
        {
            for (int y = 0; y < numberOfRows; y++)
            {
                Vector2Int key = new(column, y);
                if (squareGrid.Remove(key, out var square)) Destroy(square.gameObject);
            }
        }
    }
}