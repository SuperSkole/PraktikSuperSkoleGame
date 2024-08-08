using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CORE.Scripts
{
    public static class LettersAndWordsManager
    {
        // Used hashset for the O(1) look up  
        private static readonly HashSet<string> TwoLetterWords = new HashSet<string>();
        private static readonly HashSet<string> ThreeLetterWords = new HashSet<string>();
        private static readonly HashSet<string> FourLetterWords = new HashSet<string>();
    
        private static readonly HashSet<char> AllDanishLetters = new HashSet<char>
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'Æ', 'Ø', 'Å'
        };

        private static readonly HashSet<char> Vowels = new HashSet<char>
        {
            'A', 'E', 'I', 'O', 'U', 'Y', 'Æ', 'Ø', 'Å'
        };

        private static readonly HashSet<char> Consonants = new HashSet<char>
        {
            'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q',
            'R', 'S', 'T', 'V', 'W', 'X', 'Z'
        };

        private static Dictionary<char, int> _letterWeights = new Dictionary<char, int>
        {
            {'A', 1}, {'B', 3}, {'C', 3}, {'D', 2}, {'E', 1},
            {'F', 4}, {'G', 2}, {'H', 4}, {'I', 1}, {'J', 8},
            {'K', 5}, {'L', 1}, {'M', 3}, {'N', 1}, {'O', 1},
            {'P', 3}, {'Q', 10}, {'R', 1}, {'S', 1}, {'T', 1},
            {'U', 1}, {'V', 4}, {'W', 4}, {'X', 8}, {'Y', 4},
            {'Z', 10}, {'Æ', 6}, {'Ø', 7}, {'Å', 6}
        };

        private static readonly HashSet<string> ValidWords = new HashSet<string>();

        static LettersAndWordsManager()
        {
            LoadWordsFromCSV();
            PopulateTestWords();
        }
    
        private static void LoadWordsFromCSV()
        {
            // find path to file in folder teachingdata + unity assets path
            string filePath = Path.Combine(Application.dataPath, "TeachingData", "Words_Danish_2L.csv");
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string word = line.Trim();
                    switch (word.Length)
                    {
                        case 2:
                            TwoLetterWords.Add(word.ToUpper());
                            break;
                        case 3:
                            ThreeLetterWords.Add(word.ToUpper());
                            break;
                        case 4:
                            FourLetterWords.Add(word.ToUpper());
                            break;
                    }
                }
                Debug.Log("Words loaded successfully.");
            }
            catch (IOException ex)
            {
                Debug.LogError("Error loading words from CSV: " + ex.Message);
            }
        }

        private static void PopulateTestWords()
        {
            string[] words = new string[] { "by", "is", "ål", "tå", "bi", "ko", "lo", "fe", "eg", "le", "si", "ti" , "to", "so" , "el" , "de" , "bo" , "go" , "øv" };

            foreach (string word in words)
            {
                ValidWords.Add(word.ToUpper());
            }
        }

        public static List<string> GetRandomWords(int count)
        {
            List<string> chosenWords = ValidWords.OrderBy(word => Random.value).Take(count).ToList();
            Debug.Log($"Selected Words: {string.Join(", ", chosenWords)}");
            return chosenWords;
        }

        public static HashSet<string> GetValidWords()
        {
            // Return a copy to avoid external modification
            return new HashSet<string>(ValidWords);
        }

        public static List<char> GetRandomLetters(int count)
        {
            return AllDanishLetters.OrderBy(letter => Random.value).Take(count).ToList();
        }

        public static List<char> GetRandomVowels(int count)
        {
            return Vowels.OrderBy(letter => Random.value).Take(count).ToList();
        }

        public static List<char> GetRandomConsonants(int count)
        {
            return Consonants.OrderBy(letter => Random.value).Take(count).ToList();
        }
    }
}
