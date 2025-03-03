using UnityEngine;

namespace _Game.GridSystem
{
    public class DotBlock : GridObjectBase
    {
        public DotBlock(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }
    }
}