using _Game.Enums;
using UnityEngine;

namespace _Game.DataStructures
{
    
    public struct OnLevelInitializeEvent { }
    public struct OnLevelStartEvent { }
    public struct OnLevelLoseEvent {}
    public struct OnLevelWinEvent {}
    public struct OnRocketSelected {}
    
    public struct OnScoreChanged {}
    public struct OnMovementCountChanged {}
    
    public struct OnObjectPlacedEvent
    {
        public int ObjectCount;
    }
    
    public struct OnSquareCreatedEvent{}

    public struct OnBlastEvent 
    {
        public ScoreType ScoreType;
        public int BlastCount;
    }
    

}