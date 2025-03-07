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
        
    }
}