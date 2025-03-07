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
        [SerializeField] private GameObject winLevelPanel;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private GameObject loseLevelPanel;
        [SerializeField] private Button restartButton;
        
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
            nextLevelButton.onClick.AddListener(() => 
            {
                EventBus.Fire(new OnLevelStartEvent());
            });
            restartButton.onClick.AddListener(() => 
            {
                EventBus.Fire(new OnLevelStartEvent());
            });
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
        
        private void OpenLevelEnd(bool isWin)
        {
            inGamePanel?.SetActive(false);
            winLevelPanel?.SetActive(isWin);
            loseLevelPanel?.SetActive(!isWin);
        }

        private void OpenLevelStart()
        {
            winLevelPanel?.SetActive(false);
            loseLevelPanel?.SetActive(false);
            inGamePanel?.SetActive(true);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelInitializeEvent>(@event => Initialize());
            EventBus.Subscribe<OnLevelStartEvent>(@event => OpenLevelStart());
            EventBus.Subscribe<OnScoreChanged>(@event => UpdatePointUI());
            EventBus.Subscribe<OnMovementCountChanged>(@event => UpdateMovementUI());
            EventBus.Subscribe<OnLevelWinEvent>(@event => OpenLevelEnd(true));
            EventBus.Subscribe<OnLevelLoseEvent>(@event => OpenLevelEnd(false));
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelInitializeEvent>(@event => Initialize());
            EventBus.Unsubscribe<OnLevelStartEvent>(@event => OpenLevelStart());
            EventBus.Unsubscribe<OnScoreChanged>(@event => UpdatePointUI());
            EventBus.Unsubscribe<OnMovementCountChanged>(@event => UpdateMovementUI());
            EventBus.Unsubscribe<OnLevelWinEvent>(@event => OpenLevelEnd(true));
            EventBus.Unsubscribe<OnLevelLoseEvent>(@event => OpenLevelEnd(false));
        }
    }
}