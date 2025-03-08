using System;
using System.Collections.Generic;
using _Game.DataStructures;
using _Game.Enums;
using _Game.GridSystem;
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
            HideGhostBlock();

            foreach (var line in shape.Lines)
            {
                Vector2Int linePos = pivotGridPos + line.gridPosition;
                if (!GridManager.Instance.IsGridPositionEmpty(linePos, line.isHorizontal,true)&& shape.ShapeType!=ShapeType.Jocker)
                    return;
                if(shape.ShapeType == ShapeType.Jocker && !GridManager.Instance.IsGridPositionValid(linePos, line.isHorizontal,false))
                    return;
            }
            if (!_currentGhost)
                _currentGhost = Instantiate(
                shape.GhostPrefab,
                GridManager.Instance.GridToWorldPosition(pivotGridPos),
                Quaternion.identity
                );
            _currentGhost.transform.position = GridManager.Instance.GridToWorldPosition(pivotGridPos);
        }
        public void HideGhostBlock()
        {
            if (_currentGhost)
            {
                Destroy(_currentGhost);
                _currentGhost = null;
            }
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelStartEvent>(@event => HideGhostBlock());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelStartEvent>(@event => HideGhostBlock());
        }
    }
}