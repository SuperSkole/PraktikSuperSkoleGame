using System.Collections.Generic;
using Analytics;
using UnityEngine;

namespace CORE.Scripts.Game_Rules
{

    /// <summary>
    /// An implementation of IGameRules for games where the player is looking for consonants
    /// </summary>
    public class FindConsonant : IGameRules
    {

        /// <summary>
        /// Returns a random letter of the correct type
        /// </summary>
        /// <returns>a random letter of the correct type</returns>
        public string GetCorrectAnswer()
        {
            return GameManager.Instance.PerformanceWeightManager.GetNextLanguageUnitsByTypeAndCategory(LanguageUnit.Letter, LetterCategory.Consonant, 1)[0].Identifier;
        }

        /// <summary>
        /// Returns a random letter of the wrong type
        /// </summary>
        /// <returns> a random letter of the wrong type</returns>
        public string GetWrongAnswer()
        {
            return LetterManager.GetRandomVowel().ToString();
        }
        /// <summary>
        /// Not used
        /// </summary>
        public void SetCorrectAnswer()
        {

        }

        /// <summary>
        /// Checks whether a letter is a consonant
        /// </summary>
        /// <param name="symbol">the letter to be checked</param>
        /// <returns>Whether the letter is a consonant</returns>
        public bool IsCorrectSymbol(string symbol)
        {
            return LetterManager.GetConsonants().Contains(symbol[0]);
        }

        /// <summary>
        /// Returns the word konsonanter
        /// </summary>
        /// <returns>the word konsonanter</returns>
        public string GetDisplayAnswer()
        {
            return "konsonanter";
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
