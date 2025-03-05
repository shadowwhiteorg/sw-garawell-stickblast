using System;
using System.Collections.Generic;
using _Game.BlockSystem;
using _Game.DataStructures;
using _Game.Enums;
using _Game.GridSystem;
using _Game.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private int numberOfTouchableObjects;
        [SerializeField] private MoveParent outOfTheSceneTarget;
        private List<SidelineBlock> _sidelineBlocks = new();
        
        public MoveParent OutOfTheSceneTarget => outOfTheSceneTarget;
        private void Start()
        {
            InitializeLevel();
        }

        private void InitializeLevel()
        {
            CreateTouchableBlocks();
            EventBus.Fire(new LevelInitializeEvent());
        }
        private void CreateTouchableBlocks()
        {
            _sidelineBlocks.Clear();

            for (int i = 0; i < numberOfTouchableObjects; i++)
            {
                bool isHorizontal = Random.value > 0.5f;
                var sidelineBlock = CreateTouchableBlock(isHorizontal, i);
                _sidelineBlocks.Add(sidelineBlock);
            }
        }

        private SidelineBlock CreateTouchableBlock(bool isHorizontal, int index)
        {
            var prefab = isHorizontal ? GridManager.Instance.BlockCatalog.horizontalSidelinePrefab : GridManager.Instance.BlockCatalog.verticalSidelinePrefab;
            var sidelineBlock = Instantiate(prefab, outOfTheSceneTarget.transform, true);

            sidelineBlock.LockUnlockMove(false);
            Vector3 mTargetLocalPosition = Vector3.right * ((index - 1) * GridManager.Instance.BlockSize);
            sidelineBlock.transform.localPosition = mTargetLocalPosition;
            sidelineBlock.SetInitialPosition(mTargetLocalPosition);
            sidelineBlock.SetWorldPosition(sidelineBlock.transform.position);

            GridHandler.Instance.RegisterTouchable(sidelineBlock);
            return sidelineBlock;
        }

        public void MoveTouchablesIntoScene()
        {
            MovementHandler<MoveParent>.MoveWithEase(outOfTheSceneTarget,
                new Vector3(0, outOfTheSceneTarget.Position.y, 0), 35, Easing.OutBack);
        }

    }
}
