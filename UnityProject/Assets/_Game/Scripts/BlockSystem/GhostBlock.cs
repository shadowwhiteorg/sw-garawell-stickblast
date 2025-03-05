using _Game.Interfaces;
using UnityEngine;

namespace _Game.BlockSystem
{
    public class GhostBlock : GridObjectBase
    {
        public int RowY { get; }
        public int ColumnX { get; }
        public float PosX { get; }
        public float PosY { get; }

        public Vector2Int GridPosition { get; private set; }
        public Vector2 WorldPosition { get; private set; }
        
        public void SetPosition(int column, int row, float posX, float posY)
        {
            GridPosition = new Vector2Int(column, row);
            WorldPosition = new Vector2(posX, posY);
            
        }
    }
}