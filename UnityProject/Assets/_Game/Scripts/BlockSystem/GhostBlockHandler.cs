using System.Collections.Generic;
using _Game.DataStructures;
using _Game.Managers;
using _Game.Utils;
using UnityEngine;

namespace _Game.BlockSystem
{
    public class GhostBlockHandler : Singleton<GhostBlockHandler>
    {
        private GameObject _currentGhost;

        public void ShowGhostShape(Vector2Int pivotGridPos, Shape shape)
        {
            HideGhostBlock(); // Clear existing ghost block

            // Instantiate the shape's ghost prefab
            _currentGhost = Instantiate(
                shape.GhostPrefab,
                GridManager.Instance.GridToWorldPosition(pivotGridPos),
                Quaternion.identity
            );
        }

        public void HideGhostBlock()
        {
            if (_currentGhost)
            {
                Destroy(_currentGhost);
                _currentGhost = null;
            }
        }
    }
}