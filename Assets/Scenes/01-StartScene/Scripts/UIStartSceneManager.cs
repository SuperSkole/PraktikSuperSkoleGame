using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.StartScene.Scripts
{
    public class UIStartSceneManager : MonoBehaviour
    {
        // Manages UI screens and character selection.

        // Reference to various UI screens.
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject characterChoiceScreen;
        [SerializeField] private GameObject loadOldSaveScreen;

        // Name of the game scene to load.
        [SerializeField] private string sceneName;

        // Currently active UI screen.
        private GameObject currentActiveScreen;

        // Reference to the image displaying the character selection.
        [SerializeField] private Image displayImage;
        
        // Initializes the start screen as active upon loading.
        private void Awake ()
        {
            ValidateUIReferences();
            startScreen.SetActive(true);
            currentActiveScreen = startScreen;
        }
        
        private void ValidateUIReferences()
        {
            if (startScreen == null ||
                characterChoiceScreen == null ||
                loadOldSaveScreen == null)
                
                Debug.LogError("UI Reference not set in the inspector", this);
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
        public void ActivateStartScreen()
        {
            DeactivateCurrent();
            startScreen.SetActive(true);
            currentActiveScreen = startScreen;
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
