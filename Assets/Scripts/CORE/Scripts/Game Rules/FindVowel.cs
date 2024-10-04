using System.Collections.Generic;

using UnityEngine;

namespace CORE.Scripts.Game_Rules
{

    /// <summary>
    /// An implementation of IGameRules for games where the player is looking for vowels
    /// </summary>
    public class FindVowel : IGameRules
    {

        /// <summary>
        /// Returns a random letter of the correct type
        /// </summary>
        /// <returns>a random letter of the correct type</returns>
        public string GetCorrectAnswer()
        {
            
            
            Debug.Log("Getting vowel");
            var vowel
                = GameManager.Instance.DynamicDifficultyAdjustmentManager
                    .GetNextLanguageUnitsBasedOnLevel(1);
            
            Debug.Log("Vowel: " + vowel[0].Identifier);            
            return vowel[0].Identifier;
            
            return LetterManager.GetRandomVowel().ToString();
        }

        /// <summary>
        /// Returns a random letter of the wrong type
        /// </summary>
        /// <returns> a random letter of the wrong type</returns>
        public string GetWrongAnswer()
        {
            return LetterManager.GetRandomConsonant().ToString();
        }
        /// <summary>
        /// Not used
        /// </summary>
        public void SetCorrectAnswer()
        {

        }

        /// <summary>
        /// Checks whether a letter is a vowel
        /// </summary>
        /// <param name="symbol">the letter to be checked</param>
        /// <returns>Whether the letter is a vowel</returns>
        public bool IsCorrectSymbol(string symbol)
        {
            return LetterManager.GetDanishVowels().Contains(symbol.ToUpper()[0]);
        }

        /// <summary>
        /// Returns the word vokaler
        /// </summary>
        /// <returns>the word vokaler</returns>
        public string GetDisplayAnswer()
        {
            return "vokaler";
        }

        /// <summary>
        /// Not used. Always returns true
        /// </summary>
        /// <returns>true always</returns>
        public bool SequenceComplete()
        {
            return true;
        }

        public string GetSecondaryAnswer()
        {
            return "";
        }
    }
}