using _Game.BlockSystem;
using _Game.DataStructures;
using _Game.Enums;
using _Game.Managers;
using _Game.Utils;
using UnityEngine;

namespace _Game.CoreMechanic
{
    public class PlacementHandler : Singleton<PlacementHandler>
    {
        // public bool TryGetBlockAt(Vector2Int gridPos, bool isHorizontal, out SidelineBlock block)
        // {
        //     return GridManager.Instance.TryGetSidelineBlock(gridPos, isHorizontal, out block);
        // }
        //
        // public bool IsGridPositionEmpty(Vector2Int gridPos, bool isHorizontal)
        // {
        //     return GridManager.Instance.IsGridPositionEmpty(gridPos, isHorizontal);
        // }
        //
        // public bool TryPlaceLine(Vector2Int gridPos, SidelineBlock lineBlock)
        // {
        //     return GridManager.Instance.TryPlaceLine(gridPos, lineBlock);
        // }

        // public void TryPlaceBlock(SidelineBlock currentBlock, Vector2Int gridPos)
        // {
        //     if (currentBlock == null) return;
        //
        //     if (GridManager.Instance.IsGridPositionValid(gridPos) && 
        //         TryPlaceLine(gridPos, currentBlock))
        //     {
        //         MovementHandler<SidelineBlock>.MoveWithEase(currentBlock, GridManager.Instance.GridToWorldPosition(gridPos),35,Easing.InOutSine);
        //         currentBlock.SetPosition(gridPos.x, gridPos.y, GridManager.Instance.GridToWorldPosition(gridPos).x, GridManager.Instance.GridToWorldPosition(gridPos).y);
        //         
        //     }
        //     else
        //     {   
        //         // MovementHandler<SidelineBlock>.MoveWithEase(currentBlock, currentBlock.InitialPosition,35,Easing.InOutSine);
        //
        //         GridManager.Instance.SelectionHandler.ReleaseSelectedObject(currentBlock);
        //         currentBlock.ResetPosition();
        //     }
        //
        //     GhostBlockHandler.Instance.HideGhostBlock();
        // }
        
        public bool TryPlaceShape(Vector2Int pivotGridPos, Shape shape)
        {
            // Check if all required positions are empty
            foreach (var line in shape.lines)
            {
                Vector2Int linePos = pivotGridPos + line.offset;
                if (!GridManager.Instance.IsGridPositionEmpty(linePos, line.isHorizontal))
                    return false;
            }

            // Place all lines
            foreach (var line in shape.lines)
            {
                Vector2Int linePos = pivotGridPos + line.offset;
                SidelineBlock lineBlock = Instantiate(
                    line.isHorizontal ? 
                        GridManager.Instance.BlockCatalog.horizontalSidelinePrefab : 
                        GridManager.Instance.BlockCatalog.verticalSidelinePrefab,
                    GridManager.Instance.GridToWorldPosition(linePos),
                    Quaternion.identity
                );
                lineBlock.SetPosition(linePos.x, linePos.y, linePos.x, linePos.y);
                GridManager.Instance.TryPlaceLine(linePos, lineBlock);
            }

            return true;
        }
    }
}