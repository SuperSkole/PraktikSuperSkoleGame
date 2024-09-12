using System.Threading.Tasks;
using CORE;
using LoadSave;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        public static LoadGameController Instance;
        
        private SaveGameController saveGameController;

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
        /// Initializes the singleton instance and the SaveGameController.
        /// </summary>
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
        /// Handles a load request by initiating the loading of saved game data.
        /// </summary>
        /// <param name="saveKey">The key of the save file to load.</param>
        private void HandleLoadRequest(string saveKey)
        {
            Debug.Log("LoadGameController-HandleLoadRequest: Handling load request for save key: " + saveKey);
            
            // Use SaveGameController to load the game from Unity Cloud Save and pass OnDataLoaded as the callback
            saveGameController.LoadGame(saveKey, OnDataLoaded);
        }

        /// <summary>
        /// Callback function that is triggered when saved game data is successfully loaded.
        /// Sets up the game state with the loaded data and transitions to the appropriate scenes.
        /// </summary>
        /// <param name="data">The data transfer object containing the loaded game data.</param>
        private void OnDataLoaded(SaveDataDTO data)
        {
            if (data != null)
            {
                // Define the delegate and subscribe before loading the scenes
                UnityEngine.Events.UnityAction<Scene, LoadSceneMode> onSceneLoaded = null;
                onSceneLoaded = (scene, mode) =>
                {
                    // Early out; if not player scene
                    if (scene.name != SceneNames.Player)
                    {
                        return;
                    }

                    loadGameSetup.SetupPlayer(data);
                        
                    // Unsubscribe from the event
                    SceneManager.sceneLoaded -= onSceneLoaded;
                };

                // Subscribe to the scene loaded event
                SceneManager.sceneLoaded += onSceneLoaded;
                
                // Load the House and Player scenes, using additive mode for Player
                SceneManager.LoadScene(SceneNames.House);
                SceneManager.LoadSceneAsync(SceneNames.Player, LoadSceneMode.Additive);
                Debug.Log("Data loaded successfully.");
            }
            else
            {
                Debug.LogError("Failed to load game data.");
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
