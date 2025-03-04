using UnityEngine;

namespace _Game.Interfaces
{
    public interface IGridObject
    {
        int RowY { get; }
        int ColumnX { get; }
        float PosX { get; }
        float PosY { get; } 
        void SetPosition(int column, int row, float posX, float posY);
        Vector2Int GridPosition { get; }
        Vector2 WorldPosition { get; }

    }
}