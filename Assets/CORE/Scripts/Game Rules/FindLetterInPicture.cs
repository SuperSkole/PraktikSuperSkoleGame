using System.Collections.Generic;
using UnityEngine;

namespace CORE.Scripts.Game_Rules
{
    public class FindLetterInPicture : IGameRules
    {
        private char correctLetter;
        private string displayName;

        /// <summary>
        /// Returns a random letter of the correct type
        /// </summary>
        /// <returns>a random letter of the correct type</returns>
        public string GetCorrectAnswer()
        {
            return correctLetter.ToString();
        }

        /// <summary>
        /// Returns a random letter of the wrong type
        /// </summary>
        /// <returns> a random letter of the wrong type</returns>
        public string GetWrongAnswer()
        {
            string letter = LetterManager.GetRandomLetter().ToString().ToLower();
            while (letter == GetCorrectAnswer())
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
            List<char> vowels = LetterManager.GetDanishVowels();
            string answer = WordsForImagesManager.GetRandomWordForImage();
            char vowel = 'b';
            while (true)
            {
                bool hasOver1Letter = false;
                for (int i = 0; i < answer.Length; i++)
                {
                    char letter = answer[i];
                    if (vowels.Contains(letter) && !vowels.Contains(vowel))
                    {
                        vowel = letter;
                    }
                    else if (vowels.Contains(letter) && vowel != letter)
                    {
                        hasOver1Letter = true;
                        break;
                    }

                }
                if (hasOver1Letter)
                {
                    answer = WordsForImagesManager.GetRandomWordForImage();
                }
                else break;
            }
            displayName = answer;
            correctLetter = vowel;
        }

        /// <summary>
        /// Checks whether a letter is the correct type
        /// </summary>
        /// <param name="symbol">the letter to be checked</param>
        /// <returns>Whether it is the correct type</returns>
        public bool IsCorrectSymbol(string symbol)
        {
            return correctLetter.ToString() == symbol.ToLower();
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