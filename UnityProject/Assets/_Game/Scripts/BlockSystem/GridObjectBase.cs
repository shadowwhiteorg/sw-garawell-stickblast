using _Game.Interfaces;
using UnityEngine;

namespace _Game.BlockSystem
{
    public abstract class GridObjectBase : MonoBehaviour, IGridObject
    {
        public int RowY { get; }
        public int ColumnX { get; }
        public float PosX { get; }
        public float PosY { get; }
        public Vector2 WorldPosition { get; private set; }
        public Vector2Int GridPosition { get; private set; }

        public virtual void SetPosition(int column, int row, float posX, float posY)
        {
            WorldPosition = new Vector2(posX, posY);
            GridPosition = new Vector2Int(column, row);
        }
        
        
        
        public virtual void SetWorldPosition(Vector2 worldPosition)
        {
            WorldPosition = worldPosition;
        }

        public void ResetPosition()
        {
            
            // TODO: Get the logic from selection handler -> release selectedObject
        }
    }
}