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
            EnsureDirectoryExists();
            string filePath = Path.Combine(SaveDirectory, fileName);
            try
            {
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
            // Early out; no username no save.
            if (string.IsNullOrEmpty(username))
            {
                Debug.Log("Save operation aborted: Username is required.");
                return; 
            }
            
            SaveDataDTO data = CreateSaveData();
            string fileName = GenerateSaveFileName(username, monsterName);
            string filePath = Path.Combine(SaveDirectory, fileName);

            if (!File.Exists(filePath))
            {
                EnsureDirectoryExists();
                string json = JsonUtility.ToJson(data, true); // 'true' for writing with focus on human readabilty //do we want the player to be able to easely read there safe file?
                //Debug.Log($"Serialized JSON: {json}");
                try
                {
                    File.WriteAllText(filePath, json);
                    //Debug.Log($"Successfully saved game to {filePath}");
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

        private SaveDataDTO CreateSaveData()
        {
            var gm = GameManager.Instance.PlayerData;
            SaveDataDTO data = new SaveDataDTO
            {
                Username = gm.Username, 
                MonsterName = gm.MonsterName,
                MonsterTypeID = gm.MonsterTypeID,
                MonsterColor = gm.MonsterColor,
                GoldAmount = gm.CurrentGoldAmount,
                XPAmount = gm.CurrentXPAmount,
                PlayerLevel = gm.CurrentLevel,
                SavedPlayerStartPostion = new SavePlayerPosition(gm.CurrentPosition),
            };
            return data;
        }
        
        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }
    }
}