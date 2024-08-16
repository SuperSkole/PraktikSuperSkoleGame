using UnityEngine;
using System.IO;
using CORE;

namespace LoadSave
{
    public class SaveToJsonManager
    {
        private string SaveDirectory => Path.Combine(Application.dataPath, "Saves");

        private OldGameManager gm; // change to Gamemanager 

        public SaveToJsonManager(OldGameManager oldGameManagerManager)
        {
            gm = oldGameManagerManager;
        }

        public void SaveGame(string hashedUsername)
        {
            SaveDataDTO data = CreateSaveData();
            string filePath = Path.Combine(SaveDirectory, $"{hashedUsername}_save.json");

            string json = JsonUtility.ToJson(data, true); // 'true' for writing with focus on human readabilty
            File.WriteAllText(filePath, json);
        }

        private SaveDataDTO CreateSaveData()
        {
            SaveDataDTO data = new SaveDataDTO
            {
                // TODO: HashedUsername = gm.player.hashedUsername, 
                PlayerName = gm.player.PlayerName,
                MonsterTypeID = gm.player.MonsterTypeID,
                GoldAmount = gm.player.CurrentGoldAmount,
                XPAmount = gm.player.CurrentXPAmount,
                PlayerLevel = gm.player.CurrentLevel,
                SavedPlayerStartPostion = new SavePlayerPosition(gm.player.CurrentPosition),
                // HeadColor = new SerializableColor(gm.player.currentHeadColor),
                // BodyColor = new SerializableColor(gm.player.currentBodyColor),
                // LegColor = new SerializableColor(gm.player.currentLegColor)
            };
            return data;
        }
    }
}