using System.Threading.Tasks;
using CORE;
using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

namespace Scenes._03_StartScene.Scripts
{
    /// <summary>
    /// Controls the game loading process, managing the communication with save data components.
    /// It does not get involved with how data is loaded or how player objects are instantiated.
    /// </summary>
    public class LoadGameController : MonoBehaviour
    {
        /// <summary>
        /// The setup script that manages loading the player and environment after game data is loaded.
        /// </summary>
        [SerializeField] private LoadGameSetup loadGameSetup;
        
        private SaveGameController saveGameController;
        private bool isLoading = false;
        
        /// <summary>
        /// Initializes the singleton instance and the SaveGameController.
        /// </summary>
        public static LoadGameController Instance;
        
        private void Awake() 
        {
            if (Instance == null)
            {
                Instance = this;
                saveGameController = new SaveGameController();
            }
            else
            {
                Destroy(gameObject); // Ensure only one instance exists
            }
        }

        /// <summary>
        /// Deletes the game save with the given save key.
        /// Updates any associated UI components after successful deletion.
        /// </summary>
        /// <param name="saveKey">The unique identifier for the save file to be deleted.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating success.</returns>
        public async Task<bool> DeleteSave(string saveKey)
        {
            Debug.Log("LoadGameController-DeleteSave: Attempting to delete save with key: " + saveKey);
            
            bool success = await saveGameController.DeleteSave(saveKey);
            if (success)
            {
                Debug.Log("Save deleted successfully: " + saveKey);
                // Optionally: Notify UI components or refresh panels after deletion
                SavePanel[] panels = FindObjectsOfType<SavePanel>();
                foreach (var panel in panels)
                {
                    if (panel.SaveKey == saveKey)
                    {
                        panel.ClearPanel();
                    }
                }

                return true;
            }
            else
            {
                Debug.LogError("Failed to delete save: " + saveKey);
            }

            return false;
        }
        
        /// <summary>
        /// Registers a SavePanel to listen for load requests from the UI.
        /// </summary>
        /// <param name="panel">The SavePanel that will trigger load requests.</param>
        public void RegisterPanel(SavePanel panel) 
        {
            panel.OnLoadRequested += HandleLoadRequest;
        }

        /// <summary>
        /// Handles a load request by initiating the loading of saved game data.
        /// </summary>
        private void HandleLoadRequest(string saveKey)
        {
            if (isLoading)
            {
                Debug.LogWarning("Load operation is already in progress, ignoring this request.");
                return;
            }

            isLoading = true;
            //Debug.Log($"LoadGameController-HandleLoadRequest: Handling load request for save key: {saveKey}");
    
            // Load the saved game data using the generic LoadGame<T> method
            saveGameController.LoadGame<SaveDataDTO>(saveKey, data => OnDataLoaded(data, saveKey));
        }

        /// <summary>
        /// Callback function that is triggered when saved game data is successfully loaded.
        /// Sets up the game state with the loaded data and transitions to the appropriate scenes.
        /// </summary>
        private void OnDataLoaded(SaveDataDTO dataDTO, string saveKey)
        {
            if (dataDTO != null)
            {
                // Convert SaveDataDTO back into PlayerData using the DataConverter
                PlayerData playerData = GameManager.Instance.Converter.ConvertToPlayerData(dataDTO);

                // Define the delegate and subscribe before loading the scenes
                UnityEngine.Events.UnityAction<Scene, LoadSceneMode> onSceneLoaded = null;
                onSceneLoaded = (scene, mode) =>
                {
                    // Early out if not player scene
                    if (scene.name != SceneNames.Player)
                    {
                        return;
                    }
                    
                    loadGameSetup.SetupPlayer(playerData);
                        
                    // Unsubscribe from the event
                    SceneManager.sceneLoaded -= onSceneLoaded;
                    
                    // Reset loading flag
                    isLoading = false;
                    
                    // Notify SavePanel that loading is complete
                    NotifyLoadComplete(saveKey);
                };

                // Subscribe to the scene loaded event
                SceneManager.sceneLoaded += onSceneLoaded;

                // Load the House and Player scenes, using additive mode for Player
                SceneManager.LoadSceneAsync(SceneNames.Player, LoadSceneMode.Additive);
                SceneLoader.Instance.LoadScene(SceneNames.House);

                Debug.Log("Data loaded successfully.");
            }
            else
            {
                Debug.LogError("Failed to load game data.");
                isLoading = false; // Reset loading flag if loading failed
            }
        }
        
        private void NotifyLoadComplete(string saveKey)
        {
            SavePanel[] panels = FindObjectsOfType<SavePanel>();
            foreach (var panel in panels)
            {
                if (panel.SaveKey == saveKey)
                {
                    panel.OnLoadComplete();
                }
            }
        }

        /// <summary>
        /// Unregisters all SavePanel listeners to prevent memory leaks when the controller is destroyed.
        /// </summary>
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
