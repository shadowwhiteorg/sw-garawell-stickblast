using _Game.BlockSystem;
using _Game.DataStructures;
using _Game.Enums;
using _Game.GridSystem;
using _Game.Managers;
using _Game.Utils;
using UnityEngine;

namespace _Game.CoreMechanic
{
    public class PlacementHandler : Singleton<PlacementHandler>
    {
        public bool TryPlaceShape(Vector2Int pivotGridPos, Shape shape, bool testOnly = true)
        {
            foreach (var line in shape.Lines)
            {
                Vector2Int linePos = pivotGridPos + line.gridPosition;
                if (!GridManager.Instance.IsGridPositionEmpty(linePos, line.isHorizontal))
                    return false;
            }
            if (!testOnly)
            {
                EventBus.Fire(new ObjectPlacedEvent());
                foreach (var line in shape.Lines)
                {
                    Vector2Int linePos = pivotGridPos + line.gridPosition;
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
            }

            return true;
        }

    }
}