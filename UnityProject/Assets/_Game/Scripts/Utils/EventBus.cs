using System;
using System.Collections.Generic;

namespace _Game.Utils
{

    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> events = new();

        public static void Subscribe<T>(Action<T> listener)
        {
            Type eventType = typeof(T);
            if (!events.ContainsKey(eventType))
                events[eventType] = null;

            events[eventType] = (Action<T>)events[eventType] + listener;
        }

        public static void Unsubscribe<T>(Action<T> listener)
        {
            Type eventType = typeof(T);
            if (events.ContainsKey(eventType))
            {
                events[eventType] = (Action<T>)events[eventType] - listener;
                if (events[eventType] == null) events.Remove(eventType);
            }
        }

        public static void Fire<T>(T eventData)
        {
            Type eventType = typeof(T);
            if (events.TryGetValue(eventType, out var del))
                (del as Action<T>)?.Invoke(eventData);
        }
    }


}