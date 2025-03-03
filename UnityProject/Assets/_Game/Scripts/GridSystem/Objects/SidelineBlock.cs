using UnityEngine;

namespace _Game.GridSystem
{
    public class SidelineBlock : GridObjectBase
    {
        public bool IsHorizontal { get; private set; }
        
        public SidelineBlock(Vector2Int gridPosition, bool isHorizontal)
        {
            GridPosition = gridPosition;
            IsHorizontal = isHorizontal;
        }
    }
}