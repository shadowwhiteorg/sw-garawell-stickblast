using _Game.BlockSystem;
using _Game.CoreMechanic;
using _Game.InputSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Managers
{
    public class InputHandler : Singleton<InputHandler>
    {
        [SerializeField] private float initialTouchableXOffset = 0.5f;
        public float InitialTouchableXOffset => initialTouchableXOffset;

        private SelectionHandler _selectionHandler;
        private Camera _camera;

        private void Awake()
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
    }
}