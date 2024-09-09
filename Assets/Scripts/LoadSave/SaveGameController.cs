using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CORE;
using Unity.Services.CloudSave;
using UnityEngine;

namespace LoadSave
{
    /// <summary>
    /// Acts as a controller to manage save/load interactions.
    /// </summary>
    public class SaveGameController
    {
        private UnityCloudSaveService cloudSaveService;

        public SaveGameController()
        {
            // Initialize the service with the desired repository implementation.
            ISaveRepository repository = new CloudSaveRepository();
            cloudSaveService = new UnityCloudSaveService(repository);
        }

        /// <summary>
        /// Asynchronously saves player data to the cloud.
        /// </summary>
        /// <param name="playerData">The player data to save.</param>
        /// <returns>A task representing the asynchronous save operation.</returns>
        public async Task SaveGameAsync(PlayerData playerData)
        {
            try
            {
                // Construct the key using the username and monster name
                string saveKey = GenerateSaveKey(playerData.Username, playerData.MonsterName);

                // Save player data using the constructed key
                await cloudSaveService.SavePlayerDataAsync(playerData, saveKey);
                
                Debug.Log("Game saved successfully.");
            }
            catch (Exception ex)
            {
                // Log exception details if the save operation fails
                Debug.LogError($"An error occurred while saving the game: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the player's game data when called.
        /// </summary>
        public async void LoadGame(string saveKey, Action<SaveDataDTO> onDataLoaded)
        {
            try
            {
                SaveDataDTO data = await cloudSaveService.LoadPlayerDataAsync(saveKey);  // Load save data from the cloud
                if (data != null)
                {
                    Debug.Log("Game loaded successfully.");
                    onDataLoaded?.Invoke(data);  // Callback to LoadGameController
                }
                else
                {
                    Debug.LogError("Failed to load game data.");
                    onDataLoaded?.Invoke(null);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred while loading the game: {ex.Message}");
                onDataLoaded?.Invoke(null);
            }
        }
        
        public async Task<SaveDataDTO> LoadSaveDataAsync(string saveKey)
        {
            return await cloudSaveService.LoadPlayerDataAsync(saveKey);
        }
        
        /// <summary>
        /// Generates a unique key for saving data using username and monster name.
        /// </summary>
        private string GenerateSaveKey(string username, string monsterName)
        {
            string timestamp = DateTime.Now.ToString("MMddHHmm");  
            return $"{username}_{monsterName}_{timestamp}";
        }

        /// <summary>
        /// Gets all save keys for the current user from Unity Cloud Save.
        /// </summary>
        /// <returns>A list of save keys.</returns>
        public async Task<List<string>> GetAllSaveKeysAsync()
        {
            var keys = await CloudSaveService.Instance.Data.Player.ListAllKeysAsync();
            List<string> relevantKeys = new List<string>();

            foreach (var keyItem in keys)
            {
                //Debug.Log($"Key: {keyItem.Key}"); 

                // Ensure correct string comparison using the Key property
                if (keyItem.Key.StartsWith(GameManager.Instance.CurrentUser))
                {
                    //Debug.Log($"Relevant key found for user {GameManager.Instance.CurrentUser}: {keyItem.Key}");
                    relevantKeys.Add(keyItem.Key);  
                }
            }

            return relevantKeys;
        }
    }
}
