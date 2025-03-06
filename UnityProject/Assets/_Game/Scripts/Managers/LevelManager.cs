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
    [SerializeField] private BlockCatalog blockCatalog; // Assign BlockCatalog in the Inspector
    private List<SidelineBlock> _sidelineBlocks = new();

    public MoveParent OutOfTheSceneTarget => outOfTheSceneTarget;

    private void Start()
    {
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        // CreateTouchableBlocks();
        EventBus.Fire(new LevelInitializeEvent());
    }

    // public void CreateTouchableBlocks()
    // {
    //     _sidelineBlocks.Clear();
    //
    //     for (int i = 0; i < numberOfTouchableObjects; i++)
    //     {
    //         // Randomly select a Shape
    //         Shape Shape = blockCatalog.shapes[Random.Range(0, blockCatalog.shapes.Count)];
    //         var sidelineBlock = CreateTouchableBlock(Shape, i);
    //         _sidelineBlocks.Add(sidelineBlock);
    //     }
    // }

    // private SidelineBlock CreateTouchableBlock(Shape Shape, int index)
    // {
    //     // Use the first line's prefab for the main block
    //     var prefab = Shape.lines[0].isHorizontal ? 
    //         GridManager.Instance.BlockCatalog.horizontalSidelinePrefab : 
    //         GridManager.Instance.BlockCatalog.verticalSidelinePrefab;
    //
    //     var sidelineBlock = Instantiate(prefab, outOfTheSceneTarget.transform, true);
    //     sidelineBlock.Shape = Shape; // Assign the Shape to the block
    //
    //     sidelineBlock.LockUnlockMove(false);
    //     Vector3 mTargetLocalPosition = Vector3.right * ((index - 1) * GridManager.Instance.BlockSize);
    //     sidelineBlock.transform.localPosition = mTargetLocalPosition;
    //     sidelineBlock.SetInitialPosition(mTargetLocalPosition);
    //     sidelineBlock.SetWorldPosition(sidelineBlock.transform.position);
    //
    //     GridHandler.Instance.RegisterTouchable(sidelineBlock);
    //     return sidelineBlock;
    // }
    //
    // public void MoveTouchablesIntoScene()
    // {
    //     MovementHandler<MoveParent>.MoveWithEase(outOfTheSceneTarget,
    //         new Vector3(0, outOfTheSceneTarget.Position.y, 0), 35, Easing.OutBack);
    // }
    }
}
