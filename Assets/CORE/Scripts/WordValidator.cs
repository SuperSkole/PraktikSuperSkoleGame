using System.Collections.Generic;
using UnityEngine;

namespace CORE.Scripts
{
    /// <summary>
    /// Validate that words are correct either on specific hashset or by looking at all hashset
    /// </summary>
    public class WordValidator : MonoBehaviour
    {
        /// <summary>
        /// Provides centralized word validation for mini-games.
        /// </summary>
        /// <param name="word">The word that is needs verification</param>
        /// <param name="wordLength">The length of the word, to get the correct hashset</param>
        /// <returns>True if the word is found in specific hashset; otherwise, false.</returns>
        public bool IsValidWord(string word, int wordLength)
        {
            string setName = $"Words_Danish_{wordLength}L";
            var words = WordsManager.GetWordsFromSet(setName);
            return words.Contains(word.ToUpper());
        }
        
        /// <summary>
        /// Checks if a word is valid in any of the available hashsets.
        /// </summary>
        /// <param name="word">The word to verify.</param>
        /// <returns>True if the word is found in any hashset; otherwise, false.</returns>
        public bool IsWordValidInAnySet(string word)
        {
            foreach (var setName in WordsManager.AllSetNames)
            {
                if (WordsManager.GetWordsFromSet(setName).Contains(word.ToUpper()))
                    return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Validates a word based on the initial consonants and checks if it exists in a hashset.
        /// </summary>
        /// <param name="word">The formed word to verify.</param>
        /// <param name="initialConsonants">The initial consonants used to form words.</param>
        /// <returns>True if the word can be formed from the consonants and exists in any hashset; otherwise, false.</returns>
        public bool IsValidCombinationWord(string word, string initialConsonants)
        {
            // Check if all letters in the word are derivable from the initial consonants
            if (!CanBeFormedFromConsonants(word, initialConsonants))
                return false;

            // Normalize and check if the word exists in the relevant hashset
            foreach (var setName in WordsManager.AllSetNames)
            {
                var wordsSet = WordsManager.GetWordsFromSet(setName);
                if (wordsSet.Contains(word.ToUpper()))
                    return true;
            }
            
            return false;
        }

        private bool CanBeFormedFromConsonants(string word, string consonants)
        {
            Dictionary<char, int> consonantCount = new Dictionary<char, int>();
            foreach (char c in consonants.ToUpper())
            {
                if (!consonantCount.ContainsKey(c))
                    consonantCount[c] = 0;
                consonantCount[c]++;
            }

            foreach (char c in word.ToUpper())
            {
                if (!consonantCount.ContainsKey(c) || consonantCount[c] == 0)
                    return false;
                consonantCount[c]--;
            }
            
            return true;
        }
    }
}