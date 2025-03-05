using _Game.Interfaces;
using UnityEngine;

namespace _Game.BlockSystem
{
    public sealed class SidelineBlock : GridObjectBase, ITouchable
    {
        [SerializeField] private Vector2 touchSize = Vector2.zero;
        private bool _canMove;
        public bool CanMove => _canMove;

        public Vector2 TouchSize
        {
            get => touchSize;
            set => touchSize = value;
        }

        public SidelineBlock(Vector2Int gridPosition, Vector2 worldPosition)
        {
            SetPosition( gridPosition.x, gridPosition.y,worldPosition.x,worldPosition.y );
        }

        public void LockUnlockMove(bool canMove)
        {
            _canMove = canMove;
        }
    }
    
    
}