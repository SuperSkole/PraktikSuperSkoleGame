using System.Collections.Generic;
using UnityEngine;

namespace LoadSave
{
    public class HighScore : MonoBehaviour
    {
        // Using Dictionary to track words and their counts.
        public Dictionary<string, int> CollectedWords = new Dictionary<string, int>();
        public Dictionary<char, int> CollectedLetters = new Dictionary<char, int>();
        public Dictionary<int, int> CollectedNumbers = new Dictionary<int, int>();

        public void AddWord(string word)
        {
            if (!CollectedWords.TryAdd(word, 1))
            {
                CollectedWords[word]++; 
            }
        }

    
        public void AddLetter(char letter)
        {
            if (!CollectedLetters.TryAdd(letter, 1))
            {
                CollectedLetters[letter]++;
            }
        }

        public void AddNumber(int number)
        {
            if (!CollectedNumbers.TryAdd(number, 1))
            {
                CollectedNumbers[number]++;
            }
        }
    }
}
