using System.Collections.Generic;
using UnityEngine;

namespace _Game.LevelSystem
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Level System/Level Data")]
    public class LevelData : ScriptableObject
    {
        public int levelNr;
        public int gridWidth = 5;
        public int gridHeight = 5;
        public List<LineInfo> initialLines = new List<LineInfo>();
    }

    [System.Serializable]
    public class LineInfo
    {
        public Vector2Int gridPosition;
        public bool isHorizontal; // true = horizontal, false = vertical
    }
    
}
