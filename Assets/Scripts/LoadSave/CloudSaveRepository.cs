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
            var client = CloudSaveService.Instance.Data;
            var data = new Dictionary<string, object> { { key, jsonData } };
            await client.ForceSaveAsync(data);
        }

        /// <summary>
        /// Loads data from Unity Cloud Save by key.
        /// </summary>
        /// <param name="key">Key under which data is stored.</param>
        /// <returns>The JSON string of the saved data.</returns>
        public async Task<string> LoadAsync(string key)
        {
            var data = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { key });
            return data.ContainsKey(key) ? data[key] : null;
        }
    }
}