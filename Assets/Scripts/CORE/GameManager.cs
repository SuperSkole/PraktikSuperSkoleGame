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

        public WeightManager WeightManager { get; private set; }
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

        public void LoadGame()
        {
            // Logic to load game data
            //LoadManager.LoadGame(CurrentUser);
                
            Debug.Log("Loading game");
        }

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

        
        private void InitializeGameManager()
        {  
            if (!GetComponent<PlayerData>())
            {
                PlayerData = gameObject.AddComponent<PlayerData>();
            }
            
            if (!GetComponent<WeightManager>())
            {
                WeightManager = gameObject.AddComponent<WeightManager>();
            }
        }

        
        private void InitializeManagers()
        {            
            SaveGameController = new SaveGameController();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Early out
            if (scene.name.StartsWith("0") ||
                scene.name.Equals(SceneNames.Player) ||
                scene.name.Equals(SceneNames.Boot))
            {
                return;
            }

            // save player data before entering new scene
            SaveGame();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            instance = null;
        }
    }
}

