using System.Collections.Generic;
using System.Linq;
using CORE.Scripts;
using Scenes._50_Minigames._56_WordFactory.Scripts.Managers;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.GameModeStrategy
{
    /// <summary>
    /// Strategy for handling a game mode with 1 gear, where the gear has 9 teeth and the letters are Danish vowels.
    /// Consonants are sent to a text block for the player to form wordsOrLetters.
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
            var wordList = WordFactoryGameManager.Instance.WordList;
    
            if (!wordList.Any())
            {
                Debug.LogError("No words available for Single Gear Strategy.");
                return null;
            }
            
            // Take the first word (highest-weighted word)
            string selectedWord = wordList.First();

            if (string.IsNullOrEmpty(selectedWord))
            {
                Debug.LogWarning("SingleGearStrategy.GetLettersForWords(): No valid word found.");
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
