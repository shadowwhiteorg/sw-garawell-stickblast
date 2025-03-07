using _Game.Enums;
using UnityEngine;

namespace _Game.DataStructures
{
    public struct TouchBeginEvent { public Vector2 Position; }
    public struct TouchFinishEvent { public Vector2 Position; }
    
    public struct OnLevelInitializeEvent { }
    public struct OnLevelStartEvent { }
    public struct OnLevelLoseEvent {}
    public struct OnLevelWinEvent {}
    public struct OnLevelEndEvent { }
    
    public struct OnScoreChanged {}
    public struct OnMovementCountChanged {}
    
    public struct OnObjectPlacedEvent
    {
        public int ObjectCount;
    }

    public struct OnBlastEvent 
    {
        public ScoreType ScoreType;
        public int BlastCount;
    }
    

}