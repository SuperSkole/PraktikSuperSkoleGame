using System.Collections.Generic;
using System.Linq;
using CORE.Scripts;
using Scenes._05_Minigames.WordFactory.Scripts.Managers;
using UnityEngine;

namespace Scenes._05_Minigames.WordFactory.Scripts
{
    /// <summary>
    /// Strategy for handling a game mode with 1 gear, where the gear has 9 teeth and the letters are Danish vowels.
    /// Consonants are sent to a text block for the player to form words.
    /// </summary>
    public class SingleGearStrategy : IGearStrategy
    {
        private List<char> consonants;

        public SingleGearStrategy()
        {
            consonants = new List<char>();
        }

        public List<List<char>> GetLettersForGears()
        {
            // The number of teeth on the gear is fixed at 9
            int numberOfTeeth = 9;

            // Fetch a random word with the required length from WordManager
            //HashSet<string> words = WordsManager.GetWordsByLength(WordFactoryGameManager.Instance.DifficultyLevel);
            HashSet<string> words = WordsManager.GetWordsByLength(3);
            //string selectedWord = words.FirstOrDefault();

            // Uncomment the following line for testing with the word "BLE"
            string selectedWord = "BLE";

            if (string.IsNullOrEmpty(selectedWord))
            {
                Debug.LogError("No valid word found with the required length.");
                return null;
            }

            // Fetch Danish vowels
            List<char> danishVowels = LetterManager.GetDanishVowels();

            // Separate vowels and consonants
            List<char> gearLetters = new List<char>();
            foreach (char letter in selectedWord)
            {
                if (danishVowels.Contains(letter))
                {
                    gearLetters.Add(letter);
                }
                else
                {
                    consonants.Add(letter);
                }
            }

            // Fill the remaining slots on the gear with additional vowels if needed
            while (gearLetters.Count < numberOfTeeth)
            {
                gearLetters.Add(danishVowels[Random.Range(0, danishVowels.Count)]);
            }

            // Return the vowels for the gear
            return new List<List<char>> { gearLetters };
        }

        public List<char> GetConsonants()
        {
            return consonants;
        }
    }
}
