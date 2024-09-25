using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CORE;
using Scenes._11_PlayerHouseScene.script.HouseScripts;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.SaveData
{
    public class HouseSaving : MonoBehaviour
    {
        [SerializeField] private PlacementSystem floorData, furnitureData;
        private Dictionary<Vector3Int, PlacementData> floorDictionary, furnitureDictionary;
        public SaveContainer container {  get; private set; }

        private string SaveDirectory => Path.Combine(Application.dataPath, "Saves");
        //string SaveName = "/SaveGameDataFile.json";

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.E))
            //    SaveGridData();
            //if (Input.GetKeyDown(KeyCode.L))
            //    LoadGridData();
        }
    
        public void SaveGridData()
        {
            // Convert the dictionaries to serializable lists
            SerializableGridData floorGridData = new SerializableGridData(floorData.FloorData.placedObjects);
            SerializableGridData furnitureGridData = new SerializableGridData(furnitureData.FurnitureData.placedObjects);

            // Serialize the data to JSON format
            string floorJson = JsonUtility.ToJson(floorGridData, true);
            string furnitureJson = JsonUtility.ToJson(furnitureGridData, true);

            // Create a container for all data
            string combinedJson = "{\"floorData\":" + floorJson + ",\"furnitureData\":" + furnitureJson + "}";

            // Save the JSON to a file
            string filename = GameManager.Instance.SaveManager.GenerateSaveFileName(
                GameManager.Instance.PlayerData.Username, GameManager.Instance.PlayerData.MonsterName, "house");
        
            GameManager.Instance.SaveManager.SaveJson(combinedJson, filename);
        }
    
        public void LoadGridData()
        {
            string mostRecentFile = FindMostRecentSaveFile(GameManager.Instance.PlayerData.Username, GameManager.Instance.PlayerData.MonsterName, "house");

            if (!string.IsNullOrEmpty(mostRecentFile))
            {
                // Using LoadGameManager to read the JSON from the file
                string json = GameManager.Instance.LoadManager.LoadJsonData(mostRecentFile);
                if (!string.IsNullOrEmpty(json))
                {
                    // store json in container
                    container = JsonUtility.FromJson<SaveContainer>(json);
                }
                else
                {
                    Debug.LogError("Failed to load house data.");
                }
            }
            else
            {
                Debug.LogError("No save file found.");
            }
        }
    
        private string FindMostRecentSaveFile(string username, string monsterName, string suffix)
        {
            var directoryInfo = new DirectoryInfo(GameManager.Instance.SaveManager.SaveDirectory);
            var files = directoryInfo
                .GetFiles($"{username}_{monsterName}_*_{suffix}.json")
                .OrderByDescending(f => f.LastWriteTime)
                .FirstOrDefault();

            return files?.FullName;
        }
    
        /// <summary>
        /// here is a list of the string you can feed it
        /// floor,furniture
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Dictionary<Vector3Int, PlacementData> ReturnLoadGridFile(string type)
        {
            switch (type)
            {
                case "floor":
                    return floorDictionary;
                case "furniture":
                    return furnitureDictionary;
                default:
                    return null;
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