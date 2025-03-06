using System;
using System.Collections.Generic;
using _Game.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.DataStructures
{
    [Serializable]
    public class Shape
    {
        public ShapeType ShapeType;
        public List<LineInfo> Lines; // List of Lines in the Shape
        public GameObject GhostPrefab;       // Ghost prefab for this Shape
    }
    
}