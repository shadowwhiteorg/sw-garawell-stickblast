using System;
using _Game.DataStructures;
using _Game.LevelSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using EventBus = _Game.Utils.EventBus;

namespace _Game.Managers
{
    public class UIManager : Utils.Singleton<UIManager>
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
        [SerializeField] private Button rocketButton;
        [SerializeField] private GameObject activeRocketParent;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip selectSound;
        [SerializeField] private AudioClip dropSound;
        [SerializeField] private AudioClip levelWinSound;
        [SerializeField] private AudioClip levelLoseSound;
        [SerializeField] private AudioClip matchSound;
        [SerializeField] private AudioClip blastSound;
        
        private LevelManager _levelManager;
        private ScoreSystem _scoreSystem;
        private bool _isRocketActive;
        private bool _isLevelStarted;
        
        private void Initialize()
        {
            _levelManager = LevelManager.Instance;
            _scoreSystem = ScoreSystem.Instance;
            levelText.text = _levelManager.CurrentLevel.ToString();
            scoreText.text =  0 + " / " + _scoreSystem.TargetScore;
            scoreSlider.value = (float)0 / _scoreSystem.TargetScore;
            movementText.text = _scoreSystem.CurrentMovement + " / " + _scoreSystem.MovementLimit;
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(() => 
            {
                EventBus.Fire(new OnLevelStartEvent());
            });
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() => 
            {
                EventBus.Fire(new OnLevelStartEvent());
            });
            rocketButton.onClick.RemoveAllListeners();
            rocketButton.onClick.AddListener(() => 
            {
                EventBus.Fire(new OnRocketSelected());
            });
            _isLevelStarted = true;
        }

        private void UpdatePointUI()
        {
            if(!_isLevelStarted) return;
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
            if (isWin)
                audioSource.PlayOneShot(isWin? levelWinSound : levelLoseSound);
        }

        private void OpenLevelStart()
        {
            winLevelPanel?.SetActive(false);
            loseLevelPanel?.SetActive(false);
            inGamePanel?.SetActive(true);
            
        }

        public void PlaySelectSound()
        {
            audioSource.PlayOneShot(selectSound);
        }

        public void PlayMatchSount()
        {
            if(_isLevelStarted)
                audioSource.PlayOneShot(matchSound);
        }

        private void ActivateRocketSkill()
        {
            _isRocketActive = !_isRocketActive;
            activeRocketParent.SetActive(_isRocketActive);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelInitializeEvent>(@event => Initialize());
            EventBus.Subscribe<OnLevelStartEvent>(@event => OpenLevelStart());
            EventBus.Subscribe<OnScoreChanged>(@event => UpdatePointUI());
            EventBus.Subscribe<OnMovementCountChanged>(@event => UpdateMovementUI());
            EventBus.Subscribe<OnLevelWinEvent>(@event => OpenLevelEnd(true));
            EventBus.Subscribe<OnLevelLoseEvent>(@event => OpenLevelEnd(false));
            EventBus.Subscribe<OnObjectPlacedEvent>(@event => audioSource.PlayOneShot(dropSound));
            EventBus.Subscribe<OnSquareCreatedEvent>(@event => PlayMatchSount());
            EventBus.Subscribe<OnBlastEvent>(@event => audioSource.PlayOneShot(blastSound));
            EventBus.Subscribe<OnBlastEvent>(@event => ActivateRocketSkill());
            EventBus.Subscribe<OnRocketSelected>(@event => ActivateRocketSkill());
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelInitializeEvent>(@event => Initialize());
            EventBus.Unsubscribe<OnLevelStartEvent>(@event => OpenLevelStart());
            EventBus.Unsubscribe<OnScoreChanged>(@event => UpdatePointUI());
            EventBus.Unsubscribe<OnMovementCountChanged>(@event => UpdateMovementUI());
            EventBus.Unsubscribe<OnLevelWinEvent>(@event => OpenLevelEnd(true));
            EventBus.Unsubscribe<OnLevelLoseEvent>(@event => OpenLevelEnd(false));
            EventBus.Unsubscribe<OnObjectPlacedEvent>(@event => audioSource.PlayOneShot(dropSound));
            EventBus.Unsubscribe<OnSquareCreatedEvent>(@event =>  PlayMatchSount());
            EventBus.Unsubscribe<OnBlastEvent>(@event => audioSource.PlayOneShot(blastSound));
        }
    }
}