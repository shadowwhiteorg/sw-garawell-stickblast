using System.Collections.Generic;
using _Game.Managers;
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

        private void CheckSquare(Vector2Int squarePos)
        {
            int x = squarePos.x, y = squarePos.y;
            bool hasBottom = GridManager.Instance.HasHorizontalLine(x, y);
            bool hasTop = GridManager.Instance.HasHorizontalLine(x, y + 1);
            bool hasLeft = GridManager.Instance.HasVerticalLine(x, y);
            bool hasRight = GridManager.Instance.HasVerticalLine(x + 1, y);

            if (hasBottom && hasTop && hasLeft && hasRight)
                GridManager.Instance.CreateSquare(x, y);
        }

        public void CheckForCompletedLines()
        {
            for (int y = 0; y < GridManager.Instance.NumberOfRows; y++) CheckRow(y);
            for (int x = 0; x < GridManager.Instance.NumberOfColumns; x++) CheckColumn(x);
        }

        private void CheckRow(int row)
        {
            for (int x = 0; x < GridManager.Instance.NumberOfColumns; x++)
            {
                if (!GridManager.Instance.SquareGrid.ContainsKey(new(x, row))) return;
            }
            GridManager.Instance.BlastRow(row);
        }

        private void CheckColumn(int column)
        {
            for (int y = 0; y < GridManager.Instance.NumberOfRows; y++)
            {
                if (!GridManager.Instance.SquareGrid.ContainsKey(new(column, y))) return;
            }
            GridManager.Instance.BlastColumn(column);
        }
    }
}