using UnityEngine;

namespace _Game.GridSystem
{
    public class SquareBlock : GridObjectBase
    {
        public SquareBlock(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }
    }
}