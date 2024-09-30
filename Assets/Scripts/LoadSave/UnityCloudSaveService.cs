using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace LoadSave
{
    /// <summary>
    /// Manages the save and load operations by utilizing the CloudSaveRepository and DataConverter.
    /// </summary>
    public class UnityCloudSaveService
    {
        private readonly ISaveRepository saveRepository;
        private readonly DataConverter converter = new DataConverter();
        
        /// <summary>
        /// Constructor to inject a specific save repository (Unity Cloud Save, Backend Server, etc.).
        /// </summary>
        /// <param name="saveRepository">The save repository implementation to use.</param>
        public UnityCloudSaveService(ISaveRepository saveRepository)
        {
            this.saveRepository = saveRepository;
        }

        /// <summary>
        /// Saves a data transfer object (DTO) by converting it to JSON and storing it in the cloud using a specified key.
        /// </summary>
        /// <param name="dto">The data transfer object to be saved.</param>
        /// <param name="saveKey">The key to associate with the saved data.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public async Task SaveAsync(IDataTransferObject dto, string saveKey)
        {
            // Serialize DTO to JSON
            string jsonData = JsonConvert.SerializeObject(dto, Formatting.Indented);
            
            // Save the data in the cloud with a custom key
            await saveRepository.SaveAsync(saveKey, jsonData);
        }
        
        /// <summary>
        /// Loads the data from the cloud and returns the specific type of IDataTransferObject.
        /// </summary>
        /// <param name="saveKey">The key of the save to load.</param>
        /// <typeparam name="T">The expected type of the DTO.</typeparam>
        /// <returns>The loaded data as an IDataTransferObject.</returns>
        public async Task<T> LoadDataAsync<T>(string saveKey) where T : IDataTransferObject
        {
            string jsonData = await saveRepository.LoadAsync(saveKey);
            if (string.IsNullOrEmpty(jsonData))
            {
                return default;
            }

            // Deserialize the data into the specified DTO type
            T dto = JsonConvert.DeserializeObject<T>(jsonData);

            return dto;
        }
    }
}