using _Game.DataStructures;
using _Game.Enums;
using _Game.Utils;
using UnityEngine;

namespace _Game.LevelSystem
{
    public class ScoreSystem : MonoBehaviour
    {
        [SerializeField] private int unitDefaultPoint;
        [SerializeField] private int unitBlastPoint;
        [SerializeField] private int unitComboBonus;
        
        
        private int _currentScore;
        private int _comboCounter;
        private int _placementCounter;
        private int _movementCounter;

        private int TargetScore() => LevelCreator.Instance.CurrentLevelData.TargetScore;
        private int MovementLimit() => LevelCreator.Instance.CurrentLevelData.MovementLimit;
        
        
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

            if (_currentScore >= TargetScore())
            {
                EventBus.Fire(new OnLevelWinEvent());
            }
        }

        public void CountMovements()
        {
            _movementCounter++;
            if(_movementCounter >= MovementLimit())
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
            EventBus.Subscribe<OnObjectPlacedEvent>(@event => EarnPoint(ScoreType.Default,@event.ObjectCount));
            EventBus.Subscribe<OnBlastEvent>(@event => EarnPoint(@event.ScoreType,@event.BlastCount));
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnObjectPlacedEvent>(@event => CountPlacements());
            EventBus.Unsubscribe<OnObjectPlacedEvent>(@event => CountMovements());
            EventBus.Unsubscribe<OnObjectPlacedEvent>(@event => EarnPoint(ScoreType.Default,@event.ObjectCount));
            EventBus.Unsubscribe<OnBlastEvent>(@event => EarnPoint(ScoreType.Blast,@event.BlastCount));
        }
    }
}