using System;
using LoadSave;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Spine.Unity;
using Scenes._10_PlayerScene.Scripts;

namespace Scenes._03_StartScene.Scripts
{
    /// <summary>
    /// Handles the UI elements and interactions for a save panel, including saving, loading, and deleting game saves.
    /// </summary>
    public class SavePanel : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// Event invoked when a load game request is made.
        /// </summary>
        public event Action<string> OnLoadRequested;
        
        [SerializeField] private Image chosenCharacter;
        [SerializeField] private Image playerNameDrawing;
        [SerializeField] private TextMeshProUGUI MonsterName;
        [SerializeField] private TextMeshProUGUI GoldAmount;
        [SerializeField] private TextMeshProUGUI XPAmount;
        [SerializeField] private TextMeshProUGUI LevelAmount;
        [SerializeField] private Image blockingImage;
        [SerializeField] private Image startGameButton;
        [SerializeField] private Image deleteSaveButton;
        [SerializeField] private Image confirmDeleteButton;
        [SerializeField] private Image cancelDeleteButton;


        [SerializeField] private SkeletonGraphic skeletonGraphic;
        private ClothChanging clothChanging;
        private ColorChanging colorChanging;


        /// <summary>
        /// The key associated with the current save slot.
        /// </summary>
        public string SaveKey { get; private set; }

        /// <summary>
        /// Registers this panel with the LoadGameController when the panel is enabled.
        /// </summary>
        private void OnEnable() 
        {
            LoadGameController.Instance.RegisterPanel(this);

            if (colorChanging == null)
            {
                colorChanging = this.GetComponent<ColorChanging>();
            }
            
            if (clothChanging == null)
            {
                clothChanging = this.GetComponent<ClothChanging>();
            }
        }
        
        /// <summary>
        /// Handles pointer click events to trigger the load game button action.
        /// </summary>
        /// <param name="eventData">The pointer event data associated with the click.</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            OnLoadButtonPressed();
        }

        /// <summary>
        /// Handles the delete button press, shows confirm and cancel buttons.
        /// </summary>
        public void OnDeleteButtonPressed()
        {
            // EO; no savekey
            if (string.IsNullOrEmpty(SaveKey))
            {
                return;
            }

            // Hide the delete button
            deleteSaveButton.gameObject.SetActive(false);

            // Show the confirm/cancel buttons
            confirmDeleteButton.gameObject.SetActive(true);
            cancelDeleteButton.gameObject.SetActive(true);
                
            Debug.Log("Delete save button pressed: " + SaveKey);
        }
        
        /// <summary>
        /// Confirms the deletion of the save and triggers the deletion process through LoadGameController.
        /// </summary>
        public async void OnConfirmDeleteButtonPressed()
        {
            // EO; no savekey
            if (string.IsNullOrEmpty(SaveKey))
            {
                return;
            }

            // Call the LoadGameController to delete the save associated with this SaveKey
            bool isSaveDeleted = await LoadGameController.Instance.DeleteSave(SaveKey);

            // EO; Delete save failed
            if (!isSaveDeleted)
            {
                return;
            }

            Debug.Log("Confirm delete button pressed: " + SaveKey);
                
            // Hide the confirm and cancel buttons
            confirmDeleteButton.gameObject.SetActive(false);
            cancelDeleteButton.gameObject.SetActive(false);
                
            // Clear the panel after the save is deleted
            ClearPanel();
        }

        /// <summary>
        /// Cancels the delete operation and resets the UI elements.
        /// </summary>
        public void OnCancelDeleteButtonPressed()
        {
            // Hide confirm/cancel buttons and bring back the delete button
            confirmDeleteButton.gameObject.SetActive(false);
            cancelDeleteButton.gameObject.SetActive(false);
            deleteSaveButton.gameObject.SetActive(true);
            
            Debug.Log("Cancel delete button pressed. Delete operation aborted.");
        }

        /// <summary>
        /// Handles the logic for loading a game when the load button is pressed.
        /// </summary>
        public void OnLoadButtonPressed()
        {
            Debug.Log("Load game button pressed: " + SaveKey);
            OnLoadRequested?.Invoke(SaveKey); 
        }

        /// <summary>
        /// Sets the save key associated with this panel and updates the button visibility accordingly.
        /// </summary>
        /// <param name="key">The save key to associate with the panel.</param>
        public void SetSaveKey(string key)
        {
            SaveKey = key;
            UpdateButtonVisibility();
        }
        
        /// <summary>
        /// Clears the panel, hiding all relevant UI elements and resetting the save key.
        /// </summary>
        public void ClearPanel()
        {
            // Disable the blocking image and buttons
            blockingImage.enabled = true;
            deleteSaveButton.gameObject.SetActive(false);
            confirmDeleteButton.gameObject.SetActive(false);
            cancelDeleteButton.gameObject.SetActive(false);

            // Clear the text and image data for the panel
            GoldAmount.text = string.Empty;
            XPAmount.text = string.Empty;    
            LevelAmount.text = string.Empty;

            if (chosenCharacter)
            {
                chosenCharacter.sprite = null;
            }

            if (MonsterName)
            {
                MonsterName.text = string.Empty;
            }

            if (GoldAmount)
            {
                GoldAmount.text = string.Empty;
            }

            if (XPAmount)
            {
                XPAmount.text = string.Empty;
            }

            if (LevelAmount)
            {
                LevelAmount.text = string.Empty;
            }

            if (chosenCharacter)
            {
                chosenCharacter.sprite = null;
            }
            

            // Set SaveKey to null to indicate no save is associated with the panel
            SaveKey = null;
    
            Debug.Log("SavePanel cleared after save deletion.");
        }

        /// <summary>
        /// Updates the UI panel with save data, enabling the relevant UI elements.
        /// </summary>
        /// <param name="saveData">The save data to display in the panel.</param>
        public void UpdatePanelWithSaveData(PlayerData saveData)
        {
            // EO; no savedata
            if (saveData == null)
            {
                return;
            }

            // Update UI with player details
            startGameButton.gameObject.SetActive(true);
            deleteSaveButton.gameObject.SetActive(true);
            blockingImage.enabled = false;
            MonsterName.text = saveData.MonsterName;
            GoldAmount.text = saveData.CurrentGoldAmount.ToString();
            XPAmount.text = saveData.CurrentXPAmount.ToString();
            LevelAmount.text = saveData.CurrentLevel.ToString();

            colorChanging.SetSkeleton(skeletonGraphic);
            colorChanging.ColorChange(saveData.MonsterColor);

            clothChanging.ChangeClothes(saveData.ClothMid, skeletonGraphic);
            clothChanging.ChangeClothes(saveData.ClothTop, skeletonGraphic);
        }

        /// <summary>
        /// Updates the visibility of buttons depending on the presence of a valid save key.
        /// </summary>
        private void UpdateButtonVisibility()
        {
            startGameButton.gameObject.SetActive(!string.IsNullOrEmpty(SaveKey));
            deleteSaveButton.gameObject.SetActive(!string.IsNullOrEmpty(SaveKey));
        }
    }
}