using System.Collections.Generic;
using _Game.BlockSystem;
using _Game.Managers;
using _Game.Utils;
using UnityEngine;

namespace _Game.GridSystem
{
    public class GridHandler : Singleton<GridHandler>
    {
        private List<SidelineBlock> _interactableTouchables = new();
        public SidelineBlock closestBlock;

        public void RegisterTouchable(SidelineBlock touchable) => _interactableTouchables.Add(touchable);
        public void UnregisterTouchable(SidelineBlock touchable) => _interactableTouchables.Remove(touchable);

        public SidelineBlock GetClosestTouchable(Vector2 touchPosition)
        {
            SidelineBlock closestTouchable = null;
            float closestDistance = InputHandler.Instance.SelectRange;

            foreach (var touchable in _interactableTouchables)
            {
                if (touchable == null) continue; // Skip null touchables
                
                float distance = Vector2.Distance(touchPosition, touchable.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTouchable = touchable;
                }
            }

            closestBlock = closestTouchable;
            return closestTouchable;
        }
    }
}