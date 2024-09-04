using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CORE.Scripts.GameRules
{

    /// <summary>
    /// An implementation of IGameRules for games where the player is looking for either vowels or consonants
    /// </summary>
    public class FindLetterType : IGameRules
    {
        private List<char> correctLetterType;
        private List<char> vowels;
        private List<char> consonants;
        private string displayName;

        /// <summary>
        /// Returns a random letter of the correct type
        /// </summary>
        /// <returns>a random letter of the correct type</returns>
        public string GetCorrectAnswer()
        {
            return correctLetterType[Random.Range(0, correctLetterType.Count)].ToString();
        }

        /// <summary>
        /// Returns a random letter of the wrong type
        /// </summary>
        /// <returns> a random letter of the wrong type</returns>
        public string GetWrongAnswer()
        {
            string letter = LetterManager.GetRandomLetter().ToString().ToLower();
            while(IsCorrectSymbol(letter))
            {
                letter = LetterManager.GetRandomLetter().ToString().ToLower();
            }
            return letter;
        }

        /// <summary>
        /// Sets the vowels and consonants lists if they havent yet. Afterwards it sets up one of them as the correct type randomly
        /// </summary>
        public void SetCorrectAnswer()
        {
            if(vowels == null || consonants == null)
            {
                vowels = LetterManager.GetDanishVowels();
                consonants = LetterManager.GetConsonants();
            }
            if(Random.Range(0, 2) == 0)
            {
                correctLetterType = vowels;
                displayName = "vokaler";
            }
            else
            {
                correctLetterType = consonants;
                displayName = "konsonanter";
            }
        }

        /// <summary>
        /// Checks whether a letter is the correct type
        /// </summary>
        /// <param name="symbol">the letter to be checked</param>
        /// <returns>Whether it is the correct type</returns>
        public bool IsCorrectSymbol(string symbol)
        {
            return correctLetterType.Contains(symbol.ToUpper()[0]);
        }

        /// <summary>
        /// Returns the displayname of the current letter type
        /// </summary>
        /// <returns>display name of the current letter type</returns>
        public string GetDisplayAnswer()
        {
            return displayName;
        }

        /// <summary>
        /// Not used. Always returns true
        /// </summary>
        /// <returns>true always</returns>
        public bool SequenceComplete()
        {
            return true;
        }
    }
}
