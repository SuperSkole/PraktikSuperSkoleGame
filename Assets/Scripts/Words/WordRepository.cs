using System.Collections.Generic;
using Analytics;
using UnityEngine;

namespace Words
{
    public static class WordRepository
    {
        private static readonly Dictionary<string, List<WordData>> wordsBySet = new Dictionary<string, List<WordData>>(); 
        private static readonly Dictionary<WordLength, List<WordData>> wordsByLength = new Dictionary<WordLength, List<WordData>>(); 


        // public WordRepository()
        // {
        //     wordsByLength = new Dictionary<WordLength, List<WordData>>();
        //     wordsBySet = new Dictionary<string, List<WordData>>();
        // }

        /// <summary>
        /// Get all words of a specific length.
        /// </summary>
        public static List<WordData> GetWordsByLength(WordLength length)
        {
            return wordsByLength.ContainsKey(length) ? wordsByLength[length] : new List<WordData>();
        }
        
        public static void AddWords(string setName, List<WordData> words)
        {
            if (!wordsBySet.ContainsKey(setName))
            {
                wordsBySet[setName] = new List<WordData>();
            }

            foreach (var word in words)
            {
                wordsBySet[setName].Add(word);

                // Categorize words by length as well
                if (!wordsByLength.ContainsKey(word.Length))
                {
                    wordsByLength[word.Length] = new List<WordData>();
                }
                
                wordsByLength[word.Length].Add(word);
            }
        }
    }
}