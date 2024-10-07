using System;
using System.Collections.Generic;
using CORE;
using Scenes._10_PlayerScene.Scripts;
using Scenes._50_Minigames._56_WordFactory.Scripts.GameModeStrategy;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.Managers
{
    /// <summary>
    /// Manages the Word Factory game, handling gear addition, player positioning, and scene transitions.
    /// </summary>
    public class WordFactoryGameManager : PersistentSingleton<WordFactoryGameManager>
    {
        public event Action<GameObject> OnGearAdded;

        // public so other can access the "game settings"
        public int NumberOfGears = 2; // Default to 2 gears
        public int NumberOfTeeth = 8; // Default to 8 teeth per gear
        public int DifficultyLevel = 1; // Default difficulty level
        public GameObject CoinPrefab;
        
        public GameObject PlayerSpawnPoint;
        [SerializeField] private GameObject dropOffPoint;
        
        private GameObject wordBlockPrefabForSingleGearMode;
        private List<GameObject> gears = new List<GameObject>();
        private IGearStrategy gearStrategy;

        // Word Factory GameManger singleton
        /// <summary>
        /// Singleton instance of the WordFactoryGameManager.
        /// </summary>
        // public static WordFactoryGameManager Instance { get; private set; }
        //
        // private void Awake()
        // {
        //     if (Instance != null && Instance != this)
        //     {
        //         Destroy(gameObject);
        //     }
        //     else
        //     {
        //         Instance = this;
        //         //DontDestroyOnLoad(gameObject);
        //         SceneManager.sceneUnloaded += OnSceneUnloaded;
        //         IntializeFactoryManager();
        //     }
        // }
        protected override void Awake()
        {
            base.Awake();
            
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            IntializeFactoryManager();
        }

        /// <summary>
        /// Initializes the factory manager by setting up the gear strategy.
        /// </summary>
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
                PlayerManager.Instance.PositionPlayerAt(PlayerSpawnPoint);
                PlayerManager.Instance.SpawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
                PlayerManager.Instance.SpawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;
                
                PlayerManager.Instance.SpawnedPlayer.AddComponent<AutoMovePlayerInFactory>();
                PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayerInFactory>().DropOffPoint = dropOffPoint;
                PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayerInFactory>().PlayerSpawnPoint = PlayerSpawnPoint;
                PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerAnimatior>().SetCharacterState("Idle");
                PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerFloating>().enabled = false;
            }
            else
            {
                Debug.Log("WordFactory GM.Start(): Player Manager is null");
            }
        }
        
        /// <summary>
        /// Retrieves the current gear strategy.
        /// </summary>
        public IGearStrategy GetGearStrategy() => gearStrategy;

        /// <summary>
        /// Adds a gear to the game and invokes the OnGearAdded event.
        /// </summary>
        public void AddGear(GameObject gear)
        {
            gears.Add(gear);
            OnGearAdded?.Invoke(gear);
        }

        public List<GameObject> GetGears() => gears;
        public GameObject GetWordBlock() => wordBlockPrefabForSingleGearMode;
        public GameObject SetWordBlock(GameObject prefab) => wordBlockPrefabForSingleGearMode = prefab;

        public int GetNumberOfGears() => NumberOfGears;

        public int GetNumberOfTeeth() => NumberOfTeeth;

        public int GetDifficultyLevel() => DifficultyLevel;

        public void SetDifficultyLevel(int level) => DifficultyLevel = level;
        
        /// <summary>
        /// Sets the gear strategy based on the number of gears.
        /// </summary>
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
        
        /// <summary>
        /// If Next scene is main re-enable player movement and destroy factory singleton managers.
        /// </summary>
        private void OnSceneUnloaded(Scene scene)
        {
            if (scene.name == SceneNames.Factory)
            {
                // remove automove
                PlayerManager.Instance.SpawnedPlayer
                    .GetComponent<PlayerFloating>()
                    .enabled = true;
                //StopCoroutine(PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayerInFactory>().MoveToPositionCoroutine(null));
                Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayerInFactory>());
        
                // Clean up the game manager and sound manager when transitioning to the main scene
                Destroy(Instance);
                instance = null; 
                Destroy(WordFactorySoundManager.Instance);
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}