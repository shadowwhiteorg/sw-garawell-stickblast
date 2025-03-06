using _Game.DataStructures;
using _Game.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.BlockSystem
{
    public sealed class SidelineBlock : GridObjectBase, ITouchable
    {
        
        [SerializeField] private Vector2 touchSize = Vector2.zero;
        [SerializeField] private bool isHorizontal = false;
        [SerializeField] private Shape shape;
        public Shape Shape => shape;
        public bool IsHorizontal => isHorizontal;
        
        private bool _canMove;
        
        public Vector3 InitialPosition { get; private set;}
        public bool CanMove => _canMove;

        public Vector2 TouchSize
        {
            get => touchSize;
            set => touchSize = value;
        }

        public void SetInitialPosition(Vector3 position)
        {
            InitialPosition = position;
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