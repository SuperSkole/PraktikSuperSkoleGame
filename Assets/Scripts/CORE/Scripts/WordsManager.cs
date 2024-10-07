using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CORE.Scripts
{
    /// <summary>
    /// Letter And Word Manager stores all letters and wordsOrLetters, and gives minigames a way of populating a vaildWords hashset to use in minigames rounds
    /// </summary>
    public static class WordsManager
    {
        // Used dictionary of hashset as both have O(1) look up  
        private static Dictionary<string, HashSet<string>> _wordSets = new Dictionary<string, HashSet<string>>();
        private static HashSet<string> _validWords = new HashSet<string>();

        /// <summary>
        /// Generates a set name based on the length and optionally a type.
        /// </summary>
        private static string GetSetName(int length, string type = "")
        {
            return $"Words_Danish_{length}L{type}";
        }

        
        // Property to get all set names
        public static IEnumerable<string> AllSetNames => new List<string>(_wordSets.Keys);
        
        #region Loading and reading hashset
        // Populate hashset from data loader
        public static void AddWordToSet(string setName, string word)
        {
            if (!_wordSets.ContainsKey(setName))
            {
                _wordSets[setName] = new HashSet<string>();
            }
            _wordSets[setName].Add(word.ToUpper());
        }
        
        // Fetch wordsOrLetters
        public static HashSet<string> GetWordsFromSet(string setName)
        {
            if (_wordSets.ContainsKey(setName))
            {
                // Return a copy to avoid external modification
                return new HashSet<string>(_wordSets[setName]);
            }
            // Return an empty set if not found
            return new HashSet<string>(); 
        }
        #endregion

        #region Valid Word collection for mini-games

        // Valid Words hashset for temp use in a mini-game
        private static void PopulateValidWords(int length)
        {
            string setName = GetSetName(length);
            if (_wordSets.TryGetValue(setName, out var set))
            {
                _validWords = new HashSet<string>(set);
            }
            else
            {
                // Ensure validWords is empty if no set matches
                Debug.Log("WordsManager.PopulateValidWords(): found no matching hashset");
                _validWords.Clear(); 
            }
        }
        private static void PopulateValidWordsWithListofWords(List<string> words)
        {
            // Add the new wordsOrLetters to the valid set
            _validWords.UnionWith(words); 
        }
        
        public static List<string> PopulateValidWordsWithRandomWordsByLengthAndCount(int length, int count)
        {
            string setName = GetSetName(length);
            if (GetWordsFromSet(setName) is HashSet<string> set)
            {
                List<string> randomWords = set.OrderBy(word => Random.value).Take(count).ToList();
                // Populate valid wordsOrLetters with these random wordsOrLetters
                PopulateValidWordsWithListofWords(randomWords);
                return randomWords;
            }
            Debug.Log("WordsManager.PopulateValidWordsWithRandomWordsByLengthAndCount() returned empty list");
            return new List<string>();
        }
        
        public static HashSet<string> ReturnValidWords()
        {
            // Return a copy to avoid external modification
            return new HashSet<string>(_validWords);
        }
        
        public static void ResetValidWords()
        {
            _validWords.Clear();
            Debug.Log("WordsManager.ResetValidWords(): ValidWords have been reset.");
        }
        #endregion


        #region Get words for games
        public static HashSet<string> GetWordsByLength(int length)
        {
            string setName = GetSetName(length);
            return GetWordsFromSet(setName);
        }

        public static List<string> GetRandomWordsByLengthAndCount(int length, int count)
        {
            string setName = GetSetName(length);
            if (_wordSets.ContainsKey(setName))
            {
                return _wordSets[setName].OrderBy(word => Random.value).Take(count).ToList();
            }

            // Return an empty list if the set is not found and log error
            Debug.Log("LettersAndWordsManager: GetRandomWordsByLength() returned empty list");
            return new List<string>();
        }
        
        public static List<string> GetRandomWordsFromCombinationByCount(int count)
        {
            // This assumes your combination set is named specifically for the combination CSV you mentioned
            string setName = "Words_Danish_3L_Combination";
    
            if (_wordSets.TryGetValue(setName, out var set))
            {
                var randomWords = set.OrderBy(word => Random.value).Take(count).ToList();
                PopulateValidWordsWithListofWords(randomWords);
                return randomWords;
            }
    
            return new List<string>(); // Returns an empty list if no set found or not enough wordsOrLetters are present
        }

        #endregion
    }
}
