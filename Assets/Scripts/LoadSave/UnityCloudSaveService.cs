using System.Threading.Tasks;
using UnityEngine;

namespace LoadSave
{
    /// <summary>
    /// Manages the save and load operations by utilizing the CloudSaveRepository and DataConverter.
    /// </summary>
    public class UnityCloudSaveService
    {
        private readonly ISaveRepository _saveRepository;
        private readonly DataConverter _converter = new DataConverter();
        
        /// <summary>
        /// Constructor to inject a specific save repository (Unity Cloud Save, Backend Server, etc.).
        /// </summary>
        /// <param name="saveRepository">The save repository implementation to use.</param>
        public UnityCloudSaveService(ISaveRepository saveRepository)
        {
            _saveRepository = saveRepository;
        }

        /// <summary>
        /// Saves the player data by converting it to a DTO and storing it in the cloud.
        /// </summary>
        /// <param name="playerData">The PlayerData object to be saved.</param>
        public async Task SavePlayerDataAsync(PlayerData playerData)
        {
            // Convert the PlayerData to a SaveDataDTO
            SaveDataDTO dto = _converter.ConvertToDTO(playerData);

            // Serialize DTO to JSON
            string jsonData = JsonUtility.ToJson(dto, prettyPrint: true);

            // Save the data in the cloud
            await _saveRepository.SaveAsync(playerData.Username, jsonData);
        }

        /// <summary>
        /// Loads the player data from the cloud and converts it back to a PlayerData object.
        /// </summary>
        /// <param name="username">Username to load data for.</param>
        /// <returns>The PlayerData object populated with cloud data.</returns>
        public async Task<PlayerData> LoadPlayerDataAsync(string username)
        {
            // Load JSON data from the cloud
            string jsonData = await _saveRepository.LoadAsync(username);

            if (string.IsNullOrEmpty(jsonData)) return null;

            // Deserialize JSON to SaveDataDTO
            SaveDataDTO dto = JsonUtility.FromJson<SaveDataDTO>(jsonData);

            // Convert the DTO back to PlayerData
            return _converter.ConvertToPlayerData(dto);
        }
    }
}