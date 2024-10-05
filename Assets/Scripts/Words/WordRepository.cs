using System.Collections.Generic;
using Analytics;
using UnityEngine;

namespace Words
{
    public static class WordRepository
    {
        private static readonly Dictionary<string, List<WordData>> wordsBySet = new Dictionary<string, List<WordData>>(); 
        private static readonly Dictionary<WordLength, List<WordData>> wordsByLength = new Dictionary<WordLength, List<WordData>>();

        /// <summary>
        /// Get all words of a specific length.
        /// </summary>
        public static List<WordData> GetWordsByLength(WordLength length)
        {
            return wordsByLength.TryGetValue(length, value: out var value) ? value : new List<WordData>();
        }

        /// <summary>
        /// Retrieve all words stored in the word repository.
        /// </summary>
        /// <returns>A list of all words in the repository.</returns>
        public static List<WordData> GetAllWords()
        {
            var allWords = new List<WordData>();

            foreach (var wordList in wordsBySet.Values)
            {
                allWords.AddRange(wordList);
            }

            return allWords;
        }

        /// <summary>
        /// Adds a list of words to the specified set in the repository.
        /// </summary>
        /// <param name="setName">The name of the set to add the words to.</param>
        /// <param name="words">The list of words to be added to the repository.</param>
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