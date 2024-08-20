using LoadSave;
using Scenes.StartScene.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CORE
{
    public class GameManager : MonoBehaviour
    {
        // Player and game Data
        public PlayerData PlayerData { get; set; }

        
        public string CurrentUsername { get; private set; }
        public string CurrentPlayerName { get; private set; }
        public string CurrentSaveFileName { get; private set; }

        private PlayerManager playerManager;
        private SaveToJsonManager saveManager;
        private LoadGameManager loadGameManager;
        private static GameManager _instance;
        
        /// <summary>
        /// Auto self Creating Lazy Singleton instance
        /// </summary>
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Find existing GameManager instance in the scene or create new one if none exists
                    _instance = FindObjectOfType<GameManager>();
                    if (_instance == null)
                    {
                        GameObject gameManager = new GameObject("GameManager");
                        _instance = gameManager.AddComponent<GameManager>();
                    }
                }
                
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                // Make GM persistent when changing scnes
                DontDestroyOnLoad(gameObject); 
                
                InitializeManagers();
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else if (_instance != this)
            {
                // Highlander other GM's There can only be 1
                Destroy(gameObject); 
            }
            
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
                CurrentUsername = inputField.text;  
                Debug.Log("Username set to: " + CurrentUsername);
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
            loadGameManager.LoadGame(CurrentUsername);
                
            Debug.Log("Loading game");
        }

        public void SaveGame()
        {
            // save logic, using savemanager
            Debug.Log("Game Saved!");
            saveManager.SaveGame(PlayerData.Username);
        }

        public void ExitGame()
        {
            // Cleanup or save state before exiting
            SaveGame();
            Application.Quit();
        }
        
        private void InitializeGameManager()
        {
            Debug.Log("GameManager.InitializeGameManager()");
            // placeholder in case we need to init GM with default or necessary starting values
        }
        
        private void InitializeManagers()
        {
            gameObject.AddComponent<PlayerManager>();
            saveManager = new SaveToJsonManager();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!scene.name.StartsWith("00") && !scene.name.StartsWith("01"))
            {
                // save player data before entering new scene
                SaveGame();
                //LoadGame();
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}