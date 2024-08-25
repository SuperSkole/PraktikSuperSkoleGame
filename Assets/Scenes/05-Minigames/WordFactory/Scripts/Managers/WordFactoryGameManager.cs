using System;
using System.Collections.Generic;
using Scenes.PlayerScene.Scripts;
using UnityEngine;

namespace Scenes._05_Minigames.WordFactory.Scripts.Managers
{
    public class WordFactoryGameManager : MonoBehaviour
    {
        public event Action<GameObject> OnGearAdded;

        // public so other can access the "game settings"
        public int numberOfGears = 2; // Default to 2 gears
        public int numberOfTeeth = 8; // Default to 8 teeth per gear
        public int difficultyLevel = 1; // Default difficulty level
        
        [SerializeField] private GameObject playerSpawnPoint;
        
        private List<GameObject> gears = new List<GameObject>();

        // Word Factory GameManger singleton
        public static WordFactoryGameManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            PlayerManager.Instance.PositionPlayerAt(playerSpawnPoint);
        }

        public void AddGear(GameObject gear)
        {
            gears.Add(gear);
            OnGearAdded?.Invoke(gear);
            //Debug.Log($"Added gear: {gear.name}");
        }

        public List<GameObject> GetGears()
        {
            return gears;
        }

        public int GetNumberOfGears()
        {
            return numberOfGears;
        }

        public int GetNumberOfTeeth()
        {
            return numberOfTeeth;
        }

        public int GetDifficultyLevel()
        {
            return difficultyLevel;
        }

        public void SetDifficultyLevel(int level)
        {
            difficultyLevel = level;
        }
    }
}