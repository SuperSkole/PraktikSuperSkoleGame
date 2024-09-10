using System.Threading.Tasks;

namespace LoadSave
{
    /// <summary>
    /// Defines a contract for saving and loading data.
    /// This allows for switching between different save systems (e.g., Unity Cloud Save, Backend Server).
    /// </summary>
    public interface ISaveRepository
    {
        /// <summary>
        /// Saves data by key.
        /// </summary>
        /// <param name="key">The key for the data to save.</param>
        /// <param name="jsonData">The JSON string of the data to be saved.</param>
        Task SaveAsync(string key, string jsonData);

        /// <summary>
        /// Loads data by key.
        /// </summary>
        /// <param name="key">The key for the data to load.</param>
        /// <returns>The JSON string of the loaded data.</returns>
        Task<string> LoadAsync(string key);
    }
}