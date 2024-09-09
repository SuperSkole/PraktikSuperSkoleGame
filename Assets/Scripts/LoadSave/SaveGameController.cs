using System;
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
        /// Asynchronously saves player data to the cloud.
        /// </summary>
        /// <param name="playerData">The player data to save.</param>
        /// <returns>A task representing the asynchronous save operation.</returns>
        public async Task SaveGameAsync(PlayerData playerData)
        {
            try
            {
                await cloudSaveService.SavePlayerDataAsync(playerData);
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
        public async void LoadGame(string username)
        {
            PlayerData loadedData = await cloudSaveService.LoadPlayerDataAsync(username);
            if (loadedData != null)
            {
                Debug.Log("Game loaded successfully.");
                // Apply loaded data to the game
            }
            else
            {
                Debug.LogError("Failed to load game data.");
            }
        }
    }
}
