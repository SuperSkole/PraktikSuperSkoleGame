using UnityEngine;
using System.IO;
using CORE;

namespace LoadSave
{
    public class SaveToJsonManager
    {
        private string SaveDirectory => Path.Combine(Application.dataPath, "Saves");

        public void SaveGame(string hashedUsername)
        {
            SaveDataDTO data = CreateSaveData();
            string filePath = Path.Combine(SaveDirectory, $"{hashedUsername}_save.json");

            string json = JsonUtility.ToJson(data, true); // 'true' for writing with focus on human readabilty
            File.WriteAllText(filePath, json);
        }

        private SaveDataDTO CreateSaveData()
        {
            var gm = GameManager.Instance.PlayerData;
            SaveDataDTO data = new SaveDataDTO
            {
                Username = gm.Username, 
                PlayerName = gm.PlayerName,
                MonsterTypeID = gm.MonsterTypeID,
                GoldAmount = gm.CurrentGoldAmount,
                XPAmount = gm.CurrentXPAmount,
                PlayerLevel = gm.CurrentLevel,
                SavedPlayerStartPostion = new SavePlayerPosition(gm.CurrentPosition),
            };
            return data;
        }
    }
}