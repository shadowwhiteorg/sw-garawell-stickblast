using System.Collections.Generic;
using _Game.BlockSystem;
using _Game.CoreMechanic;
using _Game.DataStructures;
using _Game.GridSystem;
using _Game.InputSystem;
using _Game.Utils;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Managers
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] private int numberOfRows;
        [SerializeField] private int numberOfColumns;
        [SerializeField] private float blockSize;
        [SerializeField] private BlockCatalog blockCatalog;

        private Dictionary<Vector2Int, List<SidelineBlock>> _sidelineGrid = new();
        private Dictionary<Vector2Int, SquareBlock> _squareGrid = new();
        private Dictionary<Vector2, Vector2Int> _positionGrid = new();
        private List<Vector2Int> _blastedSquares = new();
        private SelectionHandler _selectionHandler;

        public BlockCatalog BlockCatalog => blockCatalog;
        public float BlockSize => blockSize;
        public int NumberOfRows => numberOfRows;
        public int NumberOfColumns => numberOfColumns;
        public Dictionary<Vector2Int, List<SidelineBlock>> SidelineGrid => _sidelineGrid;
        public Dictionary<Vector2Int, SquareBlock> SquareGrid => _squareGrid;
        public Dictionary<Vector2, Vector2Int> PositionGrid => _positionGrid;
        public SelectionHandler SelectionHandler => _selectionHandler;
        public bool TryGetSidelineBlock(Vector2Int gridPos, bool isHorizontal, out SidelineBlock block)
        {
            block = null;
            if (_sidelineGrid.TryGetValue(gridPos, out var blocks))
            {
                foreach (var b in blocks)
                {
                    if (b.IsHorizontal == isHorizontal)
                    {
                        block = b;
                        return true;
                    }
                }
            }
            return false;
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
           return gridPos.x >= 0 && gridPos.x < numberOfColumns+1 && 
                  gridPos.y >= 0 && gridPos.y < numberOfRows+1;
       }

        
       public bool IsGridPositionEmpty(Vector2Int gridPos, bool isHorizontal)
       {
           if (!IsGridPositionValid(gridPos)) return false; // Position is outside the grid

           // Check if any block at the position has the same orientation
           if (_sidelineGrid.TryGetValue(gridPos, out var blocks))
           {
               foreach (var block in blocks)
               {
                   if (block.IsHorizontal == isHorizontal)
                   {
                       // A block with the same orientation already exists
                       return false;
                   }
               }
           }

           // No block with the same orientation exists
           return true;
       }

       public bool TryPlaceLine(Vector2Int gridPos, SidelineBlock lineBlock)
       {
           if (!IsGridPositionValid(gridPos)) return false; // Position is outside the grid
           if (!IsGridPositionEmpty(gridPos, lineBlock.IsHorizontal)) return false; // Position is not empty for this orientation

           // Add the block to the list at the grid position
           if (!_sidelineGrid.TryGetValue(gridPos, out var blocks))
           {
               blocks = new List<SidelineBlock>();
               _sidelineGrid[gridPos] = blocks;
           }
           blocks.Add(lineBlock);

           MatchHandler.Instance.CheckForSquares(gridPos, lineBlock.IsHorizontal);
           return true;
       }

       public bool HasHorizontalLine(int x, int y)
       {
           Vector2Int gridPos = new(x, y);
           return _sidelineGrid.ContainsKey(gridPos) && 
                  _sidelineGrid[gridPos].Exists(b => b.IsHorizontal);
       }

       public bool HasVerticalLine(int x, int y)
       {
           Vector2Int gridPos = new(x, y);
           return _sidelineGrid.ContainsKey(gridPos) && 
                  _sidelineGrid[gridPos].Exists(b => !b.IsHorizontal);
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
            _blastedSquares.Clear();

            // Collect all squares in the row
            for (int x = 0; x < numberOfColumns; x++)
            {
                Vector2Int squarePos = new(x, row);
                if (_squareGrid.ContainsKey(squarePos))
                {
                    _blastedSquares.Add(squarePos);
                }
            }

            // Remove squares and their lines
            foreach (var squarePos in _blastedSquares)
            {
                int x = squarePos.x, y = squarePos.y;

                // Remove the square
                if (_squareGrid.Remove(squarePos, out var square))
                {
                    Destroy(square.gameObject);
                }

                // Remove lines if not part of another square
                RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x, y), true);      // Bottom horizontal line
                RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x, y + 1), true);  // Top horizontal line
                RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x, y), false);     // Left vertical line
                RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x + 1, y), false); // Right vertical line
            }

            _blastedSquares.Clear();
        }

        public void BlastColumn(int column)
        {
            _blastedSquares.Clear();

            // Collect all squares in the column
            for (int y = 0; y < numberOfRows; y++)
            {
                Vector2Int squarePos = new(column, y);
                if (_squareGrid.ContainsKey(squarePos))
                {
                    _blastedSquares.Add(squarePos);
                }
            }

            // Remove squares and their lines
            foreach (var squarePos in _blastedSquares)
            {
                int x = squarePos.x, y = squarePos.y;

                // Remove the square
                if (_squareGrid.Remove(squarePos, out var square))
                {
                    Destroy(square.gameObject);
                }

                // Remove lines if not part of another square
                RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x, y), true);      // Bottom horizontal line
                RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x, y + 1), true);  // Top horizontal line
                RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x, y), false);     // Left vertical line
                RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x + 1, y), false); // Right vertical line
            }

            _blastedSquares.Clear();
        }

        private void RemoveLineIfNotPartOfAnotherSquare(Vector2Int gridPos, bool isHorizontal)
        {
            if (!IsLinePartOfAnotherSquare(gridPos, isHorizontal))
            {
                if (_sidelineGrid.TryGetValue(gridPos, out var blocks))
                {
                    // Remove the block with the specified orientation
                    var blockToRemove = blocks.Find(b => b.IsHorizontal == isHorizontal);
                    if (blockToRemove != null)
                    {
                        blocks.Remove(blockToRemove);
                        Destroy(blockToRemove.gameObject);

                        // If no blocks remain at this position, remove the entry from the dictionary
                        if (blocks.Count == 0)
                        {
                            _sidelineGrid.Remove(gridPos);
                        }
                    }
                }
            }
        }
        

        public bool IsLinePartOfAnotherSquare(Vector2Int gridPos, bool isHorizontal)
        {
            if (isHorizontal)
            {
                // Check the square ABOVE the horizontal line
                if (gridPos.y < numberOfRows - 1 && IsSquareComplete(gridPos.x, gridPos.y + 1) && 
                    !_blastedSquares.Contains(new Vector2Int(gridPos.x, gridPos.y + 1)))
                    return true;

                // Check the square BELOW the horizontal line
                if (gridPos.y > 0 && IsSquareComplete(gridPos.x, gridPos.y - 1) && 
                    !_blastedSquares.Contains(new Vector2Int(gridPos.x, gridPos.y - 1)))
                    return true;
            }
            else
            {
                // Check the square to the EAST of the vertical line
                if (gridPos.x < numberOfColumns - 1 && IsSquareComplete(gridPos.x + 1, gridPos.y) && 
                    !_blastedSquares.Contains(new Vector2Int(gridPos.x + 1, gridPos.y)))
                    return true;

                // Check the square to the WEST of the vertical line
                if (gridPos.x > 0 && IsSquareComplete(gridPos.x - 1, gridPos.y) && 
                    !_blastedSquares.Contains(new Vector2Int(gridPos.x - 1, gridPos.y)))
                    return true;
            }

            return false;
        }

        private bool IsSquareComplete(int x, int y)
        {
            return HasHorizontalLine(x, y) && HasHorizontalLine(x, y + 1) &&
                   HasVerticalLine(x, y) && HasVerticalLine(x + 1, y);
        }
        
        
        private void OnEnable()
        {
            EventBus.Subscribe<LevelInitializeEvent>(e=> InitializeGrid() );
        }
    }
}