using System;
using LoadSave;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scenes._03_StartScene.Scripts
{
    public class SavePanel : MonoBehaviour, IPointerClickHandler
    {
        public event Action<string> OnLoadRequested;
        
        [SerializeField] private Image chosenCharacter;
        [SerializeField] private Image playerNameDrawing;
        [SerializeField] private TextMeshProUGUI monsterNameText;
        [SerializeField] private TextMeshProUGUI playerInfo; // TODO maybe level or xp or gold
        [SerializeField] private Image blockingImage;
        [SerializeField] private Image startGameButton;
        
        private string saveKey;

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
            Debug.Log("Load game button pressed: " + saveKey);
            OnLoadRequested?.Invoke(saveKey); 
        }

        public void SetSaveKey(string key)
        {
            Debug.Log("Setting save key: " + key);
            saveKey = key;
            UpdateButtonVisibility();
        }
        
        public void ClearPanel()
        {
            blockingImage.enabled = true;
        }
        
        public void UpdatePanelWithSaveData(SaveDataDTO saveData)
        {
            if (saveData != null)
            {
                // Update UI with player details
                monsterNameText.text = saveData.MonsterName;
                startGameButton.gameObject.SetActive(true);
                blockingImage.enabled = false;
            }
        }

        private void UpdateButtonVisibility()
        {
            startGameButton.gameObject.SetActive(!string.IsNullOrEmpty(saveKey));
        }
    }
}
