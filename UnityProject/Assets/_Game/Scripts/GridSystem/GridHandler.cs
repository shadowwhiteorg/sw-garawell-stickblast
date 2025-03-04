using System.Collections.Generic;
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
        [SerializeField] private GhostBlock ghostDotBlockPrefab, ghostVerticalSidelineBlockPrefab, ghostHorizontalSidelineBlockPrefab, ghostSquareBlockPrefab;

        private List<GridObjectBase> _blocksToPlace = new List<GridObjectBase>();
        private Dictionary<Vector2Int, SidelineBlock> _sidelineGrid = new();
        private Dictionary<Vector2Int, SquareBlock> _squareGrid = new();
        private HashSet<Vector2Int> _dotGrid = new();
        
    
        private void Start()
        {
            InitializeGhostGrid();            
        }

        void InitializeGhostGrid()
        {
            _blocksToPlace.Clear();
            // _blocksToPlace.AddRange(GridPlacer<GhostBlock>.Place(gridSizeX , gridSizeY,cellSize, ghostSquareBlockPrefab,this.gameObject));
            _blocksToPlace.AddRange(GridPlacer<GhostBlock>.Place(gridSizeX+1, gridSizeY+1,cellSize, ghostDotBlockPrefab,this.gameObject));
            _blocksToPlace.AddRange(GridPlacer<GhostBlock>.Place(gridSizeX +1, gridSizeY,cellSize, ghostHorizontalSidelineBlockPrefab,this.gameObject));
            _blocksToPlace.AddRange(GridPlacer<GhostBlock>.Place(gridSizeX , gridSizeY +1 ,cellSize, ghostVerticalSidelineBlockPrefab,this.gameObject));
            
            GridPlacer<GridObjectBase>.PositionTheGridAtCenter(_blocksToPlace,gridSizeX,gridSizeY,cellSize,"GhostParent");
        }
        
    }
}