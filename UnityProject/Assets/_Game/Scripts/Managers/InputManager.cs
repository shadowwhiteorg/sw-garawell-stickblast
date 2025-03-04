using System;
using System.Collections.Generic;
using _Game.Enums;
using _Game.GridSystem;
using _Game.Interfaces;
using _Game.Utils;
using UnityEngine;

namespace _Game.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private SidelineBlock testTouchable;
        private float _closestDistance;
        private Camera _camera;
        private List<ITouchable> _interactableTouchables = new List<ITouchable>();
        private ITouchable _closestTouchable;
        private ITouchable _currentTouchable;
        private Vector2 _initialTouchPosition;
        private Vector2 _currentTouchPosition;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _initialTouchPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                _currentTouchable = testTouchable;
                // _closestTouchable = GridHandler.Instance.ClosesTouchable(_camera.ScreenToWorldPoint(Input.mousePosition));
                // if(_closestTouchable == null) return;
                // _currentTouchable = _closestTouchable;
                MovementHandler<SidelineBlock>.MoveWithEase((SidelineBlock)_currentTouchable, _initialTouchPosition, 100,
                    Easing.OutSine);
            }
            
            
        }
    }
}