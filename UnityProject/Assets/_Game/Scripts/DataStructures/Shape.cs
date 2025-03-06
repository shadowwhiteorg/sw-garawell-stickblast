using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.DataStructures
{
    [Serializable]
    public class Shape
    {
        public string name;
        public List<LineInfo> lines; // List of lines in the Shape
        public GameObject ghostPrefab;       // Ghost prefab for this Shape
    }
    
}