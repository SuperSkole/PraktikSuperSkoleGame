using System;
using System.Collections.Generic;
using System.IO;
using LoadSave;
using UnityEngine;

namespace Scenes.StartScene.Scripts
{
    public class LoadGameManager : MonoBehaviour
    {
        //private GameManager gm; // TODO Change to gamemmanager

        public string SaveDirectory = Path.Combine(Application.dataPath, "Saves");

        void Awake()
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }

        public List<string> GetAllSaveFiles()
        {
            List<string> saves = new List<string>();
            string[] files = Directory.GetFiles(SaveDirectory, "*.json");
            foreach (var file in files)
            {
                saves.Add(Path.GetFileNameWithoutExtension(file));
            }
            return saves;
        }

        public SaveDataDTO LoadGameDataSync(string fileName)
        {
            string filePath = Path.Combine(SaveDirectory, fileName + ".json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<SaveDataDTO>(json);
            }
            return null;
        }
        
        public void LoadGameDataAsync(string fileName, Action<SaveDataDTO> callback)
        {
            string filePath = Path.Combine(SaveDirectory, fileName + ".json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                SaveDataDTO data = JsonUtility.FromJson<SaveDataDTO>(json);
                callback?.Invoke(data);
            }
            else
            {
                Debug.LogError("File not found: " + filePath);
                callback?.Invoke(null);
            }
        }
        
        public bool DoesSaveFileExist(string hashedUsername)
        {
            string filePath = Path.Combine(SaveDirectory, $"{hashedUsername}_save.json");
            return File.Exists(filePath);
        }
        
        public string GetSaveFilePath(string hashedUsername)
        {
            return Path.Combine(SaveDirectory, $"{hashedUsername}_save.json");
        }
        
        public void LoadGame(string hashedUsername)
        {
            string filePath = Path.Combine(SaveDirectory, $"{hashedUsername}_save.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                SaveDataDTO data = JsonUtility.FromJson<LoadSave.SaveDataDTO>(json);
            }
            else
            {
                Debug.LogError($"Save file not found for user: {hashedUsername}");
            }
        }
    }
}
