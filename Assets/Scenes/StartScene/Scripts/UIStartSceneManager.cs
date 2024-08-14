using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.StartScene.Scripts
{
    public class UIStartSceneManager : MonoBehaviour
    {
        // Screen to choose between using an existing save or creating a new one
        [SerializeField] private GameObject SaveOrNew;
        // Screen for character selection
        [SerializeField] private GameObject CharacterChoice;
        // Screen showing saved games
        [SerializeField] private GameObject SaveScene;

        // Name of the game world scene
        [SerializeField] private string sceneName;

        // Currently active UI screen
        private GameObject currentActiveScreen;

        // Activates a given screen and deactivates the current one
        private void ActivateScreen(GameObject newScreen)
        {
            if (currentActiveScreen != null)
            {
                currentActiveScreen.SetActive(false);
            }
            newScreen.SetActive(true);
            currentActiveScreen = newScreen;
        }

        // Activates the screen to choose new or existing game
        public void ActivateSaveOrNewScreen()
        {
            ActivateScreen(SaveOrNew);
        }

        // Activates the character choice screen
        public void ActivateCharacterChoice()
        {
            ActivateScreen(CharacterChoice);
        }

        // Activates the save game screen
        public void ActivateSaveScene()
        {
            ActivateScreen(SaveScene);
        }

        // Transitions to the game world scene, loading necessary player data
        public void ChangeToGameScene()
        {
            // Ensure player data is loaded before scene transition
            SceneManager.LoadScene(sceneName);
        }
    }
}