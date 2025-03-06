using System.Collections.Generic;
using _Game.BlockSystem;
using UnityEngine;

namespace _Game.DataStructures
{
    [CreateAssetMenu(fileName = "BlockCatalog", menuName = "Game/Block Catalog")]
    public class BlockCatalog : ScriptableObject
    {
        public List<Shape> shapes;
        
        public SidelineBlock horizontalSidelinePrefab;
        public SidelineBlock verticalSidelinePrefab;
        public DotBlock dotBlockPrefab;
        public SquareBlock squareBlockPrefab;
        public GhostBlock ghostDotBlockPrefab;
        public GhostBlock ghostVerticalSidelineBlockPrefab;
        public GhostBlock ghostHorizontalSidelineBlockPrefab;
    }
}