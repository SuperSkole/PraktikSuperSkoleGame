using LoadSave;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CORE
{
    public class GameManager : MonoBehaviour
    {
        // Player and game Data
        public SaveToJsonManager SaveManager;
        public LoadGameManager LoadManager;
       // public PlayerManager PlayerManager;

        public PlayerData PlayerData { get; set; }
        public HighScore HighScore;
        public string CurrentUser { get; private set; }
        public string CurrentMonsterName { get; set; }
        public string CurrentSaveFileName { get; private set; }
        public string CurrentMonsterColor { get; set; }



        
        private static GameManager _instance;
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
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<GameManager>();
                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject("GameManager");
                            _instance = singletonObject.AddComponent<GameManager>();
                            DontDestroyOnLoad(singletonObject);
                        }
                    }
                    
                    return _instance;
                }
            }
        }

        private void Awake()
        {
            // Highlander other GM's There can only be 1
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }else
            {
                _instance = this;
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

        public void SaveGame()
        {
            // save logic, using savemanager
            Debug.Log("Game Saved!");
            SaveManager.SaveGame(PlayerData.Username, PlayerData.MonsterName);
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

            if (_instance.GetComponent<PlayerData>() == null)
            {
                PlayerData = _instance.gameObject.AddComponent<PlayerData>();
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
            if (scene.name.StartsWith("00") ||
                scene.name.StartsWith("01") ||
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