using System;
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
        // public bool IsValidWord(string word, int wordLength)
        // {
        //     string setName = $"Words_Danish_{wordLength}L";
        //     var words = WordsManager.GetWordsFromSet(setName);
        //     return words.Contains(word.ToUpper());
        // }
        
        public bool IsValidWord(string word, int wordLength)
        {
            string setName1 = $"Words_Danish_{wordLength}L";
            string setName2 = $"Words_Danish_{wordLength}L_ALL";

            var words1 = WordsManager.GetWordsFromSet(setName1);
            var words2 = WordsManager.GetWordsFromSet(setName2);

            string wordUpper = word.ToUpper();

            return words1.Contains(wordUpper) || words2.Contains(wordUpper);
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
        /// Validates whether a given word is a valid combination that starts with specific initial consonants and exists in any word set.
        /// </summary>
        /// <param name="word">The word to verify.</param>
        /// <param name="initialConsonants">The initial consonants used to form words (e.g., "sk").</param>
        /// <returns>True if the word starts with the initial consonants and exists in any hashset; otherwise, false.</returns>
        public bool IsValidCombinationWord(string word, string initialConsonants)
        {
            // Ensure the word starts with the specified initial consonants
            if (!word.StartsWith(initialConsonants, StringComparison.OrdinalIgnoreCase))
                return false;

            // Normalize and check if the word exists in the relevant hashset
            string upperWord = word.ToUpper();
            foreach (var setName in WordsManager.AllSetNames)
            {
                var wordsSet = WordsManager.GetWordsFromSet(setName);
                if (wordsSet.Contains(upperWord))
                    return true;
            }
            
            return false;
        }
    }
}