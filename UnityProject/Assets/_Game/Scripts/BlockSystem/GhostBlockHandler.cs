using System.Collections.Generic;
using _Game.DataStructures;
using _Game.Managers;
using _Game.Utils;
using UnityEngine;

namespace _Game.BlockSystem
{
    public class GhostBlockHandler : Singleton<GhostBlockHandler>
    {
        [SerializeField] private GameObject ghostBlockPrefab;
        private GhostBlock _currentHorizontalGhostBlock;
        private GhostBlock _currentVerticalGhostBlock;
        private List<GameObject> _currentGhosts = new();

        public void ShowGhostShape(Vector2Int pivotGridPos, Shape shape)
        {
            HideGhostBlock(); // Clear existing ghost blocks

            foreach (var line in shape.lines)
            {
                Vector2Int linePos = pivotGridPos + line.offset;
                GameObject ghost = Instantiate(
                    shape.ghostPrefab, // Use the Shape-specific ghost prefab
                    GridManager.Instance.GridToWorldPosition(linePos),
                    Quaternion.identity
                );
                _currentGhosts.Add(ghost);
            }
        }

        public void HideGhostBlock()
        {
            foreach (var ghost in _currentGhosts) Destroy(ghost);
            _currentGhosts.Clear();
        }
    }
}