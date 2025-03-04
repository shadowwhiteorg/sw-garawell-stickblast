using UnityEngine;

namespace _Game.Interfaces
{
    public interface IGridObject
    {
        int RowY { get; }
        int ColumnX { get; }
        float PosX { get; }
        float PosY { get; } 
        Vector2 WorldPosition { get; }
        Vector2Int GridPosition { get; }
        void SetPosition(int column, int row, float posX, float posY);
        
        void SetWorldPosition(Vector2 worldPosition);
        

    }
}