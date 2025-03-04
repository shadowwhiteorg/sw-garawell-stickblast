using _Game.Interfaces;
using UnityEngine;

namespace _Game.GridSystem
{
    public abstract class GridObjectBase : MonoBehaviour, IGridObject
    {
        public int RowY { get; }
        public int ColumnX { get; }
        public float PosX { get; }
        public float PosY { get; }
        public void SetPosition(int column, int row, float posX, float posY)
        {
        }
        public Vector2Int GridPosition { get; protected set; }
        public Vector2 WorldPosition { get; }
    }
}