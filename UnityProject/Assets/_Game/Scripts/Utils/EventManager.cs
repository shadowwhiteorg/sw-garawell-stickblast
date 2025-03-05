using System;
using UnityEngine;
namespace _Game.Utils
{
    public static class EventManager
    {
        public static event Action<Vector2> OnTouch;
        public static event Action OnLevelStart;
        public static void FireOnTouch(Vector2 touchPosition) => OnTouch?.Invoke(touchPosition);
        public static void FireOnLevelStart() => OnLevelStart?.Invoke();
        
        
        public static Vector2Int ScreenToGridPosition(Vector3 screenPosition, Camera camera, Vector3 gridOriginWorldPosition, float blockSize)
        {
            Vector3 worldMousePos = camera.ScreenToWorldPoint(screenPosition);
            Vector3 relativePos = worldMousePos - gridOriginWorldPosition;
            int columnIndex = Mathf.RoundToInt(relativePos.x / blockSize);
            int rowIndex = Mathf.RoundToInt(relativePos.y / blockSize);
            return new Vector2Int(columnIndex, rowIndex);
        }

        public static Vector3 GridToWorldPosition(Vector2Int gridPosition, Vector3 gridOriginWorldPosition, float blockSize)
        {
            float worldPosX = gridOriginWorldPosition.x + gridPosition.x * blockSize;
            float worldPosY = gridOriginWorldPosition.y + gridPosition.y * blockSize;
            return new Vector3(worldPosX, worldPosY, 0); // Z might need adjustment
        }
    }
}