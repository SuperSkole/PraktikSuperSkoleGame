using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CORE.Scripts
{
    public static class LetterManager
    {
        private static ILetterProvider _letterProvider;
        private static IRandomLetterSelector _randomLetterSelector;
        private static IWeightManager _weightManager;

        static LetterManager()
        {
            // Initialiser afhængigheder
            _letterProvider = new LetterProvider();
            _randomLetterSelector = new RandomLetterSelector();
            _weightManager = new WeightManager();
        }

        /// <summary>
        /// Initializes the weights for all letters using the weight manager.
        /// </summary>
        public static void InitializeWeights()
        {
            _weightManager.InitializeWeights(_letterProvider.GetAllLetters());
        }

        public static List<char> GetRandomLetters(int count)
        {
            var allDanishLetters = _letterProvider.GetAllLetters();
            return allDanishLetters.OrderBy(_ => Random.value).Take(count).ToList();
        }

        public static char GetRandomLetter()
        {
            var letters = _letterProvider.GetAllLetters();
            return _randomLetterSelector.GetRandomLetter(letters);
        }

        public static char GetRandomVowel()
        {
            var vowels = _letterProvider.GetVowels();
            return _randomLetterSelector.GetRandomLetter(vowels);
        }

        public static char GetRandomConsonant()
        {
            var consonants = _letterProvider.GetConsonants();
            return _randomLetterSelector.GetRandomLetter(consonants);
        }

        public static char GetWeightedLetter()
        {
            var currentWeights = _weightManager.GetCurrentWeights();
            return _randomLetterSelector.GetWeightedRandomLetter(currentWeights);
        }

        public static void UpdateLetterWeight(char letter, bool isCorrect)
        {
            _weightManager.UpdateWeight(letter, isCorrect);
        }
        
        
        // TODO REMOVE OLD



        /// <summary>
        /// Gets a list containing the Consonants F,M,N,S
        /// </summary>
        /// <returns>List of all danish vowels</returns>
        public static List<char> GetFMNSConsonants()
        {
         HashSet<char> FMNSConsonants = new HashSet<char>
            {
                'F','M', 'N','S', 
            };
            
            return FMNSConsonants.ToList();
        }

        /// <summary>
        /// Gets a random consonant
        /// </summary>
        /// <returns>A random consonant from the list containing the Consonants F,M,N,S
        public static char GetRandomFMNSConsonant()
        {
            HashSet<char> FMNSConsonants = new HashSet<char>
            {
                'F','M', 'N','S', 
            };
            return FMNSConsonants.ToList()[Random.Range(0, FMNSConsonants.Count)];
        }

        /// <summary>
        /// Gets a list of all danish vowels
        /// </summary>
        /// <returns>List of all danish vowels</returns>
        public static List<char> GetDanishVowels()
        {
            var DanishVowels = _letterProvider.GetVowels();
            return DanishVowels.ToList();
        }

        /// <summary>
        /// Gets a list of all consonants
        /// </summary>
        /// <returns>List of all consonants</returns>
        public static List<char>GetConsonants()
        {
           var Consonants = _letterProvider.GetConsonants();
            return Consonants.ToList();
        }

        /// <summary>
        /// Gets a list of all letters
        /// </summary>
        /// <returns>List of all letters</returns>
        public static List<char>GetAllLetters()
        {
            var allLetters = _letterProvider.GetAllLetters();
            return allLetters.ToList();
        }
    }
}