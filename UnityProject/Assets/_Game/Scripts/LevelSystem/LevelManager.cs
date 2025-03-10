﻿using System;
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
        private bool _hasLevelStarted;
        public int CurrentLevel =>PlayerPrefs.GetInt(GameConstants.PlayerPrefsLevel, 1);
        public LevelData CurrentLevelData => _levelDataList.Count > 0 ? _levelDataList[CurrentLevel % _levelDataList.Count] : null;
        
    
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

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelWinEvent>(e=> IncrementLevel() );
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelWinEvent>(e => IncrementLevel());
        }
    }
}