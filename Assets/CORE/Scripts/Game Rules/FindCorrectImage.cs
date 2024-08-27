using CORE.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Scripts.GameRules
{

    /// <summary>
    /// Implementation of IGameRules for games where the player should look for a specific letter
    /// </summary>
    public class FindCorrectImage : IGameRules
    {

        string correctImage;

        /// <summary>
        /// returns the variable correctLetter
        /// </summary>
        /// <returns>the correct letter</returns>
        public string GetCorrectAnswer()
        {
            return correctImage;
        }

        /// <summary>
        /// Returns the correct letter in uppercase
        /// </summary>
        /// <returns>the correct letter in uppercase</returns>
        public string GetDisplayAnswer()
        {
            return correctImage.ToUpper();
        }

        /// <summary>
        /// Returns a random letter which is not the correct one
        /// </summary>
        /// <returns>A random letter which is not the correct one</returns>
        public string GetWrongAnswer()
        {
            string word = WordsForImagesManager.GetRandomWordForImage().ToString().ToLower();
            while (word == GetCorrectAnswer())
            {
                word = WordsForImagesManager.GetRandomWordForImage().ToString().ToLower();
            }
            return word;
        }

        /// <summary>
        /// Checks if the lowercase version of the given letter is the same as the correct one
        /// </summary>
        /// <param name="symbol">The symbol to be checked</param>
        /// <returns>Whether it is the correct one</returns>
        public bool IsCorrectSymbol(string symbol)
        {
            return correctImage == symbol.ToLower();
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
        /// changes correctLetter to a new one
        /// </summary>
        public void SetCorrectAnswer()
        {
            correctImage = WordsForImagesManager.GetRandomWordForImage().ToString().ToLower();
        }
    }
}
