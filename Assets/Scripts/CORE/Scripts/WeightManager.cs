using System;
using System.Collections.Generic;
using System.Linq;

namespace CORE.Scripts
{
    public class WeightManager : IWeightManager
    {
        private Dictionary<char, LetterData> letterData = new Dictionary<char, LetterData>();
        private const int START_WEIGHT = 50;

        public void InitializeWeights(IEnumerable<char> letters)
        {
            foreach (var letter in letters)
            {
                letterData[letter] = new LetterData
                {
                    Letter = letter,
                    Weight = START_WEIGHT,
                    LastUsed = DateTime.MinValue,
                    ErrorCount = 0
                };
            }
        }

        public Dictionary<char, int> GetCurrentWeights()
        {
            return letterData.ToDictionary(ld => ld.Key, ld => ld.Value.Weight);
        }

        public void UpdateWeight(char letter, bool isCorrect)
        {
            if (letterData.ContainsKey(letter))
            {
                var data = letterData[letter];
                if (isCorrect)
                {
                    data.Weight = Math.Max(data.Weight - 5, 1);
                }
                else
                {
                    data.Weight = Math.Min(data.Weight + 5, 99);
                    data.ErrorCount++;
                }
                data.LastUsed = DateTime.Now;
            }
        }
    }
}