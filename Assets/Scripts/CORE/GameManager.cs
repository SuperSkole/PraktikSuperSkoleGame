using Analytics;
using CORE.Scripts;
using LoadSave;
using Scenes;
using Scenes._10_PlayerScene.Scripts;
using Scenes._24_HighScoreScene.Scripts;
using TMPro;
using Unity.Services.Authentication;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CORE
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        // Player and game Data
        public SaveGameController SaveGameController; 
        public PlayerManager PlayerManager;

        public PerformanceWeightManager PerformanceWeightManager { get; private set; }
        public SpacedRepetitionManager SpacedRepetitionManager { get; private set; }
        public DynamicDifficultyAdjustmentManager DynamicDifficultyAdjustmentManager { get; private set; }
        
        public DataConverter Converter { get; } = new DataConverter();
        public HighScore HighScore;
        public string CurrentUser { get; set; }
        public string CurrentMonsterName { get; set; }
        public string CurrentMonsterColor { get; set; }
        public DeviceType UserDevice { get; set; }
        
        public string CurrentClothMid { get; set; }
        public string CurrentClothTop { get; set; }
        public bool IsNewGame { get; set; }
        public bool IsPlayerBootstrapped { get; set; }

        private PlayerData playerData;
        
        public PlayerData PlayerData
        {
            get
            {
                if (playerData == null)
                {
                    playerData = GetComponent<PlayerData>();
                    if (playerData == null)
                        playerData = instance.gameObject.AddComponent<PlayerData>();
                }
                
                return playerData;
            }
            set { playerData = value; }
        }
        
        /// <summary>
        /// Initializes the singleton instance and sets up the GameManager.
        /// </summary>
        protected override void Awake()
        {
            // Calling base to ensure the singleton is initialized
            base.Awake(); 

            InitializeManagers();
            SceneManager.sceneLoaded += OnSceneLoaded;
            InitializeGameManager();
        }

        #region Login Region
        
        public void SetUserDuringLogin()
        {
            // Find the username input field in login scene
            GameObject inputFieldObject = GameObject.Find("UsernameInputField");
            TMP_InputField inputField = inputFieldObject.GetComponent<TMP_InputField>(); 

            if (inputField != null)
            {
                CurrentUser = inputField.text;  
                Debug.Log("Username set to: " + CurrentUser);
            }
            else
            {
                Debug.Log("No TMP Input Field found in the scene!");
            }
        }
        
        #endregion Login Region

        /// <summary>
        /// Handles cleanup and game state saving when the application is quitting.
        /// </summary>
        public void OnApplicationQuit()
        {
            // Cleanup or save state before exiting
            Scene currentScene = SceneManager.GetActiveScene();
            
            // Save game only if the current scene is not a bootstrapper or a development scene (scenes starting with '0')
            if (!currentScene.name.StartsWith("0") && !currentScene.name.Equals("Bootstrapper"))
            {
                SaveGame();
            }
            
            AuthenticationService.Instance.SignOut();
            Application.Quit();
        }

        /// <summary>
        /// Saves the current game state by converting PlayerData into a SaveDataDTO and persisting it
        /// using the SaveGameController. If the PlayerManager, SaveGameController, or PlayerData
        /// is not available, the method logs a warning and exits.
        /// </summary>
        public async void SaveGame()
        {
            // Early exit if PlayerManager is missing
            // ReSharper disable once EnforceIfStatementBraces
            if (!PlayerManager.Instance) return;

            // Early exit if SaveGameController or PlayerData is missing
            if (SaveGameController == null)
            {
                Debug.LogWarning("SaveGameController is missing! Cannot save the game.");
                return;
            }

            if (!PlayerData)
            {
                Debug.LogWarning("PlayerData is missing! Cannot save the game.");
                return;
            }

            // Proceed to save if all conditions are met
            if (PlayerManager.Instance.SpawnedPlayer != null)
            {
                Debug.Log("Saving game...");
                //await SaveGameController.SaveGameAsync(PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>());
                
                // Convert the PlayerData to a SaveDataDTO
                SaveDataDTO dto = Converter.ConvertToDTO(PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>());
                
                await SaveGameController.SaveDataAsync(dto, "PlayerData");
            }
            else
            {
                Debug.LogWarning("Cannot save the game.");
            }
        }

        /// <summary>
        /// Initializes various managers required for the GameManager's functionality.
        /// This includes PlayerData, SpacedRepetitionManager, PerformanceWeightManager,
        /// and DynamicDifficultyAdjustmentManager if they are not already present in the GameManager.
        /// </summary>
        private void InitializeGameManager()
        {  
            if (!GetComponent<PlayerData>())
            {
                PlayerData = gameObject.AddComponent<PlayerData>();
            }
            
            if (!GetComponent<SpacedRepetitionManager>())
            {
                SpacedRepetitionManager = gameObject.AddComponent<SpacedRepetitionManager>();
            }
            
            if (!GetComponent<PerformanceWeightManager>())
            {
                PerformanceWeightManager = gameObject.AddComponent<PerformanceWeightManager>();
            }
            
            if (!GetComponent<DynamicDifficultyAdjustmentManager>())
            {
                DynamicDifficultyAdjustmentManager = gameObject.AddComponent<DynamicDifficultyAdjustmentManager>();
            }
        }

        /// <summary>
        /// Instantiates and initializes various game-related managers such as SaveGameController,
        /// </summary>
        private void InitializeManagers()
        {            
            SaveGameController = new SaveGameController();
        }

        /// <summary>
        /// Called when a new scene has been loaded. This method will handle necessary operations
        /// after a scene transition, such as saving player data before entering a new scene.
        /// Will early out if wrong scene.
        /// </summary>
        /// <param name="scene">The scene that was loaded.</param>
        /// <param name="mode">The mode in which the scene was loaded.</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Early out; if the scene is a bootstrapper or playerscene, do not save the game.
            if (scene.name.StartsWith("0") ||
                scene.name.Equals(SceneNames.Player) ||
                scene.name.Equals(SceneNames.Boot))
            {
                return;
            }

            // save player data before entering new scene
            SaveGame();
        }

        /// <summary>
        /// Cleans up resources and detaches event handlers before the GameManager is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            instance = null;
        }
    }
}

