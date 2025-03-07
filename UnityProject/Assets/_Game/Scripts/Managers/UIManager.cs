using System;
using _Game.DataStructures;
using _Game.LevelSystem;
using _Game.Utils;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace _Game.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI movementText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Slider scoreSlider;
        
        private LevelManager _levelManager;
        private ScoreSystem _scoreSystem;
        
        private void Initialize()
        {
            _levelManager = LevelManager.Instance;
            _scoreSystem = ScoreSystem.Instance;
            levelText.text = _levelManager.CurrentLevel.ToString();
            scoreText.text =  0 + " / " + _scoreSystem.TargetScore;
            scoreSlider.value = (float)0 / _scoreSystem.TargetScore;
            movementText.text = _scoreSystem.CurrentMovement + " / " + _scoreSystem.MovementLimit;
        }

        private void UpdatePointUI()
        {
            scoreText.text =  _scoreSystem.CurrentScore + " / " + _scoreSystem.TargetScore;
            scoreSlider.value = (float)_scoreSystem.CurrentScore / _scoreSystem.TargetScore;
        }

        private void UpdateMovementUI()
        {
            movementText.text = _scoreSystem.CurrentMovement + " / " + _scoreSystem.MovementLimit;
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelInitializeEvent>(@event => Initialize());
            EventBus.Subscribe<OnScoreChanged>(@event => UpdatePointUI());
            EventBus.Subscribe<OnMovementCountChanged>(@event => UpdateMovementUI());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelInitializeEvent>(@event => Initialize());
            EventBus.Unsubscribe<OnScoreChanged>(@event => UpdatePointUI());
            EventBus.Unsubscribe<OnMovementCountChanged>(@event => UpdateMovementUI());
        }
    }
}