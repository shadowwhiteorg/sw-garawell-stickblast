using System;
using System.Collections.Generic;

namespace _Game.Utils
{
    using System;
    using System.Collections.Generic;

    public static class EventBus
    {
        private static readonly Dictionary<Type, object> events = new();
        private static readonly object lockObj = new();

        public static void Subscribe<T>(Action<T> listener)
        {
            lock (lockObj)
            {
                Type eventType = typeof(T);
                if (!events.TryGetValue(eventType, out var existing))
                    events[eventType] = new List<Action<T>> { listener };
                else
                    ((List<Action<T>>)existing).Add(listener);
            }
        }

        public static void Unsubscribe<T>(Action<T> listener)
        {
            lock (lockObj)
            {
                Type eventType = typeof(T);
                if (events.TryGetValue(eventType, out var existing))
                {
                    var list = (List<Action<T>>)existing;
                    list.Remove(listener);
                    if (list.Count == 0) events.Remove(eventType);
                }
            }
        }

        public static void Fire<T>(T eventData)
        {
            List<Action<T>> listeners;
            lock (lockObj)
            {
                if (!events.TryGetValue(typeof(T), out var existing)) return;
                listeners = new List<Action<T>>((List<Action<T>>)existing);
            }

            foreach (var listener in listeners)
                listener.Invoke(eventData);
        }
    }


}