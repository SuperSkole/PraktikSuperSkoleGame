using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Letters
{
    public class RandomLetterSelector : IRandomLetterSelector
    {
        private Random _random;
        
        public RandomLetterSelector()
        {
            _random = new Random();
        }


        public char GetRandomVowel(IEnumerable<char> vowels)
        {
            Debug.Log("RandomLetterSelector.GetRandomVowel");
            var letterList = vowels.ToList();
            int index = _random.NextInt(letterList.Count);
            Debug.Log($"RandomLetterSelector.GetRandomVowel: Return {letterList[index]}");
            return letterList[index];
        }

        public char GetRandomConsonant(IEnumerable<char> consonants)
        {
            Debug.Log("RandomLetterSelector.GetRandomConsonant");
            var letterList = consonants.ToList();
            int index = _random.NextInt(letterList.Count);
            Debug.Log($"RandomLetterSelector.GetRandomConsonant: Return {letterList[index]}");
            return letterList[index];
        }

        public char GetRandomLetter(IEnumerable<char> letters)
        {
            Debug.Log("RandomLetterSelector.GetRandomLetter");
            var letterList = letters.ToList();
            int index = _random.NextInt(letterList.Count);
            Debug.Log($"RandomLetterSelector.GetRandomLetter: Return {letterList[index]}");
            return letterList[index];
        }

        public char GetWeightedRandomLetter(Dictionary<char, float> weightedLetters)
        {
            Debug.Log("RandomLetterSelector.GetWeightedRandomLetter");
            float totalWeight = weightedLetters.Values.Sum();
            float randomNumber = _random.NextInt(0, (int)totalWeight);
            float cumulativeWeight = 0;

            foreach (var kvp in weightedLetters)
            {
                cumulativeWeight += kvp.Value;
                if (randomNumber < cumulativeWeight)
                {
                    Debug.Log($"RandomLetterSelector.GetWeightedRandomLetter: Weight {cumulativeWeight} for {kvp.Key}");
                    return kvp.Key;
                }
            }

            // Fallback if we have an error
            Debug.Log("RandomLetterSelector.GetWeightedRandomLetter: Weight not found using fallback");
            return weightedLetters.Keys.First();
        }
    }
}