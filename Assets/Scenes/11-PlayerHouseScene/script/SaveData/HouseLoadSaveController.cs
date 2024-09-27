using System;
using System.Threading.Tasks;
using CORE;
using LoadSave;
using Scenes._11_PlayerHouseScene.script.HouseScripts;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.SaveData
{
    public class HouseLoadSaveController : MonoBehaviour
    {
        public PlacementSystem floorData, furnitureData;
        private SaveGameController saveGameController;
        

        private const string HOUSETAG = "House";

        private void Start()
        {
            saveGameController = new SaveGameController();
        }

        public async void SaveGridData()
        {
            // Convert the current floor and furniture dictionaries into serializable lists
            SerializableGridData floorGridData = new SerializableGridData(floorData.FloorData.placedObjects);
            SerializableGridData furnitureGridData = new SerializableGridData(furnitureData.FurnitureData.placedObjects);

            // Create a new HouseDataDTO
            HouseDataDTO houseDataDTO = new HouseDataDTO(floorGridData, furnitureGridData);

            // Use SaveGameController to save the house data to Unity Cloud Save
            await saveGameController.SaveDataAsync(houseDataDTO, HOUSETAG);

            Debug.Log("House data saved successfully to cloud.");
        }

        public async Task<T> LoadGridData<T>() where T : IDataTransferObject
        {
            string username = GameManager.Instance.PlayerData.Username;
            string monsterName = GameManager.Instance.PlayerData.MonsterName;

            // Generate save key for house data
            string saveKey = saveGameController.GenerateSaveKey(username, monsterName, HOUSETAG);

            // Use SaveGameController to load the house data from Unity Cloud Save
            T houseDataDTO = await saveGameController.LoadSaveDataAsync<T>(saveKey);

            if (houseDataDTO != null)
            {
                 return houseDataDTO;
            }
            else
            {
                Debug.LogError("Failed to load house data from the cloud.");
                return default;
            }
        }        
    }

    [Serializable]
    public class SaveContainer
    {
        public SerializableGridData floorData;
        public SerializableGridData furnitureData;
    }
}
