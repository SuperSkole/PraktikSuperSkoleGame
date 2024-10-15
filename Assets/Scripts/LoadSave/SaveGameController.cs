using CORE;
using Scenes._10_PlayerScene.Scripts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Asynchronously saves the provided data transfer object (DTO) to the cloud storage.
        /// </summary>
        /// <param name="DTO">The data transfer object to be saved.</param>
        /// <param name="dataType">The type of data being saved, used to generate the save key.</param>
        /// <returns>A task representing the asynchronous save operation.</returns>
        public async Task SaveDataAsync(
            IDataTransferObject DTO,
            string dataType)
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
                // Delete the data with the provided key, stupid unity why do you need deleteoptions
                await CloudSaveService.Instance.Data.Player.DeleteAsync(saveKey, new Unity.Services.CloudSave.Models.Data.Player.DeleteOptions());

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
        /// Deletes all save files associated with a specific user and monster name.
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="monsterName">The monstername</param>
        /// <returns></returns>
        public async Task<bool> DeleteAllSavesForMonster(string username, string monsterName)
        {
            try
            {
                // Step 1: Fetch all save keys associated with the given username and monster name
                List<string> allSaveKeys = await GetAllSaveKeysForMonsterAsync(username, monsterName);
        
                // Step 2: Delete each save key found
                foreach (var saveKey in allSaveKeys)
                {
                    // Delete the data with the provided key, stupid unity why do you need deleteoptions
                    await CloudSaveService.Instance.Data.Player.DeleteAsync(saveKey, new Unity.Services.CloudSave.Models.Data.Player.DeleteOptions());
                    Debug.Log($"Save with key '{saveKey}' deleted successfully.");
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred while deleting saves for '{monsterName}': {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Retrieves all save keys for a specific username and monster name.
        /// </summary>
        private async Task<List<string>> GetAllSaveKeysForMonsterAsync(string username, string monsterName)
        {
            // Get all save keys for the user from the cloud save service
            var allKeys = await CloudSaveService.Instance.Data.Player.ListAllKeysAsync();
    
            List<string> matchingKeys = new List<string>();

            // Search for keys that match the pattern "username_monsterName_*"
            foreach (var keyItem in allKeys)
            {
                if (keyItem.Key.StartsWith($"{username}_{monsterName}_"))
                {
                    matchingKeys.Add(keyItem.Key);
                }
            }

            return matchingKeys;
        }

        /// <summary>
        /// Generates a unique key for saving data using username, monster name, and dataType.
        /// </summary>
        /// <param name="username">The username related to the save data.</param>
        /// <param name="monsterName">The monster name related to the save data.</param>
        /// <param name="dataType">The type of data being saved.</param>
        /// <returns>A string representing the generated save key.</returns>
        public string GenerateSaveKey(string username, string monsterName, string dataType)
        {
            string sanitizedUsername = SanitizeKeyComponent(username);
            string sanitizedMonsterName = SanitizeKeyComponent(monsterName);
            string sanitizedDataType = SanitizeKeyComponent(dataType);
        
            return $"{sanitizedUsername}_{sanitizedMonsterName}_{sanitizedDataType}";
        }

        /// <summary>
        /// Sanitizes the given input string by replacing Danish characters, removing diacritical marks,
        /// converting to ASCII, and eliminating any characters that are not letters, digits,
        /// or allowed special characters.
        /// </summary>
        /// <param name="input">The input string to sanitize.</param>
        /// <returns>The sanitized string suitable for use as a key component.</returns>
        public string SanitizeKeyComponent(string input)
        {
            // Replace Danish characters with acceptable ASCII equivalents
            input = input.Replace("æ", "ae").Replace("Æ", "AE");
            input = input.Replace("ø", "oe").Replace("Ø", "OE");
            input = input.Replace("å", "aa").Replace("Å", "AA");

            // Normalize the string to decompose characters (e.g., accents)
            string normalizedString = input.Normalize(NormalizationForm.FormD);

            // Remove diacritical marks
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string withoutDiacritics = regex.Replace(normalizedString, string.Empty);

            // Remove any remaining non-ASCII characters
            byte[] bytes = Encoding.ASCII.GetBytes(withoutDiacritics);
            string asciiString = Encoding.ASCII.GetString(bytes);

            // Remove any characters that are not letters, digits, or allowed special characters
            asciiString = Regex.Replace(asciiString, @"[^a-zA-Z0-9_\-]", "");

            return asciiString;
        }
        
        /// <summary>
        /// Sanitizes loaded player data.
        /// </summary>
        /// <param name="data">The string data to sanitize.</param>
        /// <returns>Sanitized data as a string.</returns>
        public string SanitizeLoadedData(string data)
        {
            // Reuse the existing sanitize logic or adapt it as needed for loading
            return SanitizeKeyComponent(data);
        }

        /// <summary>
        /// Gets all save keys for the current user from Unity Cloud Save.
        /// </summary>
        /// <returns>A list of save keys.</returns>
        public async Task<List<string>> GetAllSaveKeysFromUserAsync()
        {
            var keys = await CloudSaveService.Instance.Data.Player.ListAllKeysAsync();
            List<string> relevantKeys = new List<string>();
            
            // Sanitize the username to match the format used in the save keys
            string sanitizedUsername = SanitizeKeyComponent(GameManager.Instance.CurrentUser);

            foreach (var keyItem in keys)
            {
                // Ensure correct string comparison using the Key property
                if (keyItem.Key.StartsWith(sanitizedUsername) && !keyItem.Key.Contains("_House"))
                {
                    relevantKeys.Add(keyItem.Key);
                }
            }

            return relevantKeys;
        }

        // public string GenerateSaveKey(
        //     string username,
        //     string monsterName,
        //     string dataType)
        // {
        //     return $"{username}_{monsterName}_{dataType}";
        // }
    }
}
