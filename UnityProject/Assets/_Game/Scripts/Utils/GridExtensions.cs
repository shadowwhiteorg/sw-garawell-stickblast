using _Game.Managers;
using UnityEngine;

namespace _Game.Utils
{
    public static class GridExtensions
    {
        public static Vector2Int WorldToGridPosition(this GridManager grid, Vector3 worldPosition)
        {
            float halfBlockSize = grid.BlockSize / 2;
            int x = Mathf.FloorToInt((worldPosition.x + halfBlockSize) / grid.BlockSize);
            int y = Mathf.FloorToInt((worldPosition.y + halfBlockSize) / grid.BlockSize);
            return new Vector2Int(x, y);
        }

        public static Vector2 GridToWorldPosition(this GridManager grid, Vector2Int gridPosition)
        {
            float x = gridPosition.x * grid.BlockSize;
            float y = gridPosition.y * grid.BlockSize;
            return new Vector2(x, y);
        }
    }
}