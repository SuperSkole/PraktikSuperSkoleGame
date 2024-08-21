using System;
using UnityEngine;
using System.IO;
using CORE;

namespace LoadSave
{
    public class SaveToJsonManager
    {
        private string SaveDirectory => Path.Combine(Application.dataPath, "Saves");

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
        
        /// <summary>
        /// /// Method to generate a unique file name for each save.
        /// </summary>
        /// <param name="username">used for save file name</param>
        /// <param name="monsterName">used for save file name, use save if not a save game</param>
        /// <param name="suffix">if not a save game, add a desripting suffix</param>
        /// <returns>The combined filename</returns>
        public string GenerateSaveFileName(string username, string detail, string suffix = null)
        {
            string timestamp = DateTime.Now.ToString("ddMMyyyyHHmm");
            return $"{username}_{detail}_{timestamp}{suffix}.json";
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