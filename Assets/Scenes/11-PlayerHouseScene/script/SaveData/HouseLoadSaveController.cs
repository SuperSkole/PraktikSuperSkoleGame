using System;
using CORE;
using LoadSave;
using Scenes._11_PlayerHouseScene.script.HouseScripts;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.SaveData
{
    public class HouseLoadSaveController : MonoBehaviour
    {
        [SerializeField] private PlacementSystem floorData, furnitureData;
        private SaveGameController saveGameController;

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
            await saveGameController.SaveDataAsync(houseDataDTO, "house");

            Debug.Log("House data saved successfully to cloud.");
        }

        public async void LoadGridData()
        {
            string username = GameManager.Instance.PlayerData.Username;
            string monsterName = GameManager.Instance.PlayerData.MonsterName;

            // Generate save key for house data
            string saveKey = saveGameController.GenerateSaveKey(username, monsterName, "house");

            // Use SaveGameController to load the house data from Unity Cloud Save
            HouseDataDTO houseDataDTO = await saveGameController.LoadSaveDataAsync<HouseDataDTO>(saveKey);

            if (houseDataDTO != null)
            {
                ApplyLoadedData(houseDataDTO);
            }
            else
            {
                Debug.LogError("Failed to load house data from the cloud.");
            }
        }

        private void ApplyLoadedData(HouseDataDTO houseDataDTO)
        {
            // Apply the loaded grid data to the house systems
            // floorData.FloorData.placedObjects = houseDataDTO.FloorData.ConvertToDictionary();
            // furnitureData.FurnitureData.placedObjects = houseDataDTO.FurnitureData.ConvertToDictionary();

            Debug.Log("House data applied successfully.");
        }
    }

    [Serializable]
    public class SaveContainer
    {
        public SerializableGridData floorData;
        public SerializableGridData furnitureData;
    }
}
