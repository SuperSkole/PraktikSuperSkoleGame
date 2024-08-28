using Scenes._05_Minigames._56_WordFactory.Scripts;
using Scenes._05_Minigames.WordFactory.Scripts;
using Scenes._05_Minigames.WordFactory.Scripts.Managers;
using Scenes.Minigames.WordFactory.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Scenes.Minigames.WordFactory.Scripts
{
    public class UIButtonManager : MonoBehaviour
    {
        [SerializeField] private WordCheckManager wordCheckManager;
        [SerializeField] private LetterHandler letterHandler;
        [SerializeField] private Button checkWordButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button exitButton;

        private void Start()
        {
            // Assign scene-specific buttons
            checkWordButton.onClick.AddListener(OnCheckWordButton);
            resetButton.onClick.AddListener(OnResetButton);
            exitButton.onClick.AddListener(OnExitButton);
        }

        private void OnCheckWordButton()
        {
            Debug.Log("Check Word button clicked");
            wordCheckManager.CheckForWord();
        }

        private void OnResetButton()
        {
            // Reset the letters on all gears
            letterHandler.ResetLetters();
        }

        private void OnExitButton()
        {
            SceneManager.LoadScene(SceneNames.Main); 
        }
    }
}