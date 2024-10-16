using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CORE.Scripts
{
    /// <summary>
    /// Manages all operations related to letters.
    /// </summary>
    public class LetterManager
    {
        // \u00c6, \u00d8 and \u00c5 are the unicode codes for the three danish vowels which can sometimes go missing when typed directly in scripts
        private static readonly HashSet<char> AllDanishLetters = new HashSet<char>
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            '\u00c6', '\u00d8', '\u00c5'
        };

        private static readonly HashSet<char> DanishVowels = new HashSet<char>
        {
            'A', 'E', 'I', 'O', 'U', 'Y', '\u00c6', '\u00d8', '\u00c5'
        };

        private static readonly HashSet<char> Consonants = new HashSet<char>
        {
            'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q',
            'R', 'S', 'T', 'V', 'W', 'X', 'Z'
        };

        private static readonly HashSet<char> FMNSConsonants = new HashSet<char>
        {
           'F','M', 'N','S', 
        };

        private static Dictionary<char, int> _weightedDanishLetters = new Dictionary<char, int>
        {
            {'A', 1}, {'B', 3}, {'C', 3}, {'D', 2}, {'E', 1},
            {'F', 4}, {'G', 2}, {'H', 4}, {'I', 1}, {'J', 8},
            {'K', 5}, {'L', 1}, {'M', 3}, {'N', 1}, {'O', 1},
            {'P', 3}, {'Q', 10}, {'R', 1}, {'S', 1}, {'T', 1},
            {'U', 1}, {'V', 4}, {'W', 4}, {'X', 8}, {'Y', 4},
            {'Z', 10}, {'\u00c6', 6}, {'\u00d8', 7}, {'\u00c5', 6}
        };

        public static List<char> GetRandomLetters(int count)
        {
            return AllDanishLetters.OrderBy(_ => Random.value).Take(count).ToList();
        }


        /// <summary>
        /// Returns a random letter
        /// </summary>
        /// <returns>A random letter</returns>
        public static char GetRandomLetter()
        {
            return AllDanishLetters.ToList()[Random.Range(0, AllDanishLetters.Count)];
        }

        public static List<char> GetRandomVowels(int count)
        {
            return DanishVowels.OrderBy(_ => Random.value).Take(count).ToList();
        }

        /// <summary>
        /// Gets a random vowel
        /// </summary>
        /// <returns>A random Vowel</returns>
        public static char GetRandomVowel()
        {
            return DanishVowels.ToList()[Random.Range(0, DanishVowels.Count)];
        }

        public static List<char> GetRandomConsonants(int count)
        {
            return Consonants.OrderBy(_ => Random.value).Take(count).ToList();
        }

        /// <summary>
        /// Gets a random consonant
        /// </summary>
        /// <returns>A random consonant</returns>
        public static char GetRandomConsonant()
        {
            return Consonants.ToList()[Random.Range(0, Consonants.Count)];
        }

        /// <summary>
        /// Gets a list of all danish vowels
        /// </summary>
        /// <returns>List of all danish vowels</returns>
        public static List<char> GetDanishVowels()
        {
            return DanishVowels.ToList();
        }

        /// <summary>
        /// Gets a list of all consonants
        /// </summary>
        /// <returns>List of all consonants</returns>
        public static List<char>GetConsonants()
        {
            return Consonants.ToList();
        }

        /// <summary>
        /// Gets a list containing the Consonants F,M,N,S
        /// </summary>
        /// <returns>List of all danish vowels</returns>
        public static List<char> GetFMNSConsonants()
        {
            return FMNSConsonants.ToList();
        }

        /// <summary>
        /// Gets a random consonant
        /// </summary>
        /// <returns>A random consonant from the list containing the Consonants F,M,N,S
        public static char GetRandomFMNSConsonant()
        {
            return FMNSConsonants.ToList()[Random.Range(0, FMNSConsonants.Count)];
        }




        /// <summary>
        /// Gets a list of all letters
        /// </summary>
        /// <returns>List of all letters</returns>
        public static List<char>GetAllLetters()
        {
            return AllDanishLetters.ToList();
        }
    }
}