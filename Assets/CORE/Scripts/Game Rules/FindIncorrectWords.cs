using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CORE.Scripts.Game_Rules
{
    /// <summary>
    /// Implementation of IGameRules for games where the player should look for a specific letter
    /// </summary>
    public class FindIncorrectWords : IGameRules
    {
        string correctLetter;
        WordValidator wordValidator = new WordValidator();

        /// <summary>
        /// returns a random vowel
        /// </summary>
        /// <returns>A random vowel</returns>
        public string GetCorrectAnswer()
        {
            return LetterManager.GetRandomVowel().ToString();
        }

        /// <summary>
        /// not used
        /// </summary>
        /// <returns>the correct letter in uppercase</returns>
        public string GetDisplayAnswer()
        {
            return "";
        }
        
        /// <summary>
        /// Returns a random consonant
        /// </summary>
        /// <returns>A random consonant</returns>
        public string GetWrongAnswer()
        {
            return LetterManager.GetRandomConsonant().ToString();
        }

        /// <summary>
        /// Checks if the lowercase version of the given letter is the same as the correct one
        /// </summary>
        /// <param name="symbol">The symbol to be checked</param>
        /// <returns>Whether it is the correct one</returns>
        public bool IsCorrectSymbol(string symbol)
        {
            return !wordValidator.IsValidWord(symbol, symbol.Length);
        }


        /// <summary>
        /// not used
        /// </summary>
        /// <returns>always true</returns>
        public bool SequenceComplete()
        {
            return true;
        }

        /// <summary>
        /// not used
        /// </summary>
        public void SetCorrectAnswer()
        {
            
        }
    }
}
