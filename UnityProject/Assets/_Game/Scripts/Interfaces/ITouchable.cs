using UnityEngine;

namespace _Game.Interfaces
{
    public interface ITouchable
    {
        Vector2 TouchSize { get; set; }
        void SetTouchSize(float xRange, float yRange);
    }
}