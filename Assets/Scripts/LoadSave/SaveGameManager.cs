using UnityEngine;
using System.IO;

namespace LoadSave
{
    public class SaveToJsonManager
    {
        private string SaveDirectory => Path.Combine(Application.dataPath, "Saves");

        private GameManager gm; // change to Gamemanager 

        public SaveToJsonManager(GameManager gameManagerManager)
        {
            gm = gameManagerManager;
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
                PlayerName = gm.player.playerName,
                MonsterName = gm.player.monsterName,
                GoldAmount = gm.player.currentGoldAmount,
                XPAmount = gm.player.currentXPAmount,
                PlayerLevel = gm.player.currentLevel,
                SavedPlayerStartPostion = new SavePlayerPosition(gm.player.currentPosition),
                HeadColor = new SerializableColor(gm.player.currentHeadColor),
                BodyColor = new SerializableColor(gm.player.currentBodyColor),
                LegColor = new SerializableColor(gm.player.currentLegColor)
            };
            return data;
        }
    }
}