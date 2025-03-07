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
        [SerializeField] private List<LevelData> _levelDataList = new List<LevelData>();
        private int _currentLevel;
        public int CurrentLevel =>PlayerPrefs.GetInt(GameConstants.PlayerPrefsLevel, 1);

        public LevelData CurrentLevelData => _levelDataList[0];
    
        private void Start()
        {
            EventBus.Fire(new OnLevelStartEvent());
        }

        private void IncrementLevel()
        {
            _currentLevel = CurrentLevel;
            _currentLevel++;
            PlayerPrefs.SetInt(GameConstants.PlayerPrefsLevel,_currentLevel);
        }
        
    }
}