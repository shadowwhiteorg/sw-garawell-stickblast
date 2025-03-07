using System;
using System.Collections.Generic;
using _Game.DataStructures;
using _Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Game.LevelSystem
{
    public class LevelManager : Singleton<LevelManager>
    {
        private int _movementCounter;
        [SerializeField] private List<LevelData> _levelDataList = new List<LevelData>();
        public int CurrentLevel => PlayerPrefs.GetInt(GameConstants.PlayerPrefsLevel, 1);
        public LevelData CurrentLevelData => _levelDataList[CurrentLevel];
    
        

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