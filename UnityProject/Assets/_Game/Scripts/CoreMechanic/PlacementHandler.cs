using System.Collections.Generic;
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
        public bool TryPlaceShape(Vector2Int pivotGridPos, Shape shape, bool testOnly = true, Transform parent = null)
        {
            foreach (var line in shape.Lines)
            {
                Vector2Int linePos = pivotGridPos + line.gridPosition;
                if (!GridManager.Instance.IsGridPositionEmpty(linePos, line.isHorizontal) && shape.ShapeType != ShapeType.Jocker)
                    return false;
                if(shape.ShapeType == ShapeType.Jocker && !GridManager.Instance.IsGridPositionValid(linePos, line.isHorizontal,false))
                    return false;
                // if (shape.ShapeType == ShapeType.Jocker &&
                //     GridManager.Instance.IsGridPositionEmpty(linePos, line.isHorizontal))
                //     return false;
            }
            if (!testOnly)
            {
                if (shape.ShapeType == ShapeType.Jocker)
                {
                    List<(Vector2Int, bool)> blockList = new List<(Vector2Int, bool)>();
                    foreach (var line in shape.Lines)
                    {   Vector2Int linePos = pivotGridPos + line.gridPosition;
                        blockList.Add((linePos, line.isHorizontal));
                    }
                    GridManager.Instance.RemoveLines(blockList);
                }
                
                foreach (var line in shape.Lines)
                {
                    Vector2Int linePos = pivotGridPos + line.gridPosition;
                    SidelineBlock lineBlock = Instantiate(
                        line.isHorizontal ? 
                            GridManager.Instance.BlockCatalog.horizontalSidelinePrefab : 
                            GridManager.Instance.BlockCatalog.verticalSidelinePrefab,
                        GridManager.Instance.GridToWorldPosition(linePos),
                        Quaternion.identity, parent
                    );
                    lineBlock.SetPosition(linePos.x, linePos.y, linePos.x, linePos.y);
                    GridManager.Instance.TryPlaceLine(linePos, lineBlock);
                }
                EventBus.Fire(new OnObjectPlacedEvent{ObjectCount = shape.Lines.Count});

            }

            return true;
        }

    }
}