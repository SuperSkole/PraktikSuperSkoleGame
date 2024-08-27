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
        public int NumberOfGears = 2; // Default to 2 gears
        public int NumberOfTeeth = 8; // Default to 8 teeth per gear
        public int DifficultyLevel = 1; // Default difficulty level
        
        [SerializeField] private GameObject playerSpawnPoint;
        
        private List<GameObject> gears = new List<GameObject>();
        private IGearStrategy gearStrategy;

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

                IntializeFactoryManager();
            }
        }

        private void IntializeFactoryManager()
        {
            Debug.Log("IntializeFactoryManager");
            
            // Retrieve the number of gears from GameConfig
            NumberOfGears = GameConfig.NumberOfGears;
            
            // Set the gear strategy based on the number of gears
            SetGearStrategy();
        }

        private void Start()
        {
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.PositionPlayerAt(playerSpawnPoint);
            }
            else
            {
                Debug.Log("WordFactory GM.Start(): Player Manager is null");
            }
        }
        
        public IGearStrategy GetGearStrategy() => gearStrategy;

        public void AddGear(GameObject gear)
        {
            gears.Add(gear);
            OnGearAdded?.Invoke(gear);
        }

        public List<GameObject> GetGears() => gears;

        public int GetNumberOfGears() => NumberOfGears;

        public int GetNumberOfTeeth() => NumberOfTeeth;

        public int GetDifficultyLevel() => DifficultyLevel;

        public void SetDifficultyLevel(int level) => DifficultyLevel = level;
        
        private void SetGearStrategy()
        {
            if (NumberOfGears >= 2)
            {
                gearStrategy = new MultiGearStrategy();
            }
            else
            {
                gearStrategy = new SingleGearStrategy();
            }

            if (gearStrategy == null)
            {
                Debug.LogError("Failed to initialize gear strategy.");
            }
        }
    }
}