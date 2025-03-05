using _Game.BlockSystem;
using UnityEngine;
using System.Collections.Generic;
namespace _Game.CoreMechanic
{
    public class PlacementHandler : MonoBehaviour
    {
        public bool TryGetBlockAt( Dictionary<Vector2Int,SidelineBlock> sidelineGrid,Vector2Int gridPos, out SidelineBlock block)
        {
            return sidelineGrid.TryGetValue(gridPos, out block);
        }

        public bool IsGridPositionEmpty(Dictionary<Vector2Int, SidelineBlock> sidelineGrid,Vector2Int gridPos, bool isHorizontal)
        {
            return !sidelineGrid.ContainsKey(gridPos);
        }

        public bool TryPlaceLine(Dictionary<Vector2Int, SidelineBlock> sidelineGrid,Vector2Int gridPos, SidelineBlock lineBlock)
        {
            if (!sidelineGrid.TryAdd(gridPos, lineBlock)) return false;

            MatchHandler.Instance.CheckForSquares(gridPos, lineBlock.IsHorizontal);
            return true;
        }
    }
}