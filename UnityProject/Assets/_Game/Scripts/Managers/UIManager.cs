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
        [SerializeField] private Image scoreSlider;
        
        private LevelManager _levelManager;
        private ScoreSystem _scoreSystem;
        
        private void Initialize()
        {
            _levelManager = LevelManager.Instance;
            _scoreSystem = ScoreSystem.Instance;
            levelText.text = LevelManager.Instance.CurrentLevel.ToString();
            scoreText.text =  _scoreSystem.CurrentScore + " / " + ScoreSystem.Instance.TargetScore;
            scoreSlider.fillAmount = (float)_scoreSystem.CurrentScore / _scoreSystem.TargetScore;
            movementText.text = _scoreSystem.CurrentMovement + " / " + ScoreSystem.Instance.MovementLimit;
        }

        private void UpdatePointUI()
        {
            scoreText.text =  _scoreSystem.CurrentScore + " / " + ScoreSystem.Instance.TargetScore;
            scoreSlider.fillAmount = (float)_scoreSystem.CurrentScore / _scoreSystem.TargetScore;
        }

        private void UpdateMovementUI()
        {
            movementText.text = _scoreSystem.CurrentMovement + " / " + ScoreSystem.Instance.MovementLimit;
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnScoreChanged>(@event => UpdatePointUI());
            EventBus.Subscribe<OnMovementCountChanged>(@event => UpdateMovementUI());
            EventBus.Subscribe<OnLevelInitializeEvent>(@event => Initialize());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnScoreChanged>(@event => UpdatePointUI());
            EventBus.Unsubscribe<OnMovementCountChanged>(@event => UpdateMovementUI());
            EventBus.Unsubscribe<OnLevelInitializeEvent>(@event => Initialize());
        }
    }
}