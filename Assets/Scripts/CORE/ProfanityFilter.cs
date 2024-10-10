using System.Collections.Generic;

namespace CORE
{
    public static class ProfanityFilter
    {
        private static readonly HashSet<string> bannedWords = new HashSet<string>
        {
            "ass", "pussy", "fuck" 
        };

        /// <summary>
        /// Checks if the given input contains any banned words.
        /// </summary>
        /// <param name="input">The input string to check for profanity.</param>
        /// <returns>True if profanity is found, false otherwise.</returns>
        public static bool ContainsProfanity(string input)
        {
            string loweredInput = input.ToLower();
            foreach (string bannedWord in bannedWords)
            {
                if (loweredInput.Contains(bannedWord))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}