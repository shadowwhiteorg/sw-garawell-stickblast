using System.Collections.Generic;
using _Game.BlockSystem;
using _Game.CoreMechanic;
using _Game.DataStructures;
using _Game.InputSystem;
using _Game.LevelSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.GridSystem
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

        private void InitializeGrid()
        {
            ClearGrid();
            _selectionHandler = new SelectionHandler();
            InitializeDotGrid(numberOfColumns, numberOfRows, blockSize);
            InitializeSideGrid(numberOfColumns, numberOfRows, blockSize);
        }

        private void InitializeSideGrid(int gridSizeX, int gridSizeY, float cellSize)
        {
            var horizontalLines = GridPlacer<SidelineBlock>.Place(gridSizeX + 1, gridSizeY, cellSize, blockCatalog.ghostHorizontalSidelineBlockPrefab, this.gameObject);
            var verticalLines = GridPlacer<SidelineBlock>.Place(gridSizeX, gridSizeY + 1, cellSize, blockCatalog.ghostVerticalSidelineBlockPrefab, this.gameObject);

            GridPlacer<SidelineBlock>.PositionTheGridAtCenter(horizontalLines, gridSizeX, gridSizeY, cellSize, "SidelineParent", this.transform);
            GridPlacer<SidelineBlock>.PositionTheGridAtCenter(verticalLines, gridSizeX, gridSizeY, cellSize, "SidelineParent", this.transform);
        }

        private void InitializeDotGrid(int gridSizeX, int gridSizeY, float cellSize)
        {
            var dots = GridPlacer<DotBlock>.Place(gridSizeX + 1, gridSizeY + 1, cellSize, blockCatalog.dotBlockPrefab, this.gameObject);
            GridPlacer<DotBlock>.PositionTheGridAtCenter(dots, gridSizeX, gridSizeY, cellSize, "DotParent", this.transform);
        }

        public bool CanPlaceShapeAnywhere(Shape shape, out Vector2Int validPosition)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                for (int y = 0; y < numberOfRows; y++)
                {
                    Vector2Int gridPos = new Vector2Int(x, y);
                    if (PlacementHandler.Instance.TryPlaceShape(gridPos, shape, this.transform))
                    {
                        validPosition = gridPos;
                        return true;
                    }
                }
            }

            validPosition = Vector2Int.zero;
            return false;
        }

        public bool IsGridPositionValid(Vector2Int gridPos, bool isHorizontal = false, bool checkForGhost = false)
        {
            int columns = isHorizontal ? numberOfColumns : numberOfColumns + 1;
            int rows = isHorizontal ? numberOfRows + 1 : numberOfRows;
            return gridPos.x >= 0 && gridPos.x < columns && gridPos.y >= 0 && gridPos.y < rows;
        }

        public bool IsGridPositionEmpty(Vector2Int gridPos, bool isHorizontal, bool checkForGhost = false)
        {
            if (!IsGridPositionValid(gridPos, isHorizontal, checkForGhost)) return false;

            if (_sidelineGrid.TryGetValue(gridPos, out var blocks))
            {
                foreach (var block in blocks)
                {
                    if (block.IsHorizontal == isHorizontal)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        

        public bool TryPlaceLine(Vector2Int gridPos, SidelineBlock lineBlock)
        {
            if (!IsGridPositionValid(gridPos) || !IsGridPositionEmpty(gridPos, lineBlock.IsHorizontal)) return false;

            if (!_sidelineGrid.TryGetValue(gridPos, out var blocks))
            {
                blocks = new List<SidelineBlock>();
                _sidelineGrid[gridPos] = blocks;
            }
            blocks.Add(lineBlock);
            lineBlock.ShowModel(true);
            lineBlock.transform.SetParent(this.transform);
            MatchHandler.Instance.CheckForSquares(gridPos, lineBlock.IsHorizontal);
            LevelCreator.Instance.CheckForValidPlacement();
            return true;
        }

        public bool HasHorizontalLine(int x, int y)
        {
            Vector2Int gridPos = new(x, y);
            return _sidelineGrid.ContainsKey(gridPos) && _sidelineGrid[gridPos].Exists(b => b.IsHorizontal);
        }

        public bool HasVerticalLine(int x, int y)
        {
            Vector2Int gridPos = new(x, y);
            return _sidelineGrid.ContainsKey(gridPos) && _sidelineGrid[gridPos].Exists(b => !b.IsHorizontal);
        }

        public void CreateSquare(int x, int y)
        {
            Vector2Int key = new(x, y);
            if (_squareGrid.ContainsKey(key)) return;

            Vector2 worldPos = this.GridToWorldPosition(key);
            SquareBlock square = Instantiate(blockCatalog.squareBlockPrefab, worldPos, Quaternion.identity, transform);
            square.InitializeVisually();
            square.SetPosition(x, y, worldPos.x, worldPos.y);
            _squareGrid.Add(key, square);
            EventBus.Fire(new OnSquareCreatedEvent());
            MatchHandler.Instance.CheckForCompletedLines();
        }

        public void BlastRow(int row)
        {
            _blastedSquares.Clear();

            for (int x = 0; x < numberOfColumns; x++)
            {
                Vector2Int squarePos = new(x, row);
                if (_squareGrid.ContainsKey(squarePos))
                {
                    _blastedSquares.Add(squarePos);
                }
            }

            foreach (var squarePos in _blastedSquares)
            {
                int x = squarePos.x, y = squarePos.y;

                if (_squareGrid.Remove(squarePos, out var square))
                {
                    square.Blast();
                }

                RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x, y), true);
                RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x, y + 1), true);
                RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x, y), false);
                RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x + 1, y), false);
            }

            _blastedSquares.Clear();
        }

        public Vector2Int? GetClickedSquare(Vector2 worldPosition)
        {
            Vector2Int gridPos = this.WorldToGridPosition(worldPosition);

            if (_squareGrid.ContainsKey(gridPos))
            {
                return gridPos;
            }

            return null;
        }

        public void BlastSquare(Vector2Int squarePos)
        {
            if (!_squareGrid.ContainsKey(squarePos)) return;

            _blastedSquares.Clear();
            _blastedSquares.Add(squarePos);

            int x = squarePos.x, y = squarePos.y;

            if (_squareGrid.Remove(squarePos, out var square))
            {
                square.Blast();
            }

            RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x, y), true);
            RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x, y + 1), true);
            RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x, y), false);
            RemoveLineIfNotPartOfAnotherSquare(new Vector2Int(x + 1, y), false);

            EventBus.Fire(new OnBlastEvent { BlastCount = 1 });
        }

        public void BlastColumn(int column)
        {
            _blastedSquares.Clear();

            for (int y = 0; y < numberOfRows; y++)
            {
                Vector2Int squarePos = new(column, y);
                if (_squareGrid.ContainsKey(squarePos))
                {
                    _blastedSquares.Add(squarePos);
                }
            }

            foreach (var squarePos in _blastedSquares)
            {
                int x = squarePos.x, y = squarePos.y;

                if (_squareGrid.Remove(squarePos, out var square))
                {
                    square.Blast();
                }

                RemoveLineForRocketBlast(new Vector2Int(x, y), true);
                RemoveLineForRocketBlast(new Vector2Int(x, y + 1), true);
                RemoveLineForRocketBlast(new Vector2Int(x, y), false);
                RemoveLineForRocketBlast(new Vector2Int(x + 1, y), false);
            }
            EventBus.Fire(new OnBlastEvent { BlastCount = _blastedSquares.Count });
            _blastedSquares.Clear();
        }

        private void RemoveLineIfNotPartOfAnotherSquare(Vector2Int gridPos, bool isHorizontal)
        {
            if (!IsLinePartOfAnotherSquare(gridPos, isHorizontal))
            {
                if (_sidelineGrid.TryGetValue(gridPos, out var blocks))
                {
                    var blockToRemove = blocks.Find(b => b.IsHorizontal == isHorizontal);
                    if (blockToRemove != null)
                    {
                        blocks.Remove(blockToRemove);
                        Destroy(blockToRemove.gameObject);

                        if (blocks.Count == 0)
                        {
                            _sidelineGrid.Remove(gridPos);
                        }
                    }
                }
            }
        }

        private void RemoveLineForRocketBlast(Vector2Int gridPos, bool isHorizontal)
        {
            if (_sidelineGrid.TryGetValue(gridPos, out var blocks))
            {
                var blockToRemove = blocks.Find(b => b.IsHorizontal == isHorizontal);
                if (blockToRemove != null)
                {
                    blocks.Remove(blockToRemove);
                    Destroy(blockToRemove.gameObject);

                    if (blocks.Count == 0)
                    {
                        _sidelineGrid.Remove(gridPos);
                    }
                }
            }
        }

        public void RemoveLines(List<(Vector2Int, bool)> blockList)
        {
            for (int i = 0; i < blockList.Count; i++)
            {
                if (_sidelineGrid.TryGetValue(blockList[i].Item1, out var blocks))
                {
                    var blockToRemove = blocks.Find(b => b.IsHorizontal == blockList[i].Item2);
                    if (blockToRemove != null)
                    {
                        blocks.Remove(blockToRemove);
                        Destroy(blockToRemove.gameObject);

                        if (blocks.Count == 0)
                        {
                            _sidelineGrid.Remove(blockList[i].Item1);
                        }
                    }
                }
            }
        }

        public bool IsLinePartOfAnotherSquare(Vector2Int gridPos, bool isHorizontal)
        {
            if (isHorizontal)
            {
                if (gridPos.y < numberOfRows - 1 && IsSquareComplete(gridPos.x, gridPos.y + 1) && !_blastedSquares.Contains(new Vector2Int(gridPos.x, gridPos.y + 1)))
                    return true;

                if (gridPos.y > 0 && IsSquareComplete(gridPos.x, gridPos.y - 1) && !_blastedSquares.Contains(new Vector2Int(gridPos.x, gridPos.y - 1)))
                    return true;
            }
            else
            {
                if (gridPos.x < numberOfColumns - 1 && IsSquareComplete(gridPos.x + 1, gridPos.y) && !_blastedSquares.Contains(new Vector2Int(gridPos.x + 1, gridPos.y)))
                    return true;

                if (gridPos.x > 0 && IsSquareComplete(gridPos.x - 1, gridPos.y) && !_blastedSquares.Contains(new Vector2Int(gridPos.x - 1, gridPos.y)))
                    return true;
            }

            return false;
        }

        private bool IsSquareComplete(int x, int y)
        {
            return HasHorizontalLine(x, y) && HasHorizontalLine(x, y + 1) && HasVerticalLine(x, y) && HasVerticalLine(x + 1, y);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelStartEvent>(e => InitializeGrid());
            EventBus.Subscribe<OnLevelWinEvent>(e => ClearGrid());
            EventBus.Subscribe<OnLevelLoseEvent>(e => ClearGrid());
        }

        public void ClearGrid()
        {
            _sidelineGrid.Clear();
            _squareGrid.Clear();

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void InitializeGrid(LevelData levelData)
        {
            ClearGrid();
            numberOfColumns = levelData.GridWidth;
            numberOfRows = levelData.GridHeight;
            InitializeGrid();
        }
    }
}