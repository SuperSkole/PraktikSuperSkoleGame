using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CORE.Scripts
{
    public class WeightManager : PersistentSingleton<WeightManager>, IWeightManager
    {
        private Dictionary<char, LetterData> letterData = new Dictionary<char, LetterData>();
        private const int START_WEIGHT = 50;

        protected override void Awake()
        {
            //Debug.Log("weight manager awake");
            base.Awake();
        }

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
            if (!letterData.TryGetValue(letter, value: out var data))
            {
                return;
            }

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