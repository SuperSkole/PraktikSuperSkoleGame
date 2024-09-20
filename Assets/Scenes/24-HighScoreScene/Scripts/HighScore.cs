using System;
using System.Collections.Generic;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

namespace Scenes._24_HighScoreScene.Scripts
{
    /// <summary>
    /// Serializable class to hold string-int key-value pairs.
    /// </summary>
    [Serializable]
    public class WordCount
    {
        public string Word;
        public int Count;
    }

    /// <summary>
    /// Serializable class to hold char-int key-value pairs.
    /// </summary>
    [Serializable]
    public class CharCount
    {
        public char Character;
        public int Count;
    }

    public class HighScore : MonoBehaviour
    {
        // List for serializing word count
        [SerializeField]
        private List<WordCount> collectedWordsList = new List<WordCount>();

        // List for serializing letter count
        [SerializeField]
        private List<CharCount> collectedLettersList = new List<CharCount>();

        // List for serializing number count
        [SerializeField]
        private List<CharCount> collectedNumbersList = new List<CharCount>();

        // Dictionaries to work with during runtime
        public Dictionary<string, int> CollectedWords = new Dictionary<string, int>();
        public Dictionary<char, int> CollectedLetters = new Dictionary<char, int>();
        public Dictionary<char, int> CollectedNumbers = new Dictionary<char, int>();

        private void OnEnable()
        {
            PlayerEvents.OnAddLetter += AddLetter;
            PlayerEvents.OnAddWord += AddWord;
            PlayerEvents.OnAddNumber += AddNumber;
        }

        private void OnDisable()
        {
            PlayerEvents.OnAddLetter -= AddLetter;
            PlayerEvents.OnAddWord -= AddWord;
            PlayerEvents.OnAddNumber -= AddNumber;
        }

        /// <summary>
        /// Adds a word to the dictionary and updates the serializable list.
        /// </summary>
        public void AddWord(string word)
        {
            if (string.IsNullOrEmpty(word)) return;

            if (CollectedWords.ContainsKey(word))
            {
                CollectedWords[word]++;
            }
            else
            {
                CollectedWords[word] = 1;
            }

            // Update the serializable list
            UpdateWordCountList();
        }

        /// <summary>
        /// Adds a letter to the dictionary and updates the serializable list.
        /// </summary>
        public void AddLetter(char letter)
        {
            if (CollectedLetters.ContainsKey(letter))
            {
                CollectedLetters[letter]++;
            }
            else
            {
                CollectedLetters[letter] = 1;
            }

            // Update the serializable list
            UpdateCharCountList(collectedLettersList, CollectedLetters);
        }

        /// <summary>
        /// Adds a number to the dictionary and updates the serializable list.
        /// </summary>
        public void AddNumber(char number)
        {
            if (CollectedNumbers.ContainsKey(number))
            {
                CollectedNumbers[number]++;
            }
            else
            {
                CollectedNumbers[number] = 1;
            }

            // Update the serializable list
            UpdateCharCountList(collectedNumbersList, CollectedNumbers);
        }

        /// <summary>
        /// Updates the serialized list of word counts to reflect the dictionary.
        /// </summary>
        private void UpdateWordCountList()
        {
            collectedWordsList.Clear();
            foreach (var kvp in CollectedWords)
            {
                collectedWordsList.Add(new WordCount { Word = kvp.Key, Count = kvp.Value });
            }
        }

        /// <summary>
        /// Updates the serialized list of char counts to reflect the dictionary.
        /// </summary>
        private void UpdateCharCountList(List<CharCount> targetList, Dictionary<char, int> sourceDict)
        {
            targetList.Clear();
            foreach (var kvp in sourceDict)
            {
                targetList.Add(new CharCount { Character = kvp.Key, Count = kvp.Value });
            }
        }
    }
}
