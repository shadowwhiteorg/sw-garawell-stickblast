using System.Collections.Generic;
using _Game.BlockSystem;
using _Game.CoreMechanic;
using _Game.DataStructures;
using _Game.InputSystem;
using _Game.Utils;
using Unity.Collections;
using UnityEngine;

namespace _Game.Managers
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] private int numberOfRows;
        [SerializeField] private int numberOfColumns;
        [SerializeField] private float blockSize;
        [SerializeField] private BlockCatalog blockCatalog;

        private Dictionary<Vector2Int, SidelineBlock> _sidelineGrid = new();
        private Dictionary<Vector2Int, SquareBlock> _squareGrid = new();
        private Dictionary<Vector2, Vector2Int> _positionGrid = new();
        private SelectionHandler _selectionHandler;

        public BlockCatalog BlockCatalog => blockCatalog;
        public float BlockSize => blockSize;
        public int NumberOfRows => numberOfRows;
        public int NumberOfColumns => numberOfColumns;
        public Dictionary<Vector2Int, SidelineBlock> SidelineGrid => _sidelineGrid;
        public Dictionary<Vector2Int, SquareBlock> SquareGrid => _squareGrid;
        public Dictionary<Vector2, Vector2Int> PositionGrid => _positionGrid;
        public SelectionHandler SelectionHandler => _selectionHandler;
        public bool TryGetSidelineBlock(Vector2Int gridPos, out SidelineBlock block)
        {
            return _sidelineGrid.TryGetValue(gridPos, out block);
        }
        
        private void InitializeGrid()
       {
           InitializeDotGrid(numberOfColumns, numberOfRows, blockSize);
           _selectionHandler = new SelectionHandler();
           // InitializeSquareGrid(numberOfColumns, numberOfRows, blockSize);
           // InitializeSideGrid(numberOfColumns, numberOfRows, blockSize);
           // InitializeGhostGrid(numberOfColumns, numberOfRows, blockSize);
       }


       private void InitializeDotGrid(int gridSizeX, int gridSizeY, float cellSize)
       {
           var dots = GridPlacer<DotBlock>.Place(gridSizeX + 1, gridSizeY + 1, cellSize, blockCatalog.dotBlockPrefab, gameObject);
           GridPlacer<DotBlock>.PositionTheGridAtCenter(dots, gridSizeX, gridSizeY, cellSize, "DotParent");


       }


       private void InitializeSquareGrid(int gridSizeX, int gridSizeY, float cellSize)
       {
           var squares = GridPlacer<SquareBlock>.Place(gridSizeX, gridSizeY, cellSize, blockCatalog.squareBlockPrefab,
               gameObject);
           GridPlacer<SquareBlock>.PositionTheGridAtCenter(squares, gridSizeX, gridSizeY, cellSize, "SquareParent");
       }


       private void InitializeSideGrid(int gridSizeX, int gridSizeY, float cellSize)
       {
           var horizontalLines = GridPlacer<SidelineBlock>.Place(gridSizeX + 1, gridSizeY, cellSize,
               blockCatalog.horizontalSidelinePrefab);
           var verticalLines = GridPlacer<SidelineBlock>.Place(gridSizeX, gridSizeY + 1, cellSize,
               blockCatalog.verticalSidelinePrefab);


           GridPlacer<SidelineBlock>.PositionTheGridAtCenter(horizontalLines, gridSizeX, gridSizeY, cellSize, "SidelineParent");
           GridPlacer<SidelineBlock>.PositionTheGridAtCenter(verticalLines, gridSizeX, gridSizeY, cellSize, "SidelineParent");
       }


       private void InitializeGhostGrid(int gridSizeX, int gridSizeY, float cellSize)
       {
           var ghostDots = GridPlacer<GhostBlock>.Place(gridSizeX + 1, gridSizeY + 1, cellSize,
               blockCatalog.ghostDotBlockPrefab);
           var ghostHorizon = GridPlacer<GhostBlock>.Place(gridSizeX + 1, gridSizeY, cellSize,
               blockCatalog.ghostHorizontalSidelineBlockPrefab);
           var ghostVertical = GridPlacer<GhostBlock>.Place(gridSizeX, gridSizeY + 1, cellSize,
               blockCatalog.ghostVerticalSidelineBlockPrefab);


           GridPlacer<GhostBlock>.PositionTheGridAtCenter(ghostDots, gridSizeX, gridSizeY, cellSize, "GhostParent");
           GridPlacer<GhostBlock>.PositionTheGridAtCenter(ghostHorizon, gridSizeX, gridSizeY, cellSize, "GhostParent");
           GridPlacer<GhostBlock>.PositionTheGridAtCenter(ghostVertical, gridSizeX, gridSizeY, cellSize, "GhostParent");
       }
       
       public bool IsGridPositionValid(Vector2Int gridPos)
       {
           return gridPos.x >= 0 && gridPos.x < numberOfColumns && 
                  gridPos.y >= 0 && gridPos.y < numberOfRows;
       }

        
        public bool IsGridPositionEmpty(Vector2Int gridPos, bool isHorizontal)
        {
            if (!IsGridPositionValid(gridPos)) return false; // Position is outside the grid
            return !_sidelineGrid.ContainsKey(gridPos);
        }

        public bool TryPlaceLine(Vector2Int gridPos, SidelineBlock lineBlock)
        {
            if (!_sidelineGrid.TryAdd(gridPos, lineBlock)) return false;
            MatchHandler.Instance.CheckForSquares(gridPos, lineBlock.IsHorizontal);
            return true;
        }

        public bool HasHorizontalLine(int x, int y)
        {
            return _sidelineGrid.TryGetValue(new(x, y), out var line) && line.IsHorizontal;
        }

        public bool HasVerticalLine(int x, int y)
        {
            return _sidelineGrid.TryGetValue(new(x, y), out var line) && !line.IsHorizontal;
        }
        
        public Vector2 GetGridOffset()
        {
            float offsetX = -(numberOfColumns * blockSize) / 2f;
            float offsetY = -(numberOfRows * blockSize) / 2f;
            return new Vector2(offsetX, offsetY);
        }

        public void CreateSquare(int x, int y)
        {
            Vector2Int key = new(x, y);
            if (_squareGrid.ContainsKey(key)) return;

            Vector2 worldPos = this.GridToWorldPosition(key);
            SquareBlock square = Instantiate(blockCatalog.squareBlockPrefab, worldPos, Quaternion.identity, transform);
            square.SetPosition(x, y, worldPos.x, worldPos.y);
            _squareGrid.Add(key, square);

            MatchHandler.Instance.CheckForCompletedLines();
        }

        public void BlastRow(int row)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                Vector2Int key = new(x, row);
                if (_squareGrid.Remove(key, out var square)) Destroy(square.gameObject);
            }
        }

        public void BlastColumn(int column)
        {
            for (int y = 0; y < numberOfRows; y++)
            {
                Vector2Int key = new(column, y);
                if (_squareGrid.Remove(key, out var square)) Destroy(square.gameObject);
            }
        }

        // public Vector2 GridToWorldPosition(Vector2Int gridPos)
        // {
        //     float x = gridPos.x * blockSize;
        //     float y = gridPos.y * blockSize;
        //     return new Vector2(x, y);
        // }
        
        private void OnEnable()
        {
            EventBus.Subscribe<LevelInitializeEvent>(e=> InitializeGrid() );
        }
    }
}