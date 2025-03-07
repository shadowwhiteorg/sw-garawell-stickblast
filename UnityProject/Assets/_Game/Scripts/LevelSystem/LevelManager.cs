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
        public int CurrentLevel => PlayerPrefs.GetInt(GameConstants.PlayerPrefsLevel, 1);
    
        

        private void Start()
        {
            EventBus.Fire(new OnLevelStartEvent());
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