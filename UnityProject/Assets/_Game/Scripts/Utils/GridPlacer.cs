using _Game.Interfaces;
using UnityEngine;
using System.Collections.Generic;

namespace _Game.Utils
{
    public static class GridPlacer<T> where T : Object, IGridObject
    {
        public static List<T> Place(int gridSizeX, int gridSizeY, float cellSize, T gridObjectPrefab, GameObject parentObject = null)
        {
            List<T> mBlockList = new List<T>();
            for (int i = 0; i < gridSizeY; i++)
            {
                for (int j = 0; j < gridSizeX; j++)
                {
                    var posX = i * cellSize;
                    var posY = j * cellSize;
                    var gridObject = Object.Instantiate(gridObjectPrefab, new Vector3(posX, posY, 0), Quaternion.identity,parentObject?.transform);
                    gridObject.SetPosition(i, j, posX, posY);
                    mBlockList.Add(gridObject);
                }
            }
            return mBlockList;
        }

        public static void PositionTheGridAtCenter(List<T> objectList, int sizeX, int sizeY, float gridSize, string parentName, Transform parentObject)
        {
            GameObject gridParent = new GameObject(parentName);
            foreach (var obj in objectList)
            {
                ((MonoBehaviour)(object)obj)?.transform.SetParent(gridParent.transform);
            }
            float centerY = -(sizeY * gridSize) / 2f;
            float centerX = -(sizeX * gridSize) / 2f;
            gridParent.transform.SetParent(parentObject);
            gridParent.transform.position = new Vector3(centerX, centerY, 0);
        }
    }
}