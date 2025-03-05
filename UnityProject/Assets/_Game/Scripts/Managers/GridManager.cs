using System;
using System.Collections.Generic;
using _Game.BlockSystem;
using _Game.DataStructures;
using _Game.Utils;
using UnityEngine;

namespace _Game.Managers
{

    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] private int numberOfRows;
        [SerializeField] private int numberOfColumns;
        [SerializeField] private float blockSize;
        [SerializeField] BlockCatalog blockCatalog;
        
        private Dictionary<Vector2Int, SidelineBlock> _sidelineGrid = new();
        private Dictionary<Vector2Int, SquareBlock> _squareGrid = new();
        
        public BlockCatalog BlockCatalog => blockCatalog;
        public float BlockSize => blockSize;
        

        private void InitializeGrid()
        {
            InitializeDotGrid(numberOfColumns, numberOfRows, blockSize);
            InitializeSquareGrid(numberOfColumns, numberOfRows, blockSize);
            InitializeSideGrid(numberOfColumns, numberOfRows, blockSize);
            InitializeGhostGrid(numberOfColumns, numberOfRows, blockSize);
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

        private void OnEnable()
        {
            EventBus.Subscribe<LevelInitializeEvent>(e=> InitializeGrid() );
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<LevelInitializeEvent>(e=> InitializeGrid() );
        }
    }
}
