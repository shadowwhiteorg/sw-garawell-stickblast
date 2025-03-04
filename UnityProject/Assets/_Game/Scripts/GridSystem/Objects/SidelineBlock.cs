using _Game.Interfaces;
using UnityEngine;

namespace _Game.GridSystem
{
    public sealed class SidelineBlock : GridObjectBase, ITouchable
    {
        [SerializeField] private Vector2 touchSize = Vector2.zero; 
        public Vector2 TouchSize { get; set; }
        
        public SidelineBlock(Vector2Int gridPosition, Vector2 worldPosition)
        {
            SetPosition( gridPosition.x, gridPosition.y,worldPosition.x,worldPosition.y );
            SetTouchSize(touchSize.x,touchSize.y);
        }

        public void SetTouchSize(float xRange, float yRange)
        {
            TouchSize = new Vector2(xRange, yRange);
        }
        
    }
}