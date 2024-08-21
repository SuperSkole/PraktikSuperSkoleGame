using System;
using System.Collections.Generic;
using System.IO;
using CORE;
using UnityEngine;

public class HouseSaving : MonoBehaviour
{
    [SerializeField] private PlacementSystem floorData, furnitureData;
    Dictionary<Vector3Int, PlacementData> floorDictionary, furnitureDictionary;
    public SaveContainer container {  get; private set; }

    //string SaveName = "/SaveGameDataFile.json";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SaveGridData();
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGridData();
        }

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
            GameManager.Instance.PlayerData.Username, "save", "house");
        
        GameManager.Instance.SaveManager.SaveJson(combinedJson, filename);
    }
    public void LoadGridData()
    {
        string filename = GameManager.Instance.SaveManager.GenerateLoadFileName(
            GameManager.Instance.PlayerData.Username, "save", "house");
        
        // Read the JSON from the file
        string json = File.ReadAllText(Application.dataPath + "HouseSave.json");
        
        container = JsonUtility.FromJson<SaveContainer>(json);
    }

    public bool IsThereSaveFile()
    {
        bool isThereFile = File.Exists(Application.dataPath + "HouseSave.json") ? true : false;

        return isThereFile;

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


