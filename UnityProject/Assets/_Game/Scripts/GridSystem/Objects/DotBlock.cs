using UnityEngine;

namespace _Game.GridSystem
{
    public sealed class DotBlock : GridObjectBase
    {
        public DotBlock(Vector2Int gridPosition, Vector2 worldPosition)
        {
            SetPosition(gridPosition.x, gridPosition.y, worldPosition.x, worldPosition.y); 
        }
    }
}