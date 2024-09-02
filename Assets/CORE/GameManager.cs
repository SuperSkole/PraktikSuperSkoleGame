using LoadSave;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CORE
{
    public class GameManager : MonoBehaviour
    {
        // Player and game Data
        public SaveToJsonManager SaveManager;
        public LoadGameManager LoadManager;

        public PlayerData PlayerData { get; set; }
        public HighScore HighScore;
        public string CurrentUser { get; set; }
        public string CurrentMonsterName { get; set; }
        public string CurrentSaveFileName { get; private set; }
        public string CurrentMonsterColor { get; set; }
        public string CurrentClothMid { get; set; }
        public string CurrentClothTop { get; set; }
        public bool IsNewGame { get; set; }
        
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

        public void ExitGame()
        {
            // Cleanup or save state before exiting
            SaveGame();
            Application.Quit();
        }
        
        public void SaveGame()
        {
            
            // save logic, using savemanager
            SaveManager.SaveGame(CurrentUser, CurrentMonsterName);
        }
        
        private void InitializeGameManager()
        {
            Debug.Log("GameManager.InitializeGameManager()");
            // placeholder in case we need to init GM with default or necessary starting values

            if (instance.GetComponent<PlayerData>() == null)
            {
                PlayerData = instance.gameObject.AddComponent<PlayerData>();
            }
        }
        
        private void InitializeManagers()
        {
            //gameObject.AddComponent<PlayerManager>();
            SaveManager = new SaveToJsonManager();
            LoadManager = new LoadGameManager();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Early out
            if (scene.name.StartsWith("0") ||
                scene.name.Equals("Bootstrapper"))
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