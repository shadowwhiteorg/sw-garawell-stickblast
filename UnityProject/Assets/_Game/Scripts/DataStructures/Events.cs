using UnityEngine;

namespace _Game.DataStructures
{
    public struct TouchBeginEvent { public Vector2 Position; }
    public struct TouchFinishEvent { public Vector2 Position; }
    
    public struct LevelInitializeEvent { }
    public struct LevelStartEvent { }
    
    public struct ObjectPlacedEvent { }
    
}