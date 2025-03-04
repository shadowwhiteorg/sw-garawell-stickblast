using System;
using System.Collections.Generic;
using _Game.Enums;
using _Game.GridSystem;
using _Game.DataStructures;
using _Game.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Game.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private float initialTouchableXOffset;
        [SerializeField] private int numberOfTouchableObjects;
        [SerializeField] private MoveParent outOfTheSceneTarget;

        private GameObject _touchableParent;
        private SidelineBlock _sidelineBlockToCreate;
        private List<SidelineBlock> _sidelineBlocks;

        private void Start()
        {
            InitializeLevel();
            EventBus.Fire(new LevelStartEvent());
        }

        private void InitializeLevel()
        {
            EventBus.Fire(new LevelInitializeEvent());
        }

        private void CreateTouchableBlocks()
        {
            _sidelineBlocks = new List<SidelineBlock>();
            for (int i = 0; i < numberOfTouchableObjects; i++)
            {
                bool mIsHorizontal = Random.value > 0.5f;
                _sidelineBlockToCreate = GridHandler.Instance.CreateSidelineBlock(mIsHorizontal);
                _sidelineBlockToCreate.LockUnlockMove(false);
                _sidelineBlockToCreate.transform.SetParent(outOfTheSceneTarget.transform);
                _sidelineBlockToCreate.transform.localPosition = Vector3.zero + (i-1)*GridHandler.Instance.CellSize*Vector3.right;
                _sidelineBlockToCreate.SetWorldPosition(_sidelineBlockToCreate.transform.position);
                _sidelineBlocks.Add(_sidelineBlockToCreate);

                GridHandler.Instance.AddToInteractableTouchables(_sidelineBlockToCreate);
            }
        }

        public void MoveTouchablesIntoScene()
        {
            MovementHandler<MoveParent>.MoveWithEase(outOfTheSceneTarget,
                new Vector3(0, outOfTheSceneTarget.Position.y,0), 35, Easing.OutBack);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<LevelStartEvent>(e => CreateTouchableBlocks());
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<LevelStartEvent>(e => CreateTouchableBlocks());
        }
    }
}