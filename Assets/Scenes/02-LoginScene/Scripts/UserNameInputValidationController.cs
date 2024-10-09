using System.Linq;
using System.Text.RegularExpressions;
using CORE;
using TMPro;
using UnityEngine;

namespace Scenes._02_LoginScene.Scripts
{
    /// <summary>
    /// Manages validation of user input for username and password fields and updates the UI accordingly.
    /// </summary>
    public class UserNameInputValidationController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInput;
        [SerializeField] private TextMeshProUGUI usernameFeedback;
        [SerializeField] private TMP_InputField passwordInput;
        [SerializeField] private TextMeshProUGUI passwordTooShortFeedback;
        [SerializeField] private TextMeshProUGUI passwordTooLongFeedback;
        [SerializeField] private TextMeshProUGUI passwordUppercaseFeedback;
        [SerializeField] private TextMeshProUGUI passwordLowercaseFeedback;
        [SerializeField] private TextMeshProUGUI passwordNumberFeedback;
        [SerializeField] private TextMeshProUGUI passwordSpecialFeedback;
        
        /// <summary>
        /// Regular expression pattern to validate usernames.
        /// Usernames must only contain letters, numbers, and specific Danish characters.
        ///
        /// a-zA-Z: Tillader engelske bogstaver (store og små).
        /// æøåÆØÅ: Inkluderer danske bogstaver.
        /// \d: Tillader tal.
        /// +: Sikrer, at der er mindst ét tilladt tegn i input.
        /// ^ og $: Sikrer, at hele inputtet skal overholde mønsteret uden andre tegn i starten eller slutningen.
        /// </summary>
        private const string UsernamePattern = @"^[a-zA-ZæøåÆØÅ\d]+$";
        
        private const int MinUsernameLength = 3;
        private const int MaxUsernameLength = 20;
        
        // Define password requirements as const
        private const int MinPasswordLength = 8;
        private const int MaxPasswordLength = 30;
        private const string UppercasePattern = @"[A-Z]";
        private const string LowercasePattern = @"[a-z]";
        private const string NumberPattern = @"[\d]";
        private const string SpecialCharPattern = @"[\W_]";

        /// <summary>
        /// Initialize input field listeners on start.
        /// </summary>
        private void Start()
        {
            usernameInput.onValueChanged.AddListener(delegate { ValidateUsername(usernameInput.text); });
            passwordInput.onValueChanged.AddListener(delegate { ValidatePassword(passwordInput.text); });
        }

        /// <summary>
        /// Validates the username input against defined criteria.
        /// </summary>
        /// <param name="input">The username input from the user.</param>
        private void ValidateUsername(string input)
        {
            bool isLengthValid = input.Length is >= MinUsernameLength and <= MaxUsernameLength;
            bool containsWhitespace = input.Any(char.IsWhiteSpace);
            bool isPatternValid = Regex.IsMatch(input, UsernamePattern);
            bool containsBannedWord = ProfanityFilter.ContainsProfanity(input);
            bool isValid = isLengthValid && isPatternValid && !containsWhitespace && !containsBannedWord;

            if (!isLengthValid)
            {
                usernameFeedback.text = "<color=red>Brugernavn skal være mellem 3 og 20 tegn.</color>";
            }
            else if (containsWhitespace)
            {
                usernameFeedback.text = "<color=red>Brugernavn må ikke indeholde mellemrum.</color>";
            }
            else if (!isPatternValid)
            {
                usernameFeedback.text = "<color=red>Brugernavn må kun indeholde bogstaver eller tal</color>";
            }
            else if (containsBannedWord)
            {
                usernameFeedback.text = "<color=red>Brugernavn indeholder upassende ord.</color>";
            }
            else
            {
                usernameFeedback.text = "<color=green>✔ Gyldigt Brugernavn</color>";
            }
        }

        /// <summary>
        /// Validates the password input against multiple complexity rules.
        /// </summary>
        /// <param name="input">The password input from the user.</param>
        private void ValidatePassword(string input)
        {
            passwordTooShortFeedback.text = input.Length >= MinPasswordLength ? "<color=green>✔ L\u00e6ngde over 8</color>" : "<color=red>✘ L\u00e6ngde minimum 8</color>";
            passwordTooLongFeedback.text = input.Length <= MaxPasswordLength ? "<color=green>✔ L\u00e6ngde under 30</color>" : "<color=red>✘ L\u00e6ngde Maximum 30</color>";
            passwordUppercaseFeedback.text = Regex.IsMatch(input, UppercasePattern) ? "<color=green>✔ Stort bogstav</color>" : "<color=red>✘ Stort bogstav Mangler</color>";
            passwordLowercaseFeedback.text = Regex.IsMatch(input, LowercasePattern) ? "<color=green>✔ Lille bogstav</color>" : "<color=red>✘ Lille bogstav Mangler</color>";
            passwordNumberFeedback.text = Regex.IsMatch(input, NumberPattern) ? "<color=green>✔ Tal</color>" : "<color=red>✘ Tal mangler</color>";
            passwordSpecialFeedback.text = Regex.IsMatch(input, SpecialCharPattern) ? "<color=green>✔ Specialtegn</color>" : "<color=red>✘ Specialtegn Mangler</color>";
        }
        
        private bool IsAcceptableCharacter(char c)
        {
            // Define acceptable characters explicitly
            const string acceptableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789æøåÆØÅ";
            return acceptableChars.Contains(c);
        }
    }
}
