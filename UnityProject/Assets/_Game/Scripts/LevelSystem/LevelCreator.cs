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
        [SerializeField] private List<LevelData> _levelDataList = new List<LevelData>();
        
        private List<SidelineBlock> _sidelineBlocks = new();
        private LevelData _currentLevelData;
        
    public LevelData CurrentLevelData => _currentLevelData;
        public MoveParent OutOfTheSceneTarget => outOfTheSceneTarget;
        public int NumberOfTouchableObjects => numberOfTouchableObjects;
    
        private void Start()
        {
            InitializeLevel();
        }
        
        public void CreateTouchableBlocks()
        {
            _sidelineBlocks.Clear();
            outOfTheSceneTarget.transform.position = new Vector3( 25, outOfTheSceneTarget.transform.position.y, outOfTheSceneTarget.transform.position.z);
    
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
                    Debug.LogWarning("No valid shape found! Possible game over condition.");
                }
            }
            if(_sidelineBlocks.Count>0)
                MoveTouchablesIntoScene();
        }

    
        private SidelineBlock CreateTouchableBlock(Shape shape, int index)
        {
            GameObject shapeParent = new GameObject($"Shape_{shape.ShapeType}");
            shapeParent.transform.SetParent(outOfTheSceneTarget.transform, false);

            Vector3 parentPosition = Vector3.right * ((index - 1) * GridManager.Instance.BlockSize*3);
            shapeParent.transform.localPosition = parentPosition;

            SidelineBlock mainBlock = null;
            foreach (var line in shape.Lines)
            {
                var prefab = line.isHorizontal ? 
                    GridManager.Instance.BlockCatalog.horizontalSidelinePrefab : 
                    GridManager.Instance.BlockCatalog.verticalSidelinePrefab;

                var lineBlock = Instantiate(prefab, shapeParent.transform, true);
                lineBlock.transform.localPosition = (Vector3)(Vector2)line.gridPosition * GridManager.Instance.BlockSize;

                lineBlock.Shape = shape;
                
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
                new Vector3(0, outOfTheSceneTarget.Position.y, 0), 35, Easing.OutBack);
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
            _currentLevelData = _levelDataList[LevelManager.Instance.CurrentLevel];

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
            
            MatchHandler.Instance.CheckAllSquares();
            CreateTouchableBlocks();
            EventBus.Fire(new OnLevelInitializeEvent());
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelStartEvent>(e=>InitializeLevel());
        }
    }
}
