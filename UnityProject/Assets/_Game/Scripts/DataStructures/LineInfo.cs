using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.DataStructures
{
    [Serializable]
    public class LineInfo
    {
        public Vector2Int gridPosition; // Relative to pivot
        public bool isHorizontal; // Orientation of the line
    }
}