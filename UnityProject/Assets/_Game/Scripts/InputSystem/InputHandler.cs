using System;
using _Game.BlockSystem;
using _Game.CoreMechanic;
using _Game.DataStructures;
using _Game.GridSystem;
using _Game.InputSystem;
using _Game.LevelSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Managers
{
    public class InputHandler : Singleton<InputHandler>
    {
        [SerializeField] private float initialTouchableXOffset = 0.5f;
        [SerializeField] private float selectRange = 5f;
        public float InitialTouchableXOffset => initialTouchableXOffset;
        public float SelectRange => selectRange;
        private bool _isRocketActive;
        public bool IsRocketActive => _isRocketActive;

        private SelectionHandler _selectionHandler;
        private Camera _camera;
        
        private void Start()
        {
            _camera = Camera.main;
            _selectionHandler = new SelectionHandler();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _selectionHandler.SelectClosestObject(
                    _camera.ScreenToWorldPoint(Input.mousePosition)
                );
                if(_isRocketActive)
                    HandleSquareClick(_camera.ScreenToWorldPoint(Input.mousePosition));
            }

            if (Input.GetMouseButton(0))
            {
                _selectionHandler.DragSelectedObject(
                    _camera.ScreenToWorldPoint(Input.mousePosition)
                );
            }

            if (Input.GetMouseButtonUp(0))
            {
                _selectionHandler.ReleaseSelectedObject();
            }
            
        }

        private void ActivateRocket()
        {
            _isRocketActive = !_isRocketActive;
        }
        
        private void HandleSquareClick(Vector2 worldPosition)
        {
            Vector2Int? clickedSquare = GridManager.Instance.GetClickedSquare(worldPosition);
            if (clickedSquare.HasValue)
            {
                GridManager.Instance.BlastSquare(clickedSquare.Value);
            }
        }
        
        private void OnEnable()
        {
            EventBus.Subscribe<OnRocketSelected>(@event => ActivateRocket());
            EventBus.Subscribe<OnBlastEvent>(@event => ActivateRocket());

        }
    }
}