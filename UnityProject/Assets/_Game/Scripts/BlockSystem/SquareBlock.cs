using UnityEngine;

namespace _Game.BlockSystem
{
    public sealed class SquareBlock : GridObjectBase
    {
        public SquareBlock(Vector2Int gridPosition, Vector2 worldPosition)
        {
            SetPosition(gridPosition.x, gridPosition.y, worldPosition.x, worldPosition.y); 
        }
    }
}