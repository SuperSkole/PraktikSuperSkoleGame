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
    }
}