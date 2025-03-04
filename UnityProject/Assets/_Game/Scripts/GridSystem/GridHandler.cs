using System.Collections.Generic;
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
        [SerializeField] private GhostBlock ghostDotBlockPrefab, ghostVerticalSidelineBlockPrefab, ghostHorizontalSidelineBlockPrefab, ghostSquareBlockPrefab;
    
        private Dictionary<Vector2Int, SidelineBlock> _sidelineGrid = new();
        private Dictionary<Vector2Int, SquareBlock> _squareGrid = new();
        private HashSet<Vector2Int> _dotGrid = new();
        
    
        private void Start()
        {
            InitializeGhostGrid();            
        }

        void InitializeGhostGrid()
        {
            GridPlacer<GhostBlock>.Place(gridSizeX , gridSizeX,cellSize, ghostDotBlockPrefab);
            GridPlacer<GhostBlock>.Place(gridSizeX -1 , gridSizeX -1 ,cellSize, ghostHorizontalSidelineBlockPrefab);
            GridPlacer<GhostBlock>.Place(gridSizeX -1 , gridSizeX -1 ,cellSize, ghostVerticalSidelineBlockPrefab);
            GridPlacer<GhostBlock>.Place(gridSizeX -1 , gridSizeX -1 ,cellSize, ghostSquareBlockPrefab);

        }
    
    // void InitializeGrids()
    // {
    //     dotGrid = new bool[gridSizeX + 1, gridSizeY + 1]; // +1 for intersections
    //     horizontalLines = new bool[gridSizeX, gridSizeY + 1];
    //     verticalLines = new bool[gridSizeX + 1, gridSizeY];
    //     squareGrid = new SquareBlock[gridSizeX, gridSizeY];
    //
    //     for (int x = 0; x < gridSizeX; x++)
    //     {
    //         for (int y = 0; y < gridSizeY; y++)
    //         {
    //             squareGrid[x, y] = null; // Initially empty
    //         }
    //     }
    // }
    //
    // void GenerateVisualGrid()
    // {
    //     // Generate Ghost Dot Blocks (at intersection points)
    //     for (int x = 0; x <= gridSizeX; x++)
    //     {
    //         for (int y = 0; y <= gridSizeY; y++)
    //         {
    //             Instantiate(dotBlockPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
    //             dotGrid[x, y] = true; // Mark as present
    //         }
    //     }
    //
    //     // Generate Ghost Horizontal Lines
    //     for (int x = 0; x < gridSizeX; x++)
    //     {
    //         for (int y = 0; y <= gridSizeY; y++)
    //         {
    //             Instantiate(horizontalSidelinePrefab, new Vector3(x + 0.5f, y, 0), Quaternion.identity, transform);
    //             horizontalLines[x, y] = false; // Initially empty
    //         }
    //     }
    //
    //     // Generate Ghost Vertical Lines
    //     for (int x = 0; x <= gridSizeX; x++)
    //     {
    //         for (int y = 0; y < gridSizeY; y++)
    //         {
    //             Instantiate(verticalSidelinePrefab, new Vector3(x, y + 0.5f, 0), Quaternion.identity, transform);
    //             verticalLines[x, y] = false; // Initially empty
    //         }
    //     }
    //
    //     // Generate Ghost Square Blocks
    //     for (int x = 0; x < gridSizeX; x++)
    //     {
    //         for (int y = 0; y < gridSizeY; y++)
    //         {
    //             Instantiate(squareBlockPrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity, transform);
    //             squareGrid[x, y] = null; // Initially empty
    //         }
    //     }
    // }

    // private void GenerateGrid()
    // {
    //     // Place dot blocks at all intersections
    //     for (int x = 0; x <= gridSizeX; x++)
    //     {
    //         for (int y = 0; y <= gridSizeY; y++)
    //         {
    //             Vector3 position = new Vector3(x, 0, y);
    //             Instantiate(dotBlockPrefab, position, Quaternion.identity, transform);
    //         }
    //     }
    //
    //     // Place horizontal sideline ghost blocks
    //     for (int x = 0; x < gridSizeX; x++)
    //     {
    //         for (int y = 0; y <= gridSizeY; y++)
    //         {
    //             Vector3 position = new Vector3(x + 0.5f, 0, y);
    //             Instantiate(horizontalSidelinePrefab, position, Quaternion.identity, transform);
    //         }
    //     }
    //
    //     // Place vertical sideline ghost blocks
    //     for (int x = 0; x <= gridSizeX; x++)
    //     {
    //         for (int y = 0; y < gridSizeY; y++)
    //         {
    //             Vector3 position = new Vector3(x, 0, y + 0.5f);
    //             Instantiate(verticalSidelinePrefab, position, Quaternion.Euler(0, 90, 0), transform);
    //         }
    //     }
    //
    //     // Place square ghost blocks
    //     for (int x = 0; x < gridSizeX; x++)
    //     {
    //         for (int y = 0; y < gridSizeY; y++)
    //         {
    //             Vector3 position = new Vector3(x + 0.5f, 0, y + 0.5f);
    //             Instantiate(squareBlockPrefab, position, Quaternion.identity, transform);
    //         }
    //     }
    // }

    // public void PlaceSidelineBlock(Vector2Int position, bool isHorizontal)
    // {
    //     if (sidelineGrid.ContainsKey(position)) return;
    //     
    //     GameObject prefab = isHorizontal ? horizontalSidelinePrefab : verticalSidelinePrefab;
    //     Instantiate(prefab, new Vector3(position.x, 0, position.y), isHorizontal ? Quaternion.identity : Quaternion.Euler(0, 90, 0), transform);
    //     
    //     SidelineBlock block = new(position, isHorizontal);
    //     sidelineGrid[position] = block;
    //     CheckAndPlaceDotBlock(position);
    //     CheckAndCreateSquare(position);
    // }
    
    // private void CheckAndPlaceDotBlock(Vector2Int position)
    // {
    //     Vector2Int[] cornerOffsets =
    //     {
    //         new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1)
    //     };
    //
    //     foreach (var offset in cornerOffsets)
    //     {
    //         Vector2Int corner = position + offset;
    //         if (dotGrid.Contains(corner)) continue;
    //
    //         bool hasLeft = sidelineGrid.TryGetValue(corner - Vector2Int.right, out var leftBlock) && leftBlock.IsHorizontal;
    //         bool hasRight = sidelineGrid.TryGetValue(corner, out var rightBlock) && rightBlock.IsHorizontal;
    //         bool hasBottom = sidelineGrid.TryGetValue(corner - Vector2Int.up, out var bottomBlock) && !bottomBlock.IsHorizontal;
    //         bool hasTop = sidelineGrid.TryGetValue(corner, out var topBlock) && !topBlock.IsHorizontal;
    //
    //         if ((hasLeft || hasRight) && (hasTop || hasBottom))
    //         {
    //             dotGrid.Add(corner);
    //             Instantiate(dotBlockPrefab, new Vector3(corner.x, 0, corner.y), Quaternion.identity, transform);
    //         }
    //     }
    // }
    
    // private void CheckAndCreateSquare(Vector2Int position)
    // {
    //     Vector2Int[] squareOffsets =
    //     {
    //         new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1)
    //     };
    //     
    //     foreach (var offset in squareOffsets)
    //     {
    //         Vector2Int squarePos = position + offset;
    //         if (IsSquareCompleted(squarePos))
    //         {
    //             squareGrid[squarePos] = new SquareBlock(squarePos);
    //             Instantiate(squareBlockPrefab, new Vector3(squarePos.x + 0.5f, 0, squarePos.y + 0.5f), Quaternion.identity, transform);
    //         }
    //     }
    // }
    
    // private bool IsSquareCompleted(Vector2Int position)
    // {
    //     return sidelineGrid.ContainsKey(position) && sidelineGrid.ContainsKey(position + Vector2Int.right)
    //         && sidelineGrid.ContainsKey(position + Vector2Int.up) && sidelineGrid.ContainsKey(position + Vector2Int.right + Vector2Int.up);
    // }
        
    }
}