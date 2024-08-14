using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LoadSave
{
    public class LoadGameManager : MonoBehaviour
    {
        public string SaveDirectory => Path.Combine(Application.dataPath, "Saves");

        //private GameManager gm; // TODO Change to gamemmanager

        void Start()
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }
        
        public bool DoesSaveFileExist(string hashedUsername)
        {
            string filePath = Path.Combine(SaveDirectory, $"{hashedUsername}_save.json");
            return File.Exists(filePath);
        }
        
        public List<string> GetAllSaveFiles()
        {
            List<string> saves = new List<string>();
            string[] files = Directory.GetFiles(SaveDirectory, "*.json");
            foreach (string file in files)
            {
                saves.Add(Path.GetFileNameWithoutExtension(file));
            }
            return saves;
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
        
        public string GetSaveFilePath(string hashedUsername)
        {
            return Path.Combine(SaveDirectory, $"{hashedUsername}_save.json");
        }
    }
}
