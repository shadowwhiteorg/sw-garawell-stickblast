using System.Collections.Generic;
using _Game.BlockSystem;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.GridSystem
{
    public static class GridFactory
    {
        public static List<T> CreateGrid<T>(int width, int height, float cellSize, T prefab, GameObject parent = null) where T : GridObjectBase
        {
            List<T> gridObjects = new();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    T instance = Object.Instantiate(prefab);
                    instance.transform.position = new Vector3(x * cellSize, y * cellSize, 0);
                    if (parent) instance.transform.SetParent(parent.transform);
                    gridObjects.Add(instance);
                }
            }
            return gridObjects;
        }

        public static void PositionGridAtCenter<T>(List<T> objects, int width, int height, float cellSize, string parentName) where T : GridObjectBase
        {
            Vector3 offset = new Vector3(width * cellSize / 2, height * cellSize / 2, 0);
            GameObject parentObject = new GameObject(parentName);
            foreach (var obj in objects)
            {
                obj.transform.position -= offset;
                obj.transform.SetParent(parentObject.transform);
            }
        }

        public static SidelineBlock CreateSidelineBlock(SidelineBlock prefab, bool isHorizontal, Transform parent)
        {
            SidelineBlock block = Object.Instantiate(prefab, parent);
            // block.Initialize(isHorizontal);
            return block;
        }
    }
}