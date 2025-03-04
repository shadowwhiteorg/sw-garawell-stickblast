using _Game.Interfaces;
using UnityEngine;

namespace _Game.Utils
{
    public static class GridPlacer<T>  where  T : Object, IGridObject
    {
        public static void Place(int gridSizeX, int gridSizeY, float cellSize, T gridObjectPrefab)
        {
            for (int i = 0; i < gridSizeX; i++)
            {
                for (int j = 0; j < gridSizeY; j++)
                {
                    var posX = i * cellSize;
                    var posY = j * cellSize;
                    var gridObject = Object.Instantiate(gridObjectPrefab, new Vector3(posX, posY, 0), Quaternion.identity);
                    gridObject.SetPosition(i, j, posX, posY);
                }
            }
        }
    }
}