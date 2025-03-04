using _Game.Interfaces;
using UnityEngine;

namespace _Game.GridSystem
{
    public class SidelineBlock : GridObjectBase, ITouchable
    {
        [SerializeField] private Vector2 touchSize = Vector2.zero; 
        public Vector2 TouchSize { get; set; }

        public sealed override void SetPosition(int column, int row, float posX, float posY)
        {
            base.SetPosition(column, row, posX, posY);
        }

        public SidelineBlock(Vector2Int gridPosition, Vector2 worldPosition)
        {
            SetPosition( gridPosition.x, gridPosition.y,worldPosition.x,worldPosition.y );
            SetTouchSize(touchSize.x,touchSize.y);
        }

        public void SetTouchSize(float xRange, float yRange)
        {
            TouchSize = new Vector2(xRange, yRange);
        }

        public Vector2 GetTouchSize()
        {
            return TouchSize;
        }
    }
}