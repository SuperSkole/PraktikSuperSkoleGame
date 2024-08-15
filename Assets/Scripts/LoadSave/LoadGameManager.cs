using System.IO;
using UnityEngine;

namespace LoadSave
{
    public class LoadGameManager
    {
        private string SaveDirectory => Path.Combine(Application.dataPath, "Saves");

        private GameManager gm; // TODO Change to gamemmanager

        public LoadGameManager(GameManager gameManagerManager)
        {
            gm = gameManagerManager;
        }
        
        public bool DoesSaveFileExist(string hashedUsername)
        {
            string filePath = Path.Combine(SaveDirectory, $"{hashedUsername}_save.json");
            return File.Exists(filePath);
        }

        public void LoadGame(string hashedUsername)
        {
            string filePath = Path.Combine(SaveDirectory, $"{hashedUsername}_save.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                SaveDataDTO data = JsonUtility.FromJson<SaveDataDTO>(json);

                // // Use SkinManager to process the loaded skin data
                // skinManager.ProcessLoadedSkins(data, gm.shopSkinManagement);

                // // Restore game state using GameManager
                // gm.SetLoadGameInfo(data);
                
            }
            else
            {
                Debug.LogError($"Save file not found for user: {hashedUsername}");
            }
        }
    }
}
