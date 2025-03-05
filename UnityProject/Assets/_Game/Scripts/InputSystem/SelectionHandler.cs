using _Game.BlockSystem;
using _Game.Enums;
using _Game.GridSystem;
using _Game.Managers;
using _Game.Utils;
using UnityEngine;

namespace _Game.InputSystem
{
    public class SelectionHandler
    {
        private SidelineBlock _selectedObject;
        private Vector2 _offset;

        public SidelineBlock SelectClosestObject(Vector2 touchPosition)
        {
            _selectedObject = GridHandler.Instance.GetClosestTouchable(touchPosition);
            if (!_selectedObject) return null;
            MovementHandler<SidelineBlock>.MoveWithEase(_selectedObject, _selectedObject.transform.position + InputHandler.Instance.InitialTouchableXOffset * Vector3.up, 50, Easing.OutSine);
            _offset = (InputHandler.Instance.InitialTouchableXOffset * Vector3.up);
            return _selectedObject;
        }

        public void DragSelectedObject(Vector2 touchPosition)
        {
            if (!_selectedObject) return;
            _selectedObject.transform.position = touchPosition + _offset;
        }

        public void ReleaseSelectedObject()
        {
            if (!_selectedObject) return;

            // Vector2 targetPosition = GridHandler.Instance.GetSnappedGridPosition(_selectedObject.transform.position);
            MovementHandler<SidelineBlock>.MoveWithEase(_selectedObject, _selectedObject.InitialPosition + LevelManager.Instance.OutOfTheSceneTarget.Position, 50, Easing.OutSine);
            

            _selectedObject = null;
        }
    }
}