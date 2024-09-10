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
        /// Saves the player data by converting it to a DTO and storing it in the cloud using a custom key.
        /// </summary>
        public async Task SavePlayerDataAsync(PlayerData playerData, string saveKey)
        {
            // Convert the PlayerData to a SaveDataDTO
            SaveDataDTO dto = _converter.ConvertToDTO(playerData);

            // Serialize DTO to JSON
            string jsonData = JsonUtility.ToJson(dto, true);

            // Save the data in the cloud with a custom key
            await _saveRepository.SaveAsync(saveKey, jsonData);
        }

        /// <summary>
        /// Loads the player data from the cloud and converts it back to a PlayerData object.
        /// </summary>
        public async Task<SaveDataDTO> LoadPlayerDataAsync(string saveKey)
        {
            string jsonData = await _saveRepository.LoadAsync(saveKey);
            if (string.IsNullOrEmpty(jsonData))
            {
                return null;
            }

            return JsonUtility.FromJson<SaveDataDTO>(jsonData);  // Deserialize and return SaveDataDTO
        }
    }
}