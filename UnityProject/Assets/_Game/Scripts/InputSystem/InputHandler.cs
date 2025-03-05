using _Game.Managers;
using _Game.Utils;
using UnityEngine;

namespace _Game.InputSystem
{

    public class InputHandler : MonoBehaviour
    {

        
        
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
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 touchPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                _selectionHandler.SelectClosestObject(touchPosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 currentPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                _selectionHandler.DragSelectedObject(currentPosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _selectionHandler.ReleaseSelectedObject();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                LevelManager.Instance.MoveTouchablesIntoScene();
            }
        }
        
        
        
    }
}