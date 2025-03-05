using _Game.Managers;
using _Game.Utils;
using UnityEngine;

namespace _Game.BlockSystem
{
    public class GhostBlockHandler : Singleton<GhostBlockHandler>
    {
        [SerializeField] private GameObject ghostBlockPrefab;
        private GameObject _currentGhostBlock;

        public void ShowGhostBlock(Vector2Int gridPos, bool isHorizontal)
        {
            if (_currentGhostBlock == null)
            {
                _currentGhostBlock = Instantiate(ghostBlockPrefab);
            }

            Vector2 worldPos = GridManager.Instance.GridToWorldPosition(gridPos);
            _currentGhostBlock.transform.position = worldPos;
            _currentGhostBlock.SetActive(true);
        }

        public void HideGhostBlock()
        {
            if (_currentGhostBlock != null)
            {
                _currentGhostBlock.SetActive(false);
            }
        }
    }
}