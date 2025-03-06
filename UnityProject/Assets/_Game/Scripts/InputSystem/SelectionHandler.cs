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
        private SidelineBlock _selectedBlock;
    private Vector2 _dragOffset;
    private Vector2Int _pivotGridPos;

    public SidelineBlock SelectClosestObject(Vector2 touchPosition)
    {
        _selectedBlock = GridHandler.Instance.GetClosestTouchable(touchPosition);
        if (!_selectedBlock) return null;

        // Calculate pivot grid position
        Vector2 worldPos = _selectedBlock.transform.position;
        _pivotGridPos = GridManager.Instance.WorldToGridPosition(worldPos);

        // Lift the block slightly
        MovementHandler<SidelineBlock>.MoveWithEase(
            _selectedBlock, 
            (Vector3)worldPos + InputHandler.Instance.InitialTouchableXOffset * Vector3.up, 
            50, 
            Easing.OutSine
        );
        _dragOffset = (Vector2)_selectedBlock.transform.position - touchPosition;
        return _selectedBlock;
    }

    public void DragSelectedObject(Vector2 touchPosition)
    {
        if (!_selectedBlock) return;
        _selectedBlock.transform.position = touchPosition + _dragOffset;

        // Update ghost positions
        GhostBlockHandler.Instance.ShowGhostShape(
            GridManager.Instance.WorldToGridPosition(touchPosition + _dragOffset), 
            _selectedBlock.Shape
        );
    }

    public void ReleaseSelectedObject()
    {
        if (!_selectedBlock) return;

        Vector2Int targetGridPos = GridManager.Instance.WorldToGridPosition(
            _selectedBlock.transform.position
        );

        if (PlacementHandler.Instance.TryPlaceShape(targetGridPos, _selectedBlock.Shape))
        {
            // Successfully placed - destroy the original block
            MonoBehaviour.Destroy(_selectedBlock.gameObject);
        }
        else
        {
            // Return to initial position
            MovementHandler<SidelineBlock>.MoveWithEase(
                _selectedBlock, 
                GridManager.Instance.GridToWorldPosition(_pivotGridPos), 
                50, 
                Easing.OutSine
            );
        }

        _selectedBlock = null;
        GhostBlockHandler.Instance.HideGhostBlock();
    }
       
        
    }
}