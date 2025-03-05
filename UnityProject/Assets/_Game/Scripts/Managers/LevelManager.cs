using System;
using System.Collections.Generic;
using _Game.DataStructures;
using _Game.GridSystem;
using _Game.Utils;
using UnityEngine;

namespace _Game.Managers
{
    public class LevelManager : MonoBehaviour
    {

        private void Start()
        {
            InitializeLevel();
        }

        private void InitializeLevel()
        {
            EventBus.Fire(new LevelInitializeEvent());
        }

    }
}
