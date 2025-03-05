using _Game.BlockSystem;
using _Game.CoreMechanic;
using _Game.InputSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Managers
{
    public class InputHandler : Singleton<InputHandler>
    {
        [SerializeField] private float initialTouchableXOffset;
        public float InitialTouchableXOffset => initialTouchableXOffset;
        private SidelineBlock _currentBlock;
        private bool _isDragging;
        private Camera _camera;
        
        private SelectionHandler _selectionHandler;
        private MovementHandler<MonoBehaviour> _movementHandler;


        
        private void Awake()
        {
            _camera = Camera.main;
            _selectionHandler = new SelectionHandler();
            _movementHandler = new MovementHandler<MonoBehaviour>();
        }
        
        private void Update()
        {
            // if (Input.GetMouseButtonDown(0))
            // {
            //     TryStartDrag();
            // }

            // if (_isDragging)
            // {
            //     HandleDrag();
            // }

            if (Input.GetMouseButtonUp(0))
            {
                TryPlaceBlock();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                LevelManager.Instance.MoveTouchablesIntoScene();
            }
            
            if (Input.GetKeyDown(KeyCode.K))
            {
                LevelManager.Instance.CreateTouchableBlocks();
            }
            
            
            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
                Vector2 touchPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                _currentBlock = _selectionHandler.SelectClosestObject(touchPosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 currentPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                _selectionHandler.DragSelectedObject(currentPosition);
                HandleDrag();
            }

            // if (Input.GetMouseButtonUp(0))
            // {
            //     _selectionHandler.ReleaseSelectedObject();
            // }

            // if (Input.GetKeyDown(KeyCode.S))
            // {
            //     LevelManager.Instance.MoveTouchablesIntoScene();
            // }
        }

        private void TryStartDrag()
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPos = GridManager.Instance.WorldToGridPosition(mouseWorldPos);

            // Check for a block at the grid position (default to horizontal for this example)
            if (PlacementHandler.Instance.TryGetBlockAt(gridPos, true, out SidelineBlock block))
            {
                _currentBlock = block;
                _isDragging = true;
            }
        }

        private void HandleDrag()
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPos = GridManager.Instance.WorldToGridPosition(mouseWorldPos);

            // Only show ghost block if the grid position is valid
            if (GridManager.Instance.IsGridPositionValid(gridPos) && 
                PlacementHandler.Instance.IsGridPositionEmpty(gridPos, _currentBlock.IsHorizontal))
            {
                GhostBlockHandler.Instance.ShowGhostBlock(gridPos, _currentBlock.IsHorizontal);
            }
            else
            {
                GhostBlockHandler.Instance.HideGhostBlock();
            }
        }

        private void TryPlaceBlock()
        {
            if (!_currentBlock) return;

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPos = GridManager.Instance.WorldToGridPosition(mouseWorldPos);

            PlacementHandler.Instance.TryPlaceBlock(_currentBlock, gridPos);

            _currentBlock = null;
            _isDragging = false;
        }
    }
}