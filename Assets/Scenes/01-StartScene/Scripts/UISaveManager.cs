using System;
using System.Collections.Generic;
using System.IO;
using LoadSave;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.StartScene.Scripts
{
    public class UISaveManager : MonoBehaviour
    {
        [SerializeField] private LoadGameManager loadGameManager;
        [SerializeField] private List<SavePanel> savePanels;
        
        private string saveFileNameOne;
        private string saveFileNameTwo;
        private string saveFileNameThree;
        
        

        private void Start()
        {
            if (loadGameManager == null)
            {
                Debug.Log("LoadGameManager is not assigned.");
                return;
            }
        }
        
        public void CheckForSavesAndPopulateSavePanels()
        {
            var saveFiles = loadGameManager.GetAllSaveFiles();
            for (int i = 0; i < savePanels.Count; i++)
            {
                if (i < saveFiles.Count)
                {
                    SaveDataDTO data = loadGameManager.LoadGameDataSync(saveFiles[i]);
                    savePanels[i].SetSaveFileName(saveFiles[i]);
                    savePanels[i].UpdatePanelWithSaveData(data);
                }
                else
                {
                    savePanels[i].ClearPanel();
                }
            }
        }

        private void LoadSaveToPanel(SavePanel panel, string fileName)
        {
            string filePath = Path.Combine(loadGameManager.SaveDirectory, fileName + ".json");
            if (File.Exists(filePath)) {
                string json = File.ReadAllText(filePath);
                SaveDataDTO saveData = JsonUtility.FromJson<SaveDataDTO>(json);
                panel.SetSaveFileName(fileName);  
                panel.UpdatePanelWithSaveData(saveData);
            }
        }
    }
}
