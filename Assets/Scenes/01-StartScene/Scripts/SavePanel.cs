using System;
using LoadSave;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scenes.StartScene.Scripts
{
    public class SavePanel : MonoBehaviour, IPointerClickHandler
    {
        public event Action<string> OnLoadRequested;
        
        [SerializeField] private Image chosenCharacter;
        [SerializeField] private Image playerNameDrawing;
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI playerInfo; // TODO maybe level or xp or gold
        [SerializeField] private Image blockingImage;
        [SerializeField] private Image startGameButton;
        
        private string saveFileName;

        private void OnEnable() 
        {
            LoadGameController.Instance.RegisterPanel(this);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnLoadButtonPressed();
        }
        
        public void OnLoadButtonPressed()
        {
            Debug.Log("Load game button pressed: " + saveFileName);
            OnLoadRequested?.Invoke(saveFileName);
        }
        
        public void ClearPanel()
        {
            blockingImage.enabled = true;
        }
        
        public void UpdatePanelWithSaveData(SaveDataDTO saveData)
        {
            if (saveData != null)
            {
                playerName.text = saveData.PlayerName;
                startGameButton.gameObject.SetActive(true);
                blockingImage.enabled = false;
            }
        }
        
        public void SetSaveFileName(string fileName) 
        {
            Debug.Log("Setting save file name: " + fileName);
            saveFileName = fileName;
            UpdateButtonVisibility();
        }

        private void UpdateButtonVisibility() 
        {
            startGameButton.gameObject.SetActive(!string.IsNullOrEmpty(saveFileName));
        }
    }
}
