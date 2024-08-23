using System;
using UnityEngine;
using System.IO;
using CORE;

namespace LoadSave
{
    public class SaveToJsonManager
    {
        public string SaveDirectory => Path.Combine(Application.dataPath, "Saves");
        
        /// <summary>
        /// /// Method to generate a unique file name for each save.
        /// </summary>
        /// <param name="username">used for save file name</param>
        /// <param name="monsterName">used for save file name</param>
        /// <param name="suffix">if not a save game, add a descriptive suffix</param>
        /// <returns>The combined filename</returns>
        public string GenerateSaveFileName(string username, string monsterName, string suffix = null)
        {
            string timestamp = DateTime.Now.ToString("ddMMHHmm");
            string fileNameSuffix = string.IsNullOrEmpty(suffix) ? "" : $"_{suffix}";
            return $"{username}_{monsterName}_{timestamp}{fileNameSuffix}.json";
        }
        
        public void SaveJson(string json, string fileName)
        {
            string filePath = Path.Combine(SaveDirectory, fileName);
            try
            {
                if (!Directory.Exists(SaveDirectory))
                {
                    Directory.CreateDirectory(SaveDirectory);
                }
                
                File.WriteAllText(filePath, json);
                Debug.Log($"Successfully saved JSON to {filePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error writing to file: {ex.Message}");
            }
        }

        /// <summary>
        /// Public method to initiate saving a game with specific details.
        /// </summary>
        /// <param name="username">used for save file name</param>
        /// <param name="monsterName">used for save file name</param>
        public void SaveGame(string username, string monsterName)
        {
            SaveDataDTO data = CreateSaveData();
            string fileName = GenerateSaveFileName(username, monsterName);
            string filePath = Path.Combine(SaveDirectory, fileName);

            if (!File.Exists(filePath))
            {
                string json = JsonUtility.ToJson(data, true); // 'true' for writing with focus on human readabilty
                Debug.Log($"Serialized JSON: {json}");
                try
                {
                    if (!Directory.Exists(SaveDirectory))
                    {
                        Directory.CreateDirectory(SaveDirectory);
                    }
                    
                    File.WriteAllText(filePath, json);
                    Debug.Log($"Successfully saved game to {filePath}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"SaveGameManager.SaveGame(): Error writing to file: {ex.Message}");
                }
            }
            else
            {
                Debug.Log($"Save file already exists and will not be overwritten: {filePath}");
            }
        }
        
        public string GenerateLoadFileName(string username, string monsterName, string suffix = null)
        {
            string placeholder = "placeholder";
            string fileNameSuffix = string.IsNullOrEmpty(suffix) ? "" : $"_{suffix}";
            return $"{username}_{monsterName}_{placeholder}_{fileNameSuffix}.json";
        }

        private SaveDataDTO CreateSaveData()
        {
            var gm = GameManager.Instance.PlayerData;
            SaveDataDTO data = new SaveDataDTO
            {
                Username = gm.Username, 
                PlayerName = gm.MonsterName,
                MonsterTypeID = gm.MonsterTypeID,
                MonsterColor = gm.MonsterColor,
                GoldAmount = gm.CurrentGoldAmount,
                XPAmount = gm.CurrentXPAmount,
                PlayerLevel = gm.CurrentLevel,
                SavedPlayerStartPostion = new SavePlayerPosition(gm.CurrentPosition),
            };
            return data;
        }
    }
}