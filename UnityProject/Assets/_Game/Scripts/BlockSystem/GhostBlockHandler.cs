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

        public void ShowGhostBlock(Vector2Int gridPos, bool isHorizontal)
        {
            GhostBlock ghostBlock = isHorizontal ? _currentHorizontalGhostBlock : _currentVerticalGhostBlock;
            if (!ghostBlock)
            {
                ghostBlock = Instantiate(isHorizontal ? GridManager.Instance.BlockCatalog.ghostHorizontalSidelineBlockPrefab : GridManager.Instance.BlockCatalog.ghostVerticalSidelineBlockPrefab);
                if (isHorizontal) _currentHorizontalGhostBlock = ghostBlock;
                else _currentVerticalGhostBlock = ghostBlock;
            }

            ghostBlock.transform.position = GridManager.Instance.GridToWorldPosition(gridPos);
            ghostBlock.gameObject.SetActive(true);
        }

        public void HideGhostBlock()
        {
            _currentHorizontalGhostBlock?.gameObject.SetActive(false);
            _currentVerticalGhostBlock?.gameObject.SetActive(false);
        }
    }
}