using System.Data.SqlTypes;
using _Game.DataStructures;
using _Game.Enums;
using _Game.Utils;
using UnityEngine;

namespace _Game.LevelSystem
{
    public class ScoreSystem : Singleton<ScoreSystem>
    {
        [SerializeField] private int unitDefaultPoint;
        [SerializeField] private int unitBlastPoint;
        [SerializeField] private int unitComboBonus;
        
        
        private int _currentScore;
        private int _comboCounter;
        private int _placementCounter;
        private int _movementCounter;
        private bool _isFirstSquare;

        public int CurrentScore => _currentScore;
        public int CurrentMovement => _movementCounter;

        public int TargetScore => LevelManager.Instance.CurrentLevelData.TargetScore;
        public int MovementLimit => LevelManager.Instance.CurrentLevelData.MovementLimit;
        
        
        public void EarnPoint(ScoreType type = default, int count =1 )
        {
            switch (type)
            {
                case ScoreType.Default:
                    _currentScore += unitDefaultPoint;
                    break;
                case ScoreType.Blast:
                    _currentScore += unitBlastPoint*count;
                    break;
            }
            EventBus.Fire(new OnScoreChanged());

            if (_currentScore >= TargetScore)
            {
                EventBus.Fire(new OnLevelWinEvent());
            }
        }

        private void ResetCounters()
        {
            _currentScore = 0;
            _movementCounter = 0;
            _placementCounter = 0;
        }

        public void CountMovements()
        {
            _movementCounter++;
            EventBus.Fire(new OnMovementCountChanged());
            if(_movementCounter >= MovementLimit)
                EventBus.Fire(new OnLevelLoseEvent());
        }
        
        private void CountPlacements()
        {
            _placementCounter++;
            if (_placementCounter >= LevelCreator.Instance.NumberOfTouchableObjects )
            {
                _placementCounter = 0;
                LevelCreator.Instance.CreateTouchableBlocks();
            }
        }
        
        private void OnEnable()
        {
            EventBus.Subscribe<OnObjectPlacedEvent>(@event => CountPlacements());
            EventBus.Subscribe<OnObjectPlacedEvent>(@event => CountMovements());
            EventBus.Subscribe<OnLevelStartEvent>(@event => ResetCounters());
            EventBus.Subscribe<OnSquareCreatedEvent>(@event => EarnPoint(ScoreType.Default));
            EventBus.Subscribe<OnBlastEvent>(@event => EarnPoint(@event.ScoreType,@event.BlastCount));
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnObjectPlacedEvent>(@event => CountPlacements());
            EventBus.Unsubscribe<OnObjectPlacedEvent>(@event => CountMovements());
            EventBus.Unsubscribe<OnLevelStartEvent>(@event => ResetCounters());
            EventBus.Unsubscribe<OnSquareCreatedEvent>(@event => EarnPoint(ScoreType.Default));
            EventBus.Unsubscribe<OnBlastEvent>(@event => EarnPoint(ScoreType.Blast,@event.BlastCount));
        }
    }
}