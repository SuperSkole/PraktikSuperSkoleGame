using Analytics;

namespace CORE.Scripts.Game_Rules
{
    /// <summary>
    /// Implementation of IGameRules for games where the player should look for a specific letter
    /// </summary>
    public class FindCorrectLetter : IGameRules
    {
        string correctLetter;

        /// <summary>
        /// returns the variable correctLetter
        /// </summary>
        /// <returns>the correct letter</returns>
        public string GetCorrectAnswer()
        {
            return correctLetter;
        }

        /// <summary>
        /// Returns the correct letter in uppercase
        /// </summary>
        /// <returns>the correct letter in uppercase</returns>
        public string GetDisplayAnswer()
        {
            return correctLetter.ToUpper();
        }

        public string GetSecondaryAnswer()
        {
            return correctLetter.ToUpper();
        }

        /// <summary>
        /// Returns a random letter which is not the correct one
        /// </summary>
        /// <returns>A random letter which is not the correct one</returns>
        public string GetWrongAnswer()
        {
            string letter = LetterManager.GetRandomLetter().ToString().ToLower();
            while(letter == GetCorrectAnswer())
            {
                letter = LetterManager.GetRandomLetter().ToString().ToLower();
            }
            return letter;
        }

        /// <summary>
        /// Checks if the lowercase version of the given letter is the same as the correct one
        /// </summary>
        /// <param name="symbol">The symbol to be checked</param>
        /// <returns>Whether it is the correct one</returns>
        public bool IsCorrectSymbol(string symbol)
        {
            return correctLetter.ToLower() == symbol.ToLower();
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
            correctLetter = GameManager.Instance.PerformanceWeightManager.GetNextLanguageUnitsByTypeAndCategory(LanguageUnit.Letter, LetterCategory.All, 1)[0].Identifier;;
        }
    }
}
