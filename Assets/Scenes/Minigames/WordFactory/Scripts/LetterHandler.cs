using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using CORE.Scripts;
using Scenes.Minigames.WordFactory.Scripts.Managers;

namespace Scenes.Minigames.WordFactory.Scripts
{
    public class LetterHandler : MonoBehaviour
    {
        [SerializeField] private WordValidation wordValidation;

        public void DistributeLetters()
        {
            List<List<char>> gearLetters = GetLettersForGears();
            AssignLettersToGears(gearLetters);
        }

        public void ResetLetters()
        {
            List<List<char>> gearLetters = GetLettersForGears();
            AssignLettersToGears(gearLetters);
        }

        private List<List<char>> GetLettersForGears()
        {
            int numberOfGears = GameManager.Instance.GetNumberOfGears();
            int numberOfTeeth = GameManager.Instance.GetNumberOfTeeth();
            int difficulty = GameManager.Instance.GetDifficultyLevel();

            // Calculate the number of words based on the difficulty
            int numberOfWords = numberOfTeeth - difficulty;

            // Fetch random words equal to the number of required words
            List<string> words = LettersAndWordsManager.GetRandomWords(numberOfWords);

            // Ensure we have enough words
            if (words.Count < numberOfWords)
            {
                Debug.LogError("Not enough valid words available.");
                return null;
            }

            // Split the words into letters for each gear, filling with random letters if the word is too short
            List<List<char>> gearLetters = SplitWordsIntoLetters(words, numberOfGears);

            // Fill remaining letters with random letters from the Danish alphabet
            FillRemainingLetters(gearLetters, numberOfTeeth);

            return gearLetters;
        }

        private List<List<char>> SplitWordsIntoLetters(List<string> words, int numberOfGears)
        {
            List<List<char>> gearLetters = new List<List<char>>();
            for (int i = 0; i < numberOfGears; i++)
            {
                gearLetters.Add(new List<char>());
            }

            foreach (var word in words)
            {
                for (int gearIndex = 0; gearIndex < numberOfGears; gearIndex++)
                {
                    if (gearIndex < word.Length)
                    {
                        gearLetters[gearIndex].Add(word[gearIndex]);
                    }
                    else
                    {
                        // Add a random letter if the word is shorter than the number of gears
                        gearLetters[gearIndex].Add(LettersAndWordsManager.GetRandomLetters(1).First());
                    }
                }
            }

            return gearLetters;
        }

        private void FillRemainingLetters(List<List<char>> gearLetters, int numberOfTeeth)
        {
            // Ensure each gear has the required number of teeth
            foreach (var gear in gearLetters)
            {
                while (gear.Count < numberOfTeeth)
                {
                    gear.Add(LettersAndWordsManager.GetRandomLetters(1).First());
                }
            }
        }

        private void AssignLettersToGears(List<List<char>> gearLetters)
        {
            if (gearLetters == null) return;

            List<GameObject> gears = GameManager.Instance.GetGears();
            for (int gearIndex = 0; gearIndex < gears.Count; gearIndex++)
            {
                GameObject gear = gears[gearIndex];
                var teethContainer = gear.transform.Find("TeethContainer");
                if (teethContainer != null)
                {
                    int toothIndex = 0;
                    foreach (Transform tooth in teethContainer)
                    {
                        if (toothIndex < gearLetters[gearIndex].Count)
                        {
                            AddLetterToTooth(tooth.gameObject, gearLetters[gearIndex][toothIndex]);
                        }
                        toothIndex++;
                    }
                }
            }
        }

        private void AddLetterToTooth(GameObject tooth, char letter)
        {
            Canvas canvas = tooth.GetComponentInChildren<Canvas>();
            if (canvas == null)
            {
                Debug.Log("Failed to find Canvas on tooth");
                return;
            }

            TextMeshProUGUI textMeshPro = canvas.GetComponentInChildren<TextMeshProUGUI>();
            if (textMeshPro == null)
            {
                Debug.Log("Failed to find TextMeshProUGUI on tooth");
                return;
            }
            
            textMeshPro.rectTransform.localPosition = new Vector3(0, -1f, 0);
            textMeshPro.text = letter.ToString();
            textMeshPro.fontSize = 1;
            textMeshPro.color = Color.black;
        }
    }
}
