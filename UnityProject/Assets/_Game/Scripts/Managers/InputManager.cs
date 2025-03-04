using System;
using System.Collections.Generic;
using _Game.Interfaces;
using _Game.Utils;
using UnityEngine;
// ReSharper disable All

namespace _Game.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        private float _closestDistance;
        private Camera _camera;
        private List<ITouchable> _interactableTouchables = new List<ITouchable>();
        private ITouchable _closestTouchable;
        private ITouchable _currentTouchable;

        private void Start()
        {
            _camera = Camera.main;
        }

        private ITouchable ClosesTouchable(Vector2 touchPosition)
        {
            _closestTouchable = null;
            _closestDistance = float.MaxValue;

            foreach (var touchable in _interactableTouchables)
            {
                if (touchable is IGridObject gridObject)
                {
                    Vector2 touchablePosition = gridObject.WorldPosition;
                    Vector2 touchSize = touchable.TouchSize;

                    if (IsWithinTouchSize(touchPosition, touchablePosition, touchSize))
                    {
                        float distance = Vector2.SqrMagnitude(touchPosition - touchablePosition);
                        if (distance < _closestDistance)
                        {
                            _closestDistance = distance;
                            _closestTouchable = touchable;
                        }
                    }
                }
            }
            return _closestTouchable;
        }

        private bool IsWithinTouchSize(Vector2 touchPosition, Vector2 touchablePosition, Vector2 touchableSize)
        {
            return touchPosition.x >= touchablePosition.x - touchableSize.x / 2 &&
                   touchPosition.x <= touchablePosition.x + touchableSize.x / 2 &&
                   touchPosition.y >= touchablePosition.y - touchableSize.y / 2 &&
                   touchPosition.y <= touchablePosition.y + touchableSize.y / 2;
        }

        public void AddToInteractableTouchables(ITouchable touchable) =>_interactableTouchables.Add(touchable);
        public void RemoveFromInteractableTouchables(ITouchable touchable) =>_interactableTouchables.Remove(touchable);
    }
}