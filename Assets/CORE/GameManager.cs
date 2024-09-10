using LoadSave;
using Scenes;
using Scenes._10_PlayerScene.Scripts;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CORE
{
    public class GameManager : MonoBehaviour
    {
        // Player and game Data
        public SaveToJsonManager SaveManager;
        public LoadGameManager LoadManager;
        public SaveGameController SaveGameController; 

        public PlayerData PlayerData { get; set; }
        public HighScore HighScore;
        public string CurrentUser { get; set; }
        public string CurrentMonsterName { get; set; }
        public string CurrentSaveFileName { get; private set; }
        public string CurrentMonsterColor { get; set; }
        public DeviceType UserDevice { get; set; }
        
        public string CurrentClothMid { get; set; }
        public string CurrentClothTop { get; set; }
        public bool IsNewGame { get; set; }
        public bool IsPlayerBootstrapped { get; set; }
        
        // GameManager Singleton
        private static GameManager instance;
        private static readonly object Lock = new object();
        
        /// <summary>
        /// Auto self Creating Lazy Singleton instance
        /// </summary>
        public static GameManager Instance
        {
            get
            {
                lock (Lock)
                {
                    if (instance is null)
                    {
                        instance = FindObjectOfType<GameManager>();
                        if (instance is null)
                        {
                            GameObject singletonObject = new GameObject("GameManager");
                            instance = singletonObject.AddComponent<GameManager>();
                            DontDestroyOnLoad(singletonObject);
                        }
                    }
                    
                    return instance;
                }
            }
        }

        private void Awake()
        {
            // Highlander other GM's There can only be 1
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeManagers();
                SceneManager.sceneLoaded += OnSceneLoaded;
                InitializeGameManager();
            }
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
            LoadManager.LoadGame(CurrentUser);
                
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
        
        public void SaveGame()
        {
            if (SaveGameController != null && PlayerData != null && PlayerManager.Instance != null)
            {
                SaveGameController.SaveGameAsync(PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>());
            }
            else
            {
                Debug.Log("SaveGameController or PlayerData is missing!");
            }
            
            
            // save logic, using savemanager
            //SaveManager.SaveGame(CurrentUser, CurrentMonsterName);
        }
        
        private void InitializeGameManager()
        {
            //Debug.Log("GameManager.InitializeGameManager()");
            // placeholder in case we need to init GM with default or necessary starting values

            if (instance.GetComponent<PlayerData>() == null)
            {
                PlayerData = instance.gameObject.AddComponent<PlayerData>();
            }
        }
        
        private void InitializeManagers()
        {
            //gameObject.AddComponent<PlayerManager>();
            //SaveManager = new SaveToJsonManager();
            LoadManager = new LoadGameManager();
            
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
        }
    }
}