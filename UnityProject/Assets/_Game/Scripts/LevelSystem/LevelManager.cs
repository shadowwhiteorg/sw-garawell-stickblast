using System;
using _Game.DataStructures;
using _Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Game.LevelSystem
{
    public class LevelManager : Singleton<LevelManager>
    {
        private int _movementCounter;

        private void OnEnable()
        {
            EventBus.Subscribe<ObjectPlacedEvent>(@event => CountMovements());
        }

        private void CountMovements()
        {
            _movementCounter++;
            if (_movementCounter >= LevelCreator.Instance.NumberOfTouchableObjects )
            {
                _movementCounter = 0;
                LevelCreator.Instance.CreateTouchableBlocks();
            }
        }
    }
}