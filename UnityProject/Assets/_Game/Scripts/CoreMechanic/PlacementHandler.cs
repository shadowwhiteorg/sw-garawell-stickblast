using _Game.BlockSystem;
using _Game.Managers;
using _Game.Utils;
using UnityEngine;

namespace _Game.CoreMechanic
{
    public class PlacementHandler : Singleton<PlacementHandler>
    {
        public bool TryGetBlockAt(Vector2Int gridPos, out SidelineBlock block)
        {
            return GridManager.Instance.TryGetSidelineBlock(gridPos, out block);
        }

        public bool IsGridPositionEmpty(Vector2Int gridPos, bool isHorizontal)
        {
            return GridManager.Instance.IsGridPositionEmpty(gridPos, isHorizontal);
        }

        public bool TryPlaceLine(Vector2Int gridPos, SidelineBlock lineBlock)
        {
            return GridManager.Instance.TryPlaceLine(gridPos, lineBlock);
        }

        public void TryPlaceBlock(SidelineBlock currentBlock, Vector2Int gridPos)
        {
            if (currentBlock == null) return;

            if (TryPlaceLine(gridPos, currentBlock))
            {
                currentBlock.SetPosition(gridPos.x, gridPos.y, GridManager.Instance.GridToWorldPosition(gridPos).x, GridManager.Instance.GridToWorldPosition(gridPos).y);
            }
            else
            {
                currentBlock.ResetPosition();
            }

            GhostBlockHandler.Instance.HideGhostBlock();
        }
    }
}