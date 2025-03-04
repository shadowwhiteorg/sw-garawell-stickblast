using _Game.Interfaces;
using UnityEngine;

namespace _Game.GridSystem
{
    public sealed class SidelineBlock : GridObjectBase, ITouchable
    {
        [SerializeField] private Vector2 touchSize = Vector2.zero;

        public Vector2 TouchSize
        {
            get => touchSize;
            set => touchSize = value;
        }

        public SidelineBlock(Vector2Int gridPosition, Vector2 worldPosition)
        {
            SetPosition( gridPosition.x, gridPosition.y,worldPosition.x,worldPosition.y );
        }
    }
    
    
}