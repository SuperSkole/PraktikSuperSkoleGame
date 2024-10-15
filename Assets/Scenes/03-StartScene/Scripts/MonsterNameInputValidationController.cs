using System.Linq;
using System.Text.RegularExpressions;
using CORE;
using TMPro;
using UnityEngine;

namespace Scenes._03_StartScene.Scripts
{
    public class MonsterNameInputValidationController : MonoBehaviour
    {
        /// <summary>
        /// Regular expression pattern to validate monstername.
        /// Usernames must only contain letters, numbers, and specific Danish characters.
        ///
        /// a-zA-Z: Tillader engelske bogstaver (store og små).
        /// æøåÆØÅ: Inkluderer danske bogstaver.
        /// \d: Tillader tal.
        /// +: Sikrer, at der er mindst ét tilladt tegn i input.
        /// ^ og $: Sikrer, at hele inputtet skal overholde mønsteret uden andre tegn i starten eller slutningen.
        /// </summary>
        private const string MonsterNamePattern = @"^[a-zA-ZæøåÆØÅ\d]+$";

        private const int MinMonsterNameLength = 3;
        private const int MaxMonsterNameLength = 15;

        /// <summary>
        /// Validates the given monster name based on specific rules and provides feedback.
        /// </summary>
        /// <param name="input">The monster name input to validate.</param>
        /// <param name="feedback">The TextMeshProUGUI element to display feedback messages.</param>
        /// <returns>True if the monster name is valid, false otherwise.</returns>
        public bool ValidateMonsterName(string input, TextMeshProUGUI feedback)
        {
            bool isLengthValid = input.Length is >= MinMonsterNameLength and <= MaxMonsterNameLength;
            bool containsBannedWord = ProfanityFilter.ContainsProfanity(input);
            bool isPatternValid = Regex.IsMatch(input, MonsterNamePattern);

            if (!isLengthValid)
            {
                feedback.text
                    = "<color=red>Navn skal være mellem 3 og 15 tegn.</color>";
                return false;
            }
            else if (containsBannedWord)
            {
                feedback.text
                    = "<color=red>Navn indeholder upassende ord.</color>";
                return false;
            }
            else if (!isPatternValid)
            {
                feedback.text
                    = "<color=red>Navn må kun indeholde bogstaver eller tal</color>";
                return false;
            }
            else
            {
                feedback.text = "<color=green>Gyldigt Monsternavn</color>";
                return true;
            }
        }
        
        private bool IsAcceptableCharacter(char c)
        {
            // Check if the character is a standard letter or digit
            if (char.IsLetterOrDigit(c))
                return true;
    
            // Include specific Danish characters
            return "æøåÆØÅ".Contains(c);
        }
    }
}