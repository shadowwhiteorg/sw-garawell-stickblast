using System;
using UnityEngine;
namespace _Game.Utils
{
    public static class EventManager
    {
        public static event Action<Vector2> OnTouch;

        public static void FireOnTouch(Vector2 touchPosition)
        {
            OnTouch?.Invoke(touchPosition);
        }
    }
}