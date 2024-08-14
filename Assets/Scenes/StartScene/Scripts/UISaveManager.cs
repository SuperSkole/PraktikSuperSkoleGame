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
        
        [SerializeField] private SavePanel savePanelOne;
        [SerializeField] private SavePanel savePanelTwo;
        [SerializeField] private SavePanel savePanelThree;
        
        private Image imageOne = null;
        private Image nameOne = null;
        private string textOne = null;
        private bool savedOne = false;

        private Image imageTwo = null;
        private Image nameTwo = null;
        private string textTwo = null;
        private bool savedTwo = false;

        private Image imageThree = null;
        private Image nameThree = null;
        private string textThree = null;
        private bool savedThree = false;
        
        private void Awake()
        {
            
            
            
            //Udfyld variablerne her

            FillSaves();
        }

        private void Start()
        {
            if (loadGameManager == null)
            {
                Debug.LogError("LoadGameManager is not assigned.");
                return;
            }
        }

        private void FillSaves()
        {
            //send alt dataen til de tre savepanaler i inspektoren
            savePanelOne.SetSaveData(imageOne, nameOne, textOne, savedOne);
            savePanelTwo.SetSaveData(imageTwo, nameTwo, textTwo, savedTwo);
            savePanelThree.SetSaveData(imageThree, nameThree, textThree, savedThree);
        }
        
        public void CheckForSavesAndDisplay()
        {
            List<string> saveFiles = loadGameManager.GetAllSaveFiles();

            if (saveFiles.Count > 0)
            {
                // Example: Load the first three saves. Adapt based on your UI needs.
                if (saveFiles.Count > 0) LoadSaveToPanel(savePanelOne, saveFiles[0]);
                if (saveFiles.Count > 1) LoadSaveToPanel(savePanelTwo, saveFiles[1]);
                if (saveFiles.Count > 2) LoadSaveToPanel(savePanelThree, saveFiles[2]);
            }
            else
            {
                Debug.Log("No save files found.");
                // Handle no save files found scenario, maybe notify the user
            }
        }

        private void LoadSaveToPanel(SavePanel panel, string fileName)
        {
            string filePath = Path.Combine(loadGameManager.SaveDirectory, fileName + ".json");
            string json = File.ReadAllText(filePath);
            SaveDataDTO saveData = JsonUtility.FromJson<SaveDataDTO>(json);
            
            panel.UpdatePanelWithSaveData(saveData);
        }


    }
}
