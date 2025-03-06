using _Game.BlockSystem;
using _Game.CoreMechanic;
using _Game.Enums;
using _Game.GridSystem;
using _Game.Managers;
using _Game.Utils;
using UnityEngine;

namespace _Game.InputSystem
{
    public class SelectionHandler
    {
        private GameObject _selectedShapeParent; // Parent object of the selected shape
        private Vector2 _dragOffset;
        private Vector2Int _pivotGridPos;

        public SidelineBlock SelectClosestObject(Vector2 touchPosition)
        {
            var selectedBlock = GridHandler.Instance.GetClosestTouchable(touchPosition);
            if (!selectedBlock) return null;

            // Get the parent object of the shape
            _selectedShapeParent = selectedBlock.transform.parent.gameObject;

            // Calculate pivot grid position
            Vector2 worldPos = _selectedShapeParent.transform.position;
            _pivotGridPos = GridManager.Instance.WorldToGridPosition(worldPos);

            // Calculate drag offset
            _dragOffset = (Vector2)_selectedShapeParent.transform.position - touchPosition;

            // Lift the shape slightly
            MovementHandler.MoveWithEase(
                _selectedShapeParent.transform,
                (Vector3)worldPos + InputHandler.Instance.InitialTouchableXOffset * Vector3.up,
                50,
                Easing.OutSine
            );

            return selectedBlock;
        }

        public void DragSelectedObject(Vector2 touchPosition)
        {
            if (!_selectedShapeParent) return;

            // Move the shape to the touch position with the offset
            _selectedShapeParent.transform.position = touchPosition + _dragOffset;

            // Update ghost positions
            Vector2Int gridPos = GridManager.Instance.WorldToGridPosition(_selectedShapeParent.transform.position);
            GhostBlockHandler.Instance.ShowGhostShape(gridPos, _selectedShapeParent.GetComponentInChildren<SidelineBlock>().Shape);
        }

        public void ReleaseSelectedObject()
        {
            if (!_selectedShapeParent) return;

            Vector2Int targetGridPos = GridManager.Instance.WorldToGridPosition(
                _selectedShapeParent.transform.position
            );

            var shape = _selectedShapeParent.GetComponentInChildren<SidelineBlock>().Shape;
            if (PlacementHandler.Instance.TryPlaceShape(targetGridPos, shape))
            {
                // Successfully placed - destroy the original shape
                MonoBehaviour.Destroy(_selectedShapeParent);
            }
            else
            {
                // Return to initial position
                MovementHandler.MoveWithEase(
                    _selectedShapeParent.transform,
                    GridManager.Instance.GridToWorldPosition(_pivotGridPos),
                    50,
                    Easing.OutSine
                );
            }

            _selectedShapeParent = null;
            GhostBlockHandler.Instance.HideGhostBlock();
        }
        
    }
}