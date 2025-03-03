using _Game.Interfaces;
using UnityEngine;

namespace _Game.GridSystem
{
    public abstract class GridObjectBase : IGridObject
    {
        public Vector2Int GridPosition { get; protected set; }
    }
}