using System.Collections.Generic;
using CORE;
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
            string username = GameManager.Instance.CurrentUser;
    
            // Get all save keys from the cloud that are related to monster saves
            List<string> saveKeys = await GameManager.Instance.SaveGameController.GetAllSaveKeysAsync();


            for (int i = 0; i < savePanels.Count; i++)
            {
                if (i < saveKeys.Count)
                {
                    // Load the save data from the cloud for the current key
                    SaveDataDTO data = await GameManager.Instance.SaveGameController.LoadSaveDataAsync(saveKeys[i]);

                    if (data != null)
                    {
                        savePanels[i].SetSaveKey(saveKeys[i]);  
                        savePanels[i].UpdatePanelWithSaveData(data);  
                    }
                }
                else
                {
                    savePanels[i].ClearPanel();  
                }
            }
        }

    }
}
