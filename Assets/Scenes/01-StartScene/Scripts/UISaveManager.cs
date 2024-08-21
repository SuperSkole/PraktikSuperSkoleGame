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
        [SerializeField] private List<SavePanel> savePanels;
        
        private string saveFileNameOne;
        private string saveFileNameTwo;
        private string saveFileNameThree;

        private string username;
        

        private void Start()
        {
            username = GameManager.Instance.CurrentUser;
        }
        
        public void CheckForSavesAndPopulateSavePanels()
        {
            var saveFiles = GameManager.Instance.LoadManager.GetAllSaveFiles();
            for (int i = 0; i < savePanels.Count; i++)
            {
                if (i < saveFiles.Count && saveFiles[i].StartsWith(username) && !saveFiles[i].Contains("house"))
                {
                    SaveDataDTO data = GameManager.Instance.LoadManager.LoadGameDataSync(saveFiles[i]);
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
