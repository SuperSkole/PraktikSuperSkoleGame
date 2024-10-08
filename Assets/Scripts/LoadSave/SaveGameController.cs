using CORE;
using Scenes._10_PlayerScene.Scripts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task SaveDataAsync(IDataTransferObject DTO, string dataType)
        {
            var username = PlayerManager.Instance.PlayerData.Username;
            var monsterName = PlayerManager.Instance.PlayerData.MonsterName;

            try
            {
                // Use the username and monsterName to generate the save key
                string saveKey = GenerateSaveKey(username, monsterName, dataType);

                // Save player data, overwriting the existing save if necessary
                await cloudSaveService.SaveAsync(DTO, saveKey);

                Debug.Log("Game saved successfully for " + monsterName);
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred while saving the game: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the game data of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the DTO to load.</typeparam>
        /// <param name="saveKey">The save key associated with the data.</param>
        /// <param name="onDataLoaded">Callback to handle the loaded data.</param>
        public async void LoadGame<T>(string saveKey, Action<T> onDataLoaded) where T : IDataTransferObject
        {
            try
            {
                // Load the data as a generic IDataTransferObject
                T data = await LoadSaveDataAsync<T>(saveKey);
                    
                if (data != null)
                {
                    Debug.Log("Game loaded successfully.");
                    onDataLoaded?.Invoke(data);
                }
                else
                {
                    Debug.LogError("Failed to load game data.");
                    onDataLoaded?.Invoke(default);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred while loading the game: {ex.Message}");
                onDataLoaded?.Invoke(default);
            }
        }

        /// <summary>
        /// Asynchronously loads the save data of the specified type from the cloud.
        /// </summary>
        public async Task<T> LoadSaveDataAsync<T>(string saveKey) where T : IDataTransferObject
        {
            return await cloudSaveService.LoadDataAsync<T>(saveKey);
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
                //Debug.Log($"Save with key '{saveKey}' deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred while deleting the save with key '{saveKey}': {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Generates a unique key for saving data using username, monster name, and dataType.
        /// </summary>
        /// <param name="username">The username related to the save data.</param>
        /// <param name="monsterName">The monster name related to the save data.</param>
        /// <param name="dataType">The type of data being saved.</param>
        /// <returns>A string representing the generated save key.</returns>
        public string GenerateSaveKey(
            string username,
            string monsterName,
            string dataType)
        {
            return $"{username}_{monsterName}_{dataType}";
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
                if (keyItem.Key.StartsWith(GameManager.Instance.CurrentUser) && !keyItem.Key.Contains("_House"))
                {
                        relevantKeys.Add(keyItem.Key);
                }
            }

            return relevantKeys;
        }
    }
}
