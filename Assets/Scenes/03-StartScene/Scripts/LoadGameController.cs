using CORE;
using LoadSave;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._03_StartScene.Scripts
{
    /// <summary>
    /// controlling the game loading process.
    /// It does not get involved with how data is loaded
    /// or how player objects are instantiated
    /// </summary>
    public class LoadGameController : MonoBehaviour
    {
        [SerializeField] private LoadGameSetup loadGameSetup;

        public static LoadGameController Instance;
        
        private SaveGameController saveGameController;

        public void RegisterPanel(SavePanel panel) 
        {
            panel.OnLoadRequested += HandleLoadRequest;
        }
        
        private void Awake() 
        {
            Instance = this;
            saveGameController = new SaveGameController();
        }
        
        private void HandleLoadRequest(string saveKey)
        {
            Debug.Log("LoadGameController-HandleLoadRequest: Handling load request for save key: " + saveKey);
            
            // Use SaveGameController to load the game from Unity Cloud Save and pass OnDataLoaded as the callback
            saveGameController.LoadGame(saveKey, OnDataLoaded);
        }

        private void OnDataLoaded(SaveDataDTO data)
        {
            if (data != null)
            {
                // Define the delegate and subscribe before loading the scenes
                UnityEngine.Events.UnityAction<Scene, LoadSceneMode> onSceneLoaded = null;
                onSceneLoaded = (scene, mode) =>
                {
                    if (scene.name == SceneNames.Player)
                    {
                        loadGameSetup.SetupPlayer(data);
                        
                        // Unsubscribe from the event
                        SceneManager.sceneLoaded -= onSceneLoaded;
                    }
                };

                // Subscribe to the scene loaded event
                SceneManager.sceneLoaded += onSceneLoaded;
                
                SceneManager.LoadScene(SceneNames.House);
                SceneManager.LoadSceneAsync(SceneNames.Player, LoadSceneMode.Additive);
                Debug.Log("Data loaded successfully.");
            }
            else
            {
                Debug.LogError("Failed to load game data.");
            }
        }

        private void OnDestroy()
        {
            SavePanel[] panels = FindObjectsOfType<SavePanel>();
            foreach (var panel in panels)
            {
                panel.OnLoadRequested -= HandleLoadRequest;
            }
        }
    }
}