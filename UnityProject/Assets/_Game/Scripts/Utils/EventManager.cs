using System;
using _Game.DataStructures;
using UnityEngine;
namespace _Game.Utils
{
    public static class EventManager
    {
        public static event Action<Vector2> OnTouch;
        public static event Action OnLevelStart;
        public static void FireOnTouch(Vector2 touchPosition) => OnTouch?.Invoke(touchPosition);
        public static void FireOnLevelStart() => OnLevelStart?.Invoke();
        
        
        

        public static void Test()
        {
            EventBus.Subscribe<TouchEvent>(e => Debug.Log($"Touched at {e.Position}"));
            EventBus.Subscribe<LevelStartEvent>(e => Debug.Log("Level Started"));
        
            EventBus.Fire(new TouchEvent { Position = new Vector2(10, 20) });
            EventBus.Fire(new LevelStartEvent());
            
        }
        
    }
}