using System;
using System.Collections.Generic;
using System.IO;
using CORE;
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

        private string username;
        

        private void Start()
        {
            username = GameManager.Instance.CurrentUser;
            
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
                if (i < saveFiles.Count && saveFiles[i].StartsWith(username))
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
    }
}
