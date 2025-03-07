using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.DataStructures
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Level System/Level Data")]
    public class LevelData : ScriptableObject
    {
        public int levelNr;
        public int GridWidth = 5;
        public int GridHeight = 5;
        public List<LineInfo> InitialLines = new List<LineInfo>();
        public int TargetScore = 500;
        public int MovementLimit = 10;
    }
    
}
