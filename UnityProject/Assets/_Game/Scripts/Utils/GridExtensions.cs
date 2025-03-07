using _Game.GridSystem;
using _Game.Managers;
using UnityEngine;

namespace _Game.Utils
{
    public static class GridExtensions
    {
        public static Vector2Int WorldToGridPosition(this GridManager grid, Vector3 worldPosition)
        {
            float halfBlockSize = grid.BlockSize / 2;
            float centerY = -(grid.NumberOfColumns * grid.BlockSize) / 2f;
            float centerX = -(grid.NumberOfRows * grid.BlockSize) / 2f;
            int x = Mathf.FloorToInt((worldPosition.x - centerX) / grid.BlockSize);
            int y = Mathf.FloorToInt((worldPosition.y - centerY) / grid.BlockSize);
            return new Vector2Int(x, y);
        }

        public static Vector2 GridToWorldPosition(this GridManager grid, Vector2Int gridPosition)
        {
            float x = gridPosition.x * grid.BlockSize;
            float y = gridPosition.y * grid.BlockSize;
            float centerY = -(grid.NumberOfColumns * grid.BlockSize) / 2f;
            float centerX = -(grid.NumberOfRows * grid.BlockSize) / 2f;
            return new Vector2(x + centerX, y+ centerY);
        }
    }
}