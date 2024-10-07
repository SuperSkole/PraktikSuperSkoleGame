using System.Text.RegularExpressions;
using CORE;
using TMPro;
using UnityEngine;

namespace Scenes._03_StartScene.Scripts
{
    public class MonsterNameInputValidationController : MonoBehaviour
    {
        private const string MonsterNamePattern = @"^[a-zA-Z\d\-_]+$";
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
                    = "<color=red>Navn må kun indeholde bogstaver, tal, - _</color>";
                return false;
            }
            else
            {
                feedback.text = "<color=green>✔ Gyldigt Monsternavn</color>";
                return true;
            }
        }
    }
}