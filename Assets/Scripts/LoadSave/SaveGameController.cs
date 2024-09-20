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
                // Use the username and monsterName to generate the save key
                string saveKey = GenerateSaveKey(playerData.Username, playerData.MonsterName);

                // Save player data, overwriting the existing save if necessary
                await cloudSaveService.SavePlayerDataAsync(playerData, saveKey);

                Debug.Log("Game saved successfully for " + playerData.MonsterName);
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred while saving the game: {ex.Message}");
            }
        }

        /// <summary>
        /// Asynchronously loads the player's game data from the cloud.
        /// </summary>
        /// <param name="saveKey">The key of the save to load.</param>
        /// <param name="onDataLoaded">Callback to pass the loaded data.</param>
        public async void LoadGame(string saveKey, Action<PlayerData> onDataLoaded)
        {
            try
            {
                // Load save data from the cloud
                PlayerData data = await cloudSaveService.LoadPlayerDataAsync(saveKey);  
                
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

        /// <summary>
        /// Deletes a save file from Unity Cloud Save based on the provided saveKey.
        /// </summary>
        /// <param name="saveKey">The key of the save to delete.</param>
        /// <returns>True if the delete operation was successful, false otherwise.</returns>
        public async Task<bool> DeleteSave(string saveKey)
        {
            try
            {
                // Delete the data with the provided key
                await CloudSaveService.Instance.Data.ForceDeleteAsync(saveKey);
                Debug.Log($"Save with key '{saveKey}' deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred while deleting the save with key '{saveKey}': {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Loads the save data asynchronously using the save key.
        /// </summary>
        public async Task<PlayerData> LoadSaveDataAsync(string saveKey)
        {
            return await cloudSaveService.LoadPlayerDataAsync(saveKey);
        }

        /// <summary>
        /// Generates a unique key for saving data using username and monster name.
        /// </summary>
        private string GenerateSaveKey(string username, string monsterName)
        {
            return $"{username}_{monsterName}";
        }

        /// <summary>
        /// Gets all save keys for the current user from Unity Cloud Save.
        /// </summary>
        /// <returns>A list of save keys.</returns>
        public async Task<List<string>> GetAllSaveKeysFromUserAsync()
        {
            var keys = await CloudSaveService.Instance.Data.Player.ListAllKeysAsync();
            List<string> relevantKeys = new List<string>();

            foreach (var keyItem in keys)
            {
                // Ensure correct string comparison using the Key property
                if (keyItem.Key.StartsWith(GameManager.Instance.CurrentUser))
                {
                    relevantKeys.Add(keyItem.Key);  
                }
            }

            return relevantKeys;
        }
    }
}
