using System.Threading.Tasks;
using CORE;
using UnityEngine;

namespace LoadSave
{
    public class DifficultyLoadSaveController : MonoBehaviour
    {
        private SaveGameController saveGameController;
        private const string DDATAG = "DDA";

        private void Start()
        {
            saveGameController = new SaveGameController();
        }

        public async void SaveGridData()
        {
            // Create a new HouseDataDTO
            DDADTO DDADTO = new DDADTO();

            // Use SaveGameController to save the house data to Unity Cloud Save
            await saveGameController.SaveDataAsync(DDADTO, DDATAG);

            Debug.Log("Dynamic Difficulty Data saved successfully to cloud.");
        }

        public async Task<T> LoadGridData<T>() where T : IDataTransferObject
        {
            string username = GameManager.Instance.PlayerData.Username;
            string monsterName = GameManager.Instance.PlayerData.MonsterName;

            // Generate save key for house data
            string saveKey = saveGameController.GenerateSaveKey(username, monsterName, DDATAG);

            // Use SaveGameController to load the house data from Unity Cloud Save
            T DDADTO = await saveGameController.LoadSaveDataAsync<T>(saveKey);

            if (DDADTO != null)
            {
                return DDADTO;
            }
            else
            {
                Debug.LogError("Failed to load Dynamic Difficulty Data from the cloud.");
                return default;
            }
        }        
    }

    public class DDADTO : IDataTransferObject
    {
    }
}