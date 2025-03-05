using _Game.BlockSystem;
using _Game.Enums;
using _Game.GridSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.InputSystem
{
    public class SelectionHandler
    {
        private SidelineBlock _selectedObject;
        private Vector2 _offset;

        public void SelectClosestObject(Vector2 touchPosition)
        {
            _selectedObject = GridHandler.Instance.GetClosestTouchable(touchPosition);
            if (_selectedObject == null) return;

            _offset = (Vector2)_selectedObject.transform.position - touchPosition;
        }

        public void DragSelectedObject(Vector2 touchPosition)
        {
            if (!_selectedObject) return;
            _selectedObject.transform.position = touchPosition + _offset;
        }

        public void ReleaseSelectedObject()
        {
            if (!_selectedObject) return;

            Vector2 targetPosition = GridHandler.Instance.GetSnappedGridPosition(_selectedObject.transform.position);
            MovementHandler<SidelineBlock>.MoveWithEase(_selectedObject, targetPosition, 50, Easing.OutSine);
            

            _selectedObject = null;
        }
    }
}