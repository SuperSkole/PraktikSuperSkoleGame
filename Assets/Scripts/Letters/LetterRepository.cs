using System.Collections.Generic;
using Analytics;

namespace Letters
{
    /// <summary>
    /// Provides methods to retrieve collections of Danish letters.
    /// </summary>
    public static class LetterRepository
    {
        /// <summary>
        /// A collection of all Danish letters.
        /// </summary>
        private static readonly HashSet<char> AllDanishLetters = new HashSet<char>
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            '\u00c6', '\u00d8', '\u00c5'
            // \u00c6, \u00d8 and \u00c5 are the unicode codes for the three danish vowels,
            // which can sometimes go missing when typed directly in scripts.
        };

        /// <summary>
        /// A collection of Danish vowels.
        /// </summary>
        private static readonly HashSet<char> DanishVowels = new HashSet<char>
        {
            'A', 'E', 'I', 'O', 'U', 'Y', '\u00c6', '\u00d8', '\u00c5'
            // \u00c6, \u00d8 and \u00c5 are the unicode codes for the three danish vowels,
            // which can sometimes go missing when typed directly in scripts.
        };

        /// <summary>
        /// A collection of Danish consonants.
        /// </summary>
        private static readonly HashSet<char> Consonants = new HashSet<char>
        {
            'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q',
            'R', 'S', 'T', 'V', 'W', 'X', 'Z'
        };

        /// <summary>
        /// Retrieves a collection of all Danish letters.
        /// </summary>
        /// <returns>An IEnumerable of all Danish letters.</returns>
        public static IEnumerable<char> GetAllLetters()
        {
            return AllDanishLetters;
        }
        
        /// <summary>
        /// Retrieves a collection of all Danish vowels.
        /// </summary>
        /// <returns>An IEnumerable of all Danish vowels.</returns>
        public static IEnumerable<char> GetVowels()
        {
            return DanishVowels;
        }
        
        /// <summary>
        /// Retrieves a collection of all Danish consonants.
        /// </summary>
        /// <returns>An IEnumerable of all Danish consonants.</returns>
        public static IEnumerable<char> GetConsonants()
        {
            return Consonants;
        }
    }
}