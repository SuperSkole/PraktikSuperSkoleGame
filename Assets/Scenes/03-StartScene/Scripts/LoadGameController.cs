using System.Collections;
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
            // Sanitize the username
            var sanitizedUsername = saveGameController.SanitizeKeyComponent(GameManager.Instance.CurrentUser);
            var monsterName = ExtractMonsterNameFromSaveKey(saveKey);

            bool success = await saveGameController.DeleteAllSavesForMonster(sanitizedUsername, monsterName);

            if (success)
            {
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
            saveGameController.LoadGame<SaveDataDTO>(saveKey, OnDataLoaded);
        }

        /// <summary>
        /// Callback function triggered when saved game data is successfully loaded.
        /// Defines the delegate and subscribes to the sceneLoaded event before loading the Player Scene asynchronously.
        /// Once the Player Scene is loaded, the player is set up, and then the House Scene is loaded.
        /// </summary>
        private void OnDataLoaded(SaveDataDTO dataDTO)
        {
            if (dataDTO != null)
            {
                // // Sanitize loaded data before converting and setting it up
                // string sanitizedUsername = saveGameController.SanitizeLoadedData(dataDTO.Username);
                // string sanitizedMonsterName = saveGameController.SanitizeLoadedData(dataDTO.MonsterName);
                //
                // dataDTO.Username = sanitizedUsername;
                // dataDTO.MonsterName = sanitizedMonsterName;
                
                // Convert SaveDataDTO back into PlayerData using the DataConverter
                PlayerData playerData = GameManager.Instance.Converter.ConvertToPlayerData(dataDTO);

                // Define the delegate and subscribe before loading the scenes
                UnityEngine.Events.UnityAction<Scene, LoadSceneMode> onPlayerSceneLoaded = null;
                onPlayerSceneLoaded = (scene, mode) =>
                {
                    if (scene.name != SceneNames.Player)
                        return;

                    // Set up the player after the player scene is loaded
                    PlayerManager.Instance.SetupPlayerFromSave(playerData);

                    // Unsubscribe from event once the player is set up
                    SceneManager.sceneLoaded -= onPlayerSceneLoaded;

                    // Load the house scene after player setup
                    StartCoroutine(LoadHouseAfterPlayerSetup());
                };

                // Subscribe to the scene loaded event
                SceneManager.sceneLoaded += onPlayerSceneLoaded;

                // Load the player scene asynchronously
                SceneManager.LoadSceneAsync(SceneNames.Player, LoadSceneMode.Additive);
            }
            else
            {
                Debug.LogError("Failed to load game data.");
                isLoading = false; // Reset loading flag if loading failed
            }
        }

        /// <summary>
        /// Loads the house scene after ensuring that the player has been fully set up.
        /// This method is designed to wait until the player setup is complete before proceeding with loading the next scene.
        /// </summary>
        /// <returns>An IEnumerator to be used with a coroutine, yielding control while awaiting player setup.</returns>
        private IEnumerator LoadHouseAfterPlayerSetup()
        {
            // Wait until PlayerManager indicates the player is fully set up
            while (PlayerManager.Instance.SpawnedPlayer == null)
            {
                yield return null;  // Wait one frame and check again
            }

            // Player setup is complete, load the house scene
            SceneLoader.Instance.LoadScene(SceneNames.House);
        }

        /// <summary>
        /// Notifies all save panels that the load operation has completed for the specified save key.
        /// This triggers the OnLoadComplete event on matching save panels.
        /// </summary>
        /// <param name="saveKey">The unique identifier for the save file that has completed loading.</param>
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

        /// <summary>
        /// Extracts the monster's name from the provided save key.
        /// </summary>
        /// <param name="saveKey">The unique identifier containing the monster's name.</param>
        /// <returns>The extracted monster name as a string.</returns>
        private string ExtractMonsterNameFromSaveKey(string saveKey)
        {
            var keyParts = saveKey.Split('_');
            return keyParts.Length > 1 ? keyParts[1] : string.Empty;
        }
    }
}
