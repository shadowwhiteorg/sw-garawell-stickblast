using System.Collections.Generic;
using _Game.GridSystem;
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
        
        public void CheckAllSquares()
        {
            for (int x = 0; x < GridManager.Instance.NumberOfColumns; x++)
            {
                for (int y = 0; y < GridManager.Instance.NumberOfRows; y++)
                {
                    CheckSquare(new Vector2Int(x, y));
                }
            }
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
        
        public void BlastConnectedSquares(Vector2Int startPos)
        {
            if (!GridManager.Instance.SquareGrid.ContainsKey(startPos)) return;
            
            HashSet<Vector2Int> squaresToBlast = new();
            FindConnectedSquares(startPos, squaresToBlast);
            
            foreach (var square in squaresToBlast)
            {
                GridManager.Instance.BlastSquare(square);
            }
        }

        private void FindConnectedSquares(Vector2Int startPos, HashSet<Vector2Int> visited)
        {
            Stack<Vector2Int> stack = new();
            stack.Push(startPos);

            while (stack.Count > 0)
            {
                Vector2Int current = stack.Pop();
                if (!visited.Add(current)) continue; // Skip if already visited
                
                foreach (Vector2Int neighbor in GetSquareNeighbors(current))
                {
                    if (GridManager.Instance.SquareGrid.ContainsKey(neighbor) && !visited.Contains(neighbor))
                    {
                        stack.Push(neighbor);
                    }
                }
            }
        }

        private List<Vector2Int> GetSquareNeighbors(Vector2Int squarePos)
        {
            List<Vector2Int> neighbors = new();

            Vector2Int[] directions =
            {
                new(squarePos.x + 1, squarePos.y), // Right
                new(squarePos.x - 1, squarePos.y), // Left
                new(squarePos.x, squarePos.y + 1), // Up
                new(squarePos.x, squarePos.y - 1)  // Down
            };

            foreach (Vector2Int dir in directions)
            {
                if (GridManager.Instance.SquareGrid.ContainsKey(dir))
                    neighbors.Add(dir);
            }

            return neighbors;
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