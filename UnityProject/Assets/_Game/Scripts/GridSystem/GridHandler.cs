using System.Collections.Generic;
using _Game.BlockSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.GridSystem
{
    public class GridHandler : Singleton<GridHandler>
    {
        private List<SidelineBlock> _interactableTouchables = new();

        public void RegisterTouchable(SidelineBlock touchable) => _interactableTouchables.Add(touchable);
        public void UnregisterTouchable(SidelineBlock touchable) => _interactableTouchables.Remove(touchable);

        public SidelineBlock GetClosestTouchable(Vector2 touchPosition)
        {
            SidelineBlock closestTouchable = null;
            float closestDistance = float.MaxValue;

            foreach (var touchable in _interactableTouchables)
            {
                if (!touchable) return closestTouchable;
                float distance = Vector2.Distance(touchPosition, touchable.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTouchable = touchable;
                }
            }
            return closestTouchable;
        }
    }
}