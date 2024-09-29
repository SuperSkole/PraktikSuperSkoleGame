using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace CORE.Scripts
{
    public class RandomLetterSelector : IRandomLetterSelector
    {
        private Random _random = new Random();

        public char GetRandomLetter(IEnumerable<char> letters)
        {
            Debug.Log("RandomLetterSelector.GetRandomLetter");
            var letterList = letters.ToList();
            int index = _random.NextInt(letterList.Count);
            Debug.Log($"RandomLetterSelector.GetRandomLetter: Return {letterList[index]}");
            return letterList[index];
        }

        public char GetWeightedRandomLetter(Dictionary<char, int> weightedLetters)
        {
            Debug.Log("RandomLetterSelector.GetWeightedRandomLetter");
            int totalWeight = weightedLetters.Values.Sum();
            int randomNumber = _random.NextInt(0, totalWeight);
            int cumulativeWeight = 0;

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