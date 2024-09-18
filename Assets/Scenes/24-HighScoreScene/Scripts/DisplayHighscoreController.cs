using System;
using System.Linq;
using Scenes._10_PlayerScene.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes._24_HighScoreScene.Scripts
{
    public class HighScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI wordsText;
        [SerializeField] private TextMeshProUGUI lettersText;
        [SerializeField] private TextMeshProUGUI numbersText;
        [SerializeField] private Image exitImageButton;
        
        private HighScore highScore;

        private void Awake()
        {
            highScore = PlayerManager.Instance.HighScore;
        }

        private void Start()
        {
            DisplayWords();
            DisplayLetters();
            DisplayNumbers();
            exitImageButton.gameObject.AddComponent<Button>().onClick.AddListener(OnExitButton);
        }

        private void OnExitButton()
        {
            SceneManager.LoadScene(SceneNames.Main); 
        }

        private void DisplayWords()
        {
            var sortedWords = highScore.CollectedWords.OrderByDescending(pair => pair.Value);
            wordsText.text = "Words Found:\n";
            foreach (var pair in sortedWords)
            {
                wordsText.text += $"{pair.Key}: {pair.Value}\n";
            }
        }

        private void DisplayLetters()
        {
            var sortedLetters = highScore.CollectedLetters.OrderByDescending(pair => pair.Value);
            lettersText.text = "Letters Found:\n";
            foreach (var pair in sortedLetters)
            {
                lettersText.text += $"{pair.Key}: {pair.Value}\n";
            }
        }

        private void DisplayNumbers()
        {
            var sortedNumbers = highScore.CollectedNumbers.OrderByDescending(pair => pair.Value);
            numbersText.text = "Letters Found:\n";
            foreach (var pair in sortedNumbers)
            {
                numbersText.text += $"{pair.Key}: {pair.Value}\n";
            }
        }
    }
}