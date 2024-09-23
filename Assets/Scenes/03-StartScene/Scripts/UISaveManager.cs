using System;
using System.Collections.Generic;
using CORE;
using CORE.Scripts;
using LoadSave;
using UnityEngine;

namespace Scenes._03_StartScene.Scripts
{
    public class UISaveManager : MonoBehaviour
    {
        [SerializeField] private List<SavePanel> savePanels;
        
        private string saveFileNameOne;
        private string saveFileNameTwo;
        private string saveFileNameThree;

        private string username;
        

        private void Start()
        {
            username = GameManager.Instance.CurrentUser;
        }
        
        public async void CheckForSavesAndPopulateSavePanels()
        {
            username = GameManager.Instance.CurrentUser;
    
            // Get all save keys from the cloud that are related to monster saves
            List<string> saveKeys = await GameManager.Instance.SaveGameController.GetAllSaveKeysFromUserAsync();
            
            // Group saves by monster name
            Dictionary<string, string> savesByMonster = new Dictionary<string, string>();
            
            foreach (var key in saveKeys)
            {
                var keyParts = key.Split('_');
                
                // Extract the monster name
                string monsterName = keyParts[1];  

                // Add the save to the dictionary
                savesByMonster[monsterName] = key;
            }


            // Populate the save panels with the newest saves for each monster
            int i = 0;
            foreach (var monsterSave in savesByMonster)
            {
                if (i < savePanels.Count)
                {
                    // Load the save data from the cloud for the current key
                    PlayerData data
                        = await GameManager.Instance.SaveGameController
                            .LoadSaveDataAsync(saveKeys[i]);

                    if (data != null)
                    {
                        savePanels[i].SetSaveKey(monsterSave.Value);
                        savePanels[i].UpdatePanelWithSaveData(data);
                    }
                    else
                    {
                        savePanels[i].ClearPanel();
                    }

                    i++;
                }
            }
        }
    }
}
