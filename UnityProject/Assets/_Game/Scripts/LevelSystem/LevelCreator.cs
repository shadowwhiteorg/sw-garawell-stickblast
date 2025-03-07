using System;
using System.Collections.Generic;
using _Game.BlockSystem;
using _Game.CoreMechanic;
using _Game.DataStructures;
using _Game.Enums;
using _Game.GridSystem;
using _Game.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.LevelSystem
{
    public class LevelCreator : Singleton<LevelCreator>
    { 
        [SerializeField] private int numberOfTouchableObjects;
        [SerializeField] private MoveParent outOfTheSceneTarget;
        [SerializeField] private BlockCatalog blockCatalog;
        
        private LevelData _currentLevelData;
        private List<SidelineBlock> _sidelineBlocks = new();
        private bool _isLevelEnd;
        
        public MoveParent OutOfTheSceneTarget => outOfTheSceneTarget;
        public int NumberOfTouchableObjects => numberOfTouchableObjects;
        
        
        public void CreateTouchableBlocks()
        {
            for (int i = 0; i < outOfTheSceneTarget.transform.childCount; i++)
            {
                Destroy(outOfTheSceneTarget.transform.GetChild(i).gameObject);
            }
            _sidelineBlocks.Clear();
            outOfTheSceneTarget.transform.position = new Vector3( 25, outOfTheSceneTarget.transform.position.y, outOfTheSceneTarget.transform.position.z);
            if (_isLevelEnd) return;
            for (int i = 0; i < numberOfTouchableObjects; i++)
            {
                SidelineBlock sidelineBlock = null;
            
                // Try to find a valid shape
                for (int attempt = 0; attempt < blockCatalog.shapes.Count; attempt++)
                {
                    Shape shape = blockCatalog.shapes[Random.Range(0, blockCatalog.shapes.Count)];
            
                    if (GridManager.Instance.CanPlaceShapeAnywhere(shape, out Vector2Int validPosition))
                    {
                        sidelineBlock = CreateTouchableBlock(shape, i);
                        break;
                    }
                }
            
                if (sidelineBlock != null)
                {
                    _sidelineBlocks.Add(sidelineBlock);
                }
                else
                {
                    // TODO: Fire Game Over Event Here!!!
                    EventBus.Fire(new OnLevelLoseEvent());
                    Debug.LogWarning("No valid shape found! Possible game over condition.");
                }
            }
            if(_sidelineBlocks.Count>0)
                MoveTouchablesIntoScene();
        }

    
        private SidelineBlock CreateTouchableBlock(Shape shape, int index)
        {
            
            GameObject shapeParent = new GameObject($"Shape_{shape.ShapeType}");
            // shapeParent.transform.SetParent(transform);
            shapeParent.transform.SetParent(outOfTheSceneTarget.transform, false);

            Vector3 parentPosition = Vector3.right * ((index - 1) * GridManager.Instance.BlockSize*2);
            shapeParent.transform.localPosition = parentPosition;
            Instantiate(shape.VisualPrefab,shapeParent.transform.position, Quaternion.identity,shapeParent.transform); 
            SidelineBlock mainBlock = null;
            foreach (var line in shape.Lines)
            {
                var prefab = line.isHorizontal ? 
                    GridManager.Instance.BlockCatalog.horizontalSidelinePrefab : 
                    GridManager.Instance.BlockCatalog.verticalSidelinePrefab;

                var lineBlock = Instantiate(prefab, shapeParent.transform, true);
                lineBlock.transform.localPosition = (Vector3)(Vector2)line.gridPosition * GridManager.Instance.BlockSize;

                lineBlock.Shape = shape;
                lineBlock.ShowModel(false);
                
                if (!mainBlock)
                {
                    mainBlock = lineBlock;
                    mainBlock.LockUnlockMove(false);
                    mainBlock.SetInitialPosition(parentPosition);
                    mainBlock.SetWorldPosition(mainBlock.transform.position);
                    GridHandler.Instance.RegisterTouchable(mainBlock);
                }
            }

            return mainBlock;
        }
        
        public void MoveTouchablesIntoScene()
        {
            MovementHandler.MoveWithEase(outOfTheSceneTarget,
                new Vector3(-1.5f, outOfTheSceneTarget.Position.y, 0), 35, Easing.OutBack);
        }
    
        public void SpawnRandomShape(Vector2 worldPosition)
        {
            Shape shape = blockCatalog.shapes[Random.Range(0, blockCatalog.shapes.Count)];
    
            var sidelineBlock = CreateTouchableBlock(shape, 0); // Index 0 for debug
            sidelineBlock.transform.position = worldPosition;
            sidelineBlock.SetWorldPosition(worldPosition);
        }
        private void InitializeLevel()
        {
            _isLevelEnd = false;
            _currentLevelData = LevelManager.Instance.CurrentLevelData;
            foreach (var line in _currentLevelData.InitialLines)
            {
                Vector2 worldPos = GridManager.Instance.GridToWorldPosition(line.gridPosition);
                SidelineBlock prefab = line.isHorizontal ? 
                    GridManager.Instance.BlockCatalog.horizontalSidelinePrefab : 
                    GridManager.Instance.BlockCatalog.verticalSidelinePrefab;

                SidelineBlock lineBlock = Instantiate(prefab, worldPos, Quaternion.identity);
                lineBlock.SetPosition(line.gridPosition.x, line.gridPosition.y, worldPos.x, worldPos.y);
                GridManager.Instance.TryPlaceLine(line.gridPosition, lineBlock);
            }
            
            EventBus.Fire(new OnLevelInitializeEvent());
            CreateTouchableBlocks();
            // MatchHandler.Instance.CheckAllSquares();
        }

        private void ResetLevel()
        {
            _isLevelEnd = true;
            for (int i = 0; i < outOfTheSceneTarget.transform.childCount; i++)
            {
                Destroy(outOfTheSceneTarget.transform.GetChild(i).gameObject);
            }
        }
        

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelStartEvent>(e=>InitializeLevel());
            EventBus.Subscribe<OnLevelWinEvent>(@event => ResetLevel());
            EventBus.Subscribe<OnLevelLoseEvent>(@event => ResetLevel());
        }private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelStartEvent>(e=>InitializeLevel());
            EventBus.Unsubscribe<OnLevelWinEvent>(@event => ResetLevel());
            EventBus.Unsubscribe<OnLevelLoseEvent>(@event => ResetLevel());
        }
    }
}
