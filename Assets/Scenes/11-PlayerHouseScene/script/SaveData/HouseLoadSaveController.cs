using CORE;
using LoadSave;
using Scenes._11_PlayerHouseScene.script.HouseScripts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.SaveData
{
    public class HouseLoadSaveController : MonoBehaviour
    {
        public PlacementSystem floorData, furnitureData, wallfurnitureData, nonePlaceablesData, placementSystem;
        private SaveGameController saveGameController;


        private const string HOUSETAG = "House";

        private void Start()
        {
            saveGameController = new SaveGameController();
        }

        public async void SaveGridData()
        {
            // Convert the current floor and furniture dictionaries into serializable lists

            SerializableGridData SavedGridData = new SerializableGridData(placementSystem.placedObjectsSaved);

            // Create a new HouseDataDTO
            HouseDataDTO houseDataDTO = new HouseDataDTO(SavedGridData);

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
                Debug.Log("Failed to either load/Find house data from the cloud.");
                return default;
            }
        }
        public bool ReturnIfGenerateSaveNameWorks()
        {
            string username = GameManager.Instance.PlayerData.Username;
            string monsterName = GameManager.Instance.PlayerData.MonsterName;

            // Generate save key for house data
            string saveKey = saveGameController.GenerateSaveKey(username, monsterName, HOUSETAG);

            if (saveKey != string.Empty) { return true; }
            else { return false; }
        }
    }

    [Serializable]
    public class SaveContainer
    {
        public SerializableGridData floorData;
        public SerializableGridData furnitureData;
        public SerializableGridData nonePlaceablesData;

        public SerializableGridData SavedGridData;

    }
}
