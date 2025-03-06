using System;
using UnityEngine;

namespace _Game.DataStructures
{
    [Serializable]
    public class LineInfo
    {
        public Vector2Int offset; // Relative to pivot
        public bool isHorizontal; // Orientation of the line
    }
}