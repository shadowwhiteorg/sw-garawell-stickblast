using System;
using System.Collections.Generic;
using _Game.DataStructures;
using _Game.Interfaces;
using _Game.Utils;
using UnityEngine;

namespace _Game.GridSystem
{
    public class GridHandler : Singleton<GridHandler>
    {

        [SerializeField] private int gridSizeX;
        [SerializeField] private int gridSizeY;
        [SerializeField] private float cellSize;

        [SerializeField] private SidelineBlock horizontalSidelinePrefab;
        [SerializeField] private SidelineBlock verticalSidelinePrefab;
        [SerializeField] private DotBlock dotBlockPrefab;
        [SerializeField] private SquareBlock squareBlockPrefab;
        [SerializeField] private GhostBlock ghostDotBlockPrefab, ghostVerticalSidelineBlockPrefab, 
                                            ghostHorizontalSidelineBlockPrefab, ghostSquareBlockPrefab;
        
        public float CellSize => cellSize;
        
        private float _closestDistance;
        private List<GridObjectBase> _blocksToPlace = new List<GridObjectBase>();
        private Dictionary<Vector2Int, SidelineBlock> _sidelineGrid = new();
        private Dictionary<Vector2Int, SquareBlock> _squareGrid = new();
        private HashSet<Vector2Int> _dotGrid = new();
        [SerializeField] private SidelineBlock _closestTouchable;
        private SidelineBlock _sidelineBlockToCreate;
        [SerializeField]private List<SidelineBlock> _interactableTouchables = new List<SidelineBlock>();
        public List<SidelineBlock> InteractableTouchables => _interactableTouchables;
        

        private void InitializeGrids()
        {
            _interactableTouchables = new List<SidelineBlock>();
            InitializeGhostGrid();
            InitializeDotGrid();
            // InitializeSquareGrid();
            // InitializeSideGrid();
        }

        private void InitializeDotGrid()
        {
            _blocksToPlace = new List<GridObjectBase>();
            _blocksToPlace.Clear();
            _blocksToPlace.AddRange(GridPlacer<DotBlock>.Place(gridSizeX+1, gridSizeY+1,cellSize, dotBlockPrefab,this.gameObject));
            
            GridPlacer<GridObjectBase>.PositionTheGridAtCenter(_blocksToPlace,gridSizeX,gridSizeY,cellSize,"GhostParent");
        }

        private void InitializeSquareGrid()
        {
            _blocksToPlace = new List<GridObjectBase>();
            _blocksToPlace.Clear();
            _blocksToPlace.AddRange(GridPlacer<SquareBlock>.Place(gridSizeX, gridSizeY,cellSize, squareBlockPrefab,this.gameObject));
            
            GridPlacer<GridObjectBase>.PositionTheGridAtCenter(_blocksToPlace,gridSizeX,gridSizeY,cellSize,"SquareParent");
        }
        void InitializeSideGrid()
        {
            _blocksToPlace = new List<GridObjectBase>();
            _blocksToPlace.Clear();
            _blocksToPlace.AddRange(GridPlacer<SidelineBlock>.Place(gridSizeX +1, gridSizeY,cellSize, horizontalSidelinePrefab));
            _blocksToPlace.AddRange(GridPlacer<SidelineBlock>.Place(gridSizeX , gridSizeY +1 ,cellSize, verticalSidelinePrefab));
            
            GridPlacer<GridObjectBase>.PositionTheGridAtCenter(_blocksToPlace,gridSizeX,gridSizeY,cellSize,"DotParent");
        }

        void InitializeGhostGrid()
        {
            _blocksToPlace = new List<GridObjectBase>();
            _blocksToPlace.Clear();
            _blocksToPlace.AddRange(GridPlacer<GhostBlock>.Place(gridSizeX+1, gridSizeY+1,cellSize, ghostDotBlockPrefab));
            _blocksToPlace.AddRange(GridPlacer<GhostBlock>.Place(gridSizeX +1, gridSizeY,cellSize, ghostHorizontalSidelineBlockPrefab));
            _blocksToPlace.AddRange(GridPlacer<GhostBlock>.Place(gridSizeX , gridSizeY +1 ,cellSize, ghostVerticalSidelineBlockPrefab));
            
            GridPlacer<GridObjectBase>.PositionTheGridAtCenter(_blocksToPlace,gridSizeX,gridSizeY,cellSize,"DotParent");
        }

        public SidelineBlock CreateSidelineBlock(bool isHorizontal)
        {
            _sidelineBlockToCreate = Instantiate(isHorizontal ? horizontalSidelinePrefab : verticalSidelinePrefab);
            return _sidelineBlockToCreate;
            _interactableTouchables.Add(_sidelineBlockToCreate);
        }

        public void RemoveFromInteractableSidelineBlocks(SidelineBlock touchable) =>_interactableTouchables.Remove(touchable);

        public Vector2 GridCenter()
        {
            return new Vector2(gridSizeX * cellSize / 2, gridSizeY * cellSize / 2);
        }
        public SidelineBlock ClosesTouchable(Vector2 touchPosition)
        {
            _closestTouchable = null;
            _closestDistance = float.MaxValue;

            foreach (var touchable in _interactableTouchables)
            {   touchable.SetWorldPosition(touchable.transform.position);
                if (touchable is IGridObject gridObject)
                {
                    Vector2 touchablePosition = gridObject.WorldPosition;
                    Vector2 touchSize = touchable.TouchSize;

                    if (IsWithinTouchSize(touchPosition, touchablePosition, touchSize))
                    {
                        Debug.Log("Step 5");
                        float distance = Vector2.Distance(touchPosition , touchablePosition);
                        Debug.Log("Distance"+ distance + "Position "+touchablePosition);
                        if (distance < _closestDistance)
                        {
                            _closestDistance = distance;
                            _closestTouchable = touchable;
                        }
                        Debug.Log("Closest Distance "+_closestDistance);
                    }
                }
            }
            return _closestTouchable;
        }
        
        private bool IsWithinTouchSize(Vector2 touchPosition, Vector2 touchablePosition, Vector2 touchableSize)
        {
            return touchPosition.x >= touchablePosition.x - touchableSize.x / 2 &&
                   touchPosition.x <= touchablePosition.x + touchableSize.x / 2 &&
                   touchPosition.y >= touchablePosition.y - touchableSize.y / 2 &&
                   touchPosition.y <= touchablePosition.y + touchableSize.y / 2;
        }

        public void AddToInteractableTouchables(SidelineBlock touchable) =>_interactableTouchables.Add(touchable);
        public void RemoveFromInteractableTouchables(SidelineBlock touchable) =>_interactableTouchables.Remove(touchable);

        private void OnEnable()
        {
            EventBus.Subscribe<LevelInitializeEvent>(e=> InitializeGhostGrid());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<LevelInitializeEvent>(e => InitializeGhostGrid());
        }
    }
}