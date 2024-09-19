using System.Collections.Generic;
using System.Threading.Tasks;
using CORE;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes._03_StartScene.Scripts
{
    /// <summary>
    /// Manages UI screens and character selection.
    /// </summary>
    public class UIStartSceneManager : MonoBehaviour
    {
        // Reference to various UI screens.
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject newMonster;
        [SerializeField] private GameObject loadMonster;
        [SerializeField] private GameObject characterChoiceScreen;
        [SerializeField] private GameObject loadOldSaveScreen;

        // Name of the game scene to load.
        [SerializeField] private string sceneName;

        // Reference to the image displaying the character selection.
        [SerializeField] private Image displayImage;
        
        // Currently active UI screen.
        private GameObject currentActiveScreen;
        
        // Initializes the start screen as active upon loading.
        private void Awake()
        {
            ValidateUIReferences();
            startScreen.SetActive(true);
            currentActiveScreen = startScreen;
        }

        private async void Start()
        {
            await CheckSaveCountAndToggleUIElements();
        }

        private void ValidateUIReferences()
        {
            if (startScreen == null ||
                characterChoiceScreen == null ||
                loadOldSaveScreen == null)
            {
                Debug.LogError("UI Reference not set in the inspector", this);
            }
        }
        
        private async Task CheckSaveCountAndToggleUIElements()
        {
            // load controller look for saves connected to username
            List<string> saveKeys = await GameManager.Instance.SaveGameController.GetAllSaveKeysFromUserAsync();

            int saveCount = saveKeys.Count;
            
            // Hide or show UI elements based on the number of saves
            if (saveCount == 0)
            {
                // No saves, hide Load Monster and only show Create New Monster
                loadMonster.SetActive(false);
                newMonster.SetActive(true);
            }
            else if (saveCount >= 3)
            {
                // 3 or more saves, disable Create New Monster
                newMonster.SetActive(false);
                loadMonster.SetActive(true);
            }
            else
            {
                // Between 1 and 2 saves, show both options
                loadMonster.SetActive(true);
                newMonster.SetActive(true);
            }
        }

        // Deactivates the currently active UI screen.
        private void DeactivateCurrent()
        {
            if (currentActiveScreen != null)
            {
                currentActiveScreen.SetActive(false);
            }
        }

        // Activates the start screen and deactivates the current screen.
        public async void ActivateStartScreen()
        {
            DeactivateCurrent();
            startScreen.SetActive(true);
            currentActiveScreen = startScreen;
            
            // Recheck the save count only if we are on the start screen
            await CheckSaveCountAndToggleUIElements();
        }

        // Activates the character choice screen and deactivates the current screen.
        public void ActivateCharacterChoice()
        {
            DeactivateCurrent();
            characterChoiceScreen.SetActive(true);
            currentActiveScreen = characterChoiceScreen;
        }

        // Activates the load old save screen and deactivates the current screen.
        public void ActivateSaveScene()
        {
            DeactivateCurrent();
            loadOldSaveScreen.SetActive(true);
            currentActiveScreen = loadOldSaveScreen;
        }

        // Changes to the specified game scene, loading player information if necessary.
        public void ChangeToGameScene()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
