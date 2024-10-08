using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.Services.CloudSave;

namespace LoadSave
{
    /// <summary>
    /// Repository responsible for interacting with Unity Cloud Save.
    /// </summary>
    public class CloudSaveRepository : ISaveRepository
    {
        /// <summary>
        /// Saves data to Unity Cloud Save.
        /// </summary>
        /// <param name="key">Key under which data is stored.</param>
        /// <param name="jsonData">The JSON-serialized string of the data to be saved.</param>
        public async Task SaveAsync(string key, string jsonData)
        {
            var client = CloudSaveService.Instance.Data.Player;
            var data = new Dictionary<string, object> { { key, jsonData } };
            await client.SaveAsync(data);
        }

        /// <summary>
        /// Asynchronously loads data from the cloud using the specified key.
        /// </summary>
        /// <param name="key">The key to load data for.</param>
        /// <returns>The loaded data as a string if the key exists, or null if not found.</returns>
        public async Task<string> LoadAsync(string key)
        {
            var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });
            return data.ContainsKey(key) ? data[key].Value.GetAsString() : null;
        }
    }
}