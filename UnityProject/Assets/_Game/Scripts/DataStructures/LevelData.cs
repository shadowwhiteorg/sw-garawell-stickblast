using System.Collections.Generic;
using UnityEngine;

namespace _Game.DataStructures
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Level System/Level Data")]
    public class LevelData : ScriptableObject
    {
        public int levelNr;
        public int gridWidth = 5;
        public int gridHeight = 5;
        public List<LineInfo> initialLines = new List<LineInfo>();
    }
    
}
