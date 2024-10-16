using Scenes._10_PlayerScene.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.Managers
{
    /// <summary>
    /// Manages the functionality of the UI buttons and handles pointer click events.
    /// </summary>
    public class UIButtonManager : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private WordCheckManager wordCheckManager; 
        [SerializeField] private LetterHandler letterHandler; 
        [SerializeField] private Button resetButton;
        [SerializeField] private Image checkWordImage;
        [SerializeField] private Image exitImage;
        
        private void Start()
        {
            checkWordImage.gameObject.AddComponent<Button>().onClick.AddListener(OnCheckWordButton);
            exitImage.gameObject.AddComponent<Button>().onClick.AddListener(OnExitButton);
        }

        /// <summary>
        /// Called when the "Check Word" button is clicked.
        /// </summary>
        public void OnCheckWordButton()
        {
            //Debug.Log("Check Word button clicked");
            wordCheckManager.CheckForWord();
        }

        /// <summary>
        /// Called when the "Reset" button is clicked.
        /// </summary>
        public void OnResetButton()
        {
            // Reset the letters on all gears
            letterHandler.ResetLetters();
        }

        /// <summary>
        /// Called when the "Exit" button is clicked.
        /// </summary>
        public void OnExitButton()
        {
            Destroy(PlayerManager.Instance.SpawnedPlayer.GetComponent<AutoMovePlayerInFactory>());
            
            // Load the main scene
            SceneManager.LoadScene(SceneNames.Main); 
        }

        /// <summary>
        /// Handles pointer click events for the buttons.
        /// </summary>
        /// <param name="eventData">Pointer event data</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            // Check which image was clicked
            if (eventData.pointerCurrentRaycast.gameObject == checkWordImage.gameObject)
            {
                Debug.Log("check button clicked");
                
                OnCheckWordButton();
            }
            else if (eventData.pointerCurrentRaycast.gameObject == exitImage.gameObject)
            {
                Debug.Log("exit button clicked");
                OnExitButton();
            }
            else if (eventData.pointerPress == resetButton.gameObject)
            {
                Debug.Log("reset button clicked");
                OnResetButton();
            }
        }
    }
}
