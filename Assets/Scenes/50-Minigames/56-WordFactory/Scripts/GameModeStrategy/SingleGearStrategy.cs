using System.Collections.Generic;
using System.Linq;
using CORE.Scripts;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.GameModeStrategy
{
    /// <summary>
    /// Strategy for handling a game mode with 1 gear, where the gear has 9 teeth and the letters are Danish vowels.
    /// Consonants are sent to a text block for the player to form words.
    /// </summary>
    public class SingleGearStrategy : IGearStrategy
    {
        private List<char> consonants = new();

        public List<char> GetConsonants()
        {
            return consonants;
        }

        public List<List<char>> GetLettersForGears()
        {
            // The number of teeth on the gear is fixed at 9
            int numberOfTeeth = 9;//is never used?

            // Fetch a random word from WordManager
            List<string> words = WordsManager.GetRandomWordsFromCombinationByCount(1);
            Debug.Log("SingleGearStrategy.GetLettersForWords(): Chosen word: " + words[0]);
            string selectedWord = words.FirstOrDefault();

            if (string.IsNullOrEmpty(selectedWord))
            {
                Debug.LogError("SingleGearStrategy.GetLettersForWords(): No valid word found.");
                return null;
            }
            
            string consonantPart = selectedWord.Substring(0, 2);

            // Fetch Danish vowels
            List<char> gearLetters = LetterManager.GetDanishVowels();

            // Add consonants to the list
            consonants = consonantPart.ToList();

            // Shuffle the vowels to ensure they are randomly placed on the gear
            gearLetters = ShuffleList(gearLetters);

            // Return the vowels for the gear
            return new List<List<char>> { gearLetters };
        }

        private List<char> ShuffleList(List<char> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = Random.Range(0, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }

            return list;
        }
    }
}
