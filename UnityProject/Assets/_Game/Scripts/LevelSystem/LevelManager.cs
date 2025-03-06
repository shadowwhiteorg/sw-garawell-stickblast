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
    public class LevelManager : Singleton<LevelManager>
    { 
        [SerializeField] private int numberOfTouchableObjects;
        [SerializeField] private MoveParent outOfTheSceneTarget;
        [SerializeField] private BlockCatalog blockCatalog;
        [SerializeField] private LevelData _levelData;
        private List<SidelineBlock> _sidelineBlocks = new();
    
        public MoveParent OutOfTheSceneTarget => outOfTheSceneTarget;
    
        private void Start()
        {
            InitializeLevel();
        }
        
    
        public void CreateTouchableBlocks()
        {
            _sidelineBlocks.Clear();
    
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
        }

    
        private SidelineBlock CreateTouchableBlock(Shape shape, int index)
        {
            // Create a parent object to hold all lines of the shape
            GameObject shapeParent = new GameObject($"Shape_{shape.ShapeType}");
            shapeParent.transform.SetParent(outOfTheSceneTarget.transform, false);

            // Position the parent object
            Vector3 parentPosition = Vector3.right * ((index - 1) * GridManager.Instance.BlockSize);
            shapeParent.transform.localPosition = parentPosition;

            // Create each line in the shape
            SidelineBlock mainBlock = null;
            foreach (var line in shape.Lines)
            {
                // Determine the prefab based on the line's orientation
                var prefab = line.isHorizontal ? 
                    GridManager.Instance.BlockCatalog.horizontalSidelinePrefab : 
                    GridManager.Instance.BlockCatalog.verticalSidelinePrefab;

                // Instantiate the line
                var lineBlock = Instantiate(prefab, shapeParent.transform, true);
                lineBlock.transform.localPosition = (Vector3)(Vector2)line.offset * GridManager.Instance.BlockSize;

                // Set the shape for the line
                lineBlock.Shape = shape;

                // Register the first line as the main block
                if (mainBlock == null)
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
    
        // Debug method to spawn a random shape at a specified position
        public void SpawnRandomShape(Vector2 worldPosition)
        {
            // Randomly select a shape
            Shape shape = blockCatalog.shapes[Random.Range(0, blockCatalog.shapes.Count)];
    
            // Create the shape at the specified position
            var sidelineBlock = CreateTouchableBlock(shape, 0); // Index 0 for debug
            sidelineBlock.transform.position = worldPosition;
            sidelineBlock.SetWorldPosition(worldPosition);
        }
        private void InitializeLevel()
        {
            // Generate initial lines from LevelData
            foreach (var line in _levelData.initialLines)
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
            EventBus.Fire(new LevelInitializeEvent());
        }
        
    }
}
