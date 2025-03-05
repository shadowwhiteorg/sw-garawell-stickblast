using System.Collections.Generic;
using _Game.BlockSystem;
using _Game.CoreMechanic;
using _Game.DataStructures;
using _Game.Utils;
using UnityEngine;

namespace _Game.Managers
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] private int numberOfRows;
        [SerializeField] private int numberOfColumns;
        [SerializeField] private float blockSize;
        [SerializeField] private BlockCatalog blockCatalog;

        private Dictionary<Vector2Int, SidelineBlock> sidelineGrid = new();
        private Dictionary<Vector2Int, SquareBlock> squareGrid = new();

        public BlockCatalog BlockCatalog => blockCatalog;
        public float BlockSize => blockSize;
        public int NumberOfRows => numberOfRows;
        public int NumberOfColumns => numberOfColumns;
        public Dictionary<Vector2Int, SidelineBlock> SidelineGrid => sidelineGrid;
        public Dictionary<Vector2Int, SquareBlock> SquareGrid => squareGrid;
        public bool TryGetSidelineBlock(Vector2Int gridPos, out SidelineBlock block)
        {
            return sidelineGrid.TryGetValue(gridPos, out block);
        }

        public bool IsGridPositionEmpty(Vector2Int gridPos, bool isHorizontal)
        {
            return !sidelineGrid.ContainsKey(gridPos);
        }

        public bool TryPlaceLine(Vector2Int gridPos, SidelineBlock lineBlock)
        {
            if (!sidelineGrid.TryAdd(gridPos, lineBlock)) return false;
            MatchHandler.Instance.CheckForSquares(gridPos, lineBlock.IsHorizontal);
            return true;
        }

        public bool HasHorizontalLine(int x, int y)
        {
            return sidelineGrid.TryGetValue(new(x, y), out var line) && line.IsHorizontal;
        }

        public bool HasVerticalLine(int x, int y)
        {
            return sidelineGrid.TryGetValue(new(x, y), out var line) && !line.IsHorizontal;
        }

        public void CreateSquare(int x, int y)
        {
            Vector2Int key = new(x, y);
            if (squareGrid.ContainsKey(key)) return;

            Vector2 worldPos = GridToWorldPosition(key);
            SquareBlock square = Instantiate(blockCatalog.squareBlockPrefab, worldPos, Quaternion.identity, transform);
            square.SetPosition(x, y, worldPos.x, worldPos.y);
            squareGrid.Add(key, square);

            MatchHandler.Instance.CheckForCompletedLines();
        }

        public void BlastRow(int row)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                Vector2Int key = new(x, row);
                if (squareGrid.Remove(key, out var square)) Destroy(square.gameObject);
            }
        }

        public void BlastColumn(int column)
        {
            for (int y = 0; y < numberOfRows; y++)
            {
                Vector2Int key = new(column, y);
                if (squareGrid.Remove(key, out var square)) Destroy(square.gameObject);
            }
        }

        public Vector2 GridToWorldPosition(Vector2Int gridPos)
        {
            float x = gridPos.x * blockSize;
            float y = gridPos.y * blockSize;
            return new Vector2(x, y);
        }
    }
}