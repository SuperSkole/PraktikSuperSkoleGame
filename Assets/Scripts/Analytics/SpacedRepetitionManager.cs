using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CORE;
using CORE.Scripts;
using Letters;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

namespace Analytics
{
    public class SpacedRepetitionManager : PersistentSingleton<SpacedRepetitionManager>, ISpacedRepetitionManager
    {
        private readonly IWeightManager weightManager;
        private readonly TimeSpan repetitionInterval = TimeSpan.FromDays(7);

        private ConcurrentDictionary<string, LetterData> letterWeights;
        
        public void EnsureInitialized()
        {
            if (letterWeights == null)
            {
                letterWeights = PlayerManager.Instance.PlayerData.LettersWeightsProperty;
            }
        }

        public void InitializeTimeWeights()
        {
            UpdateWeightsBasedOnTime();
        }

        public void UpdateWeightsBasedOnTime()
        {
            EnsureInitialized();

            foreach (var unit in letterWeights.Values)
            {
                unit.TimeWeight = CalculateTimeWeight(unit.LastUsed);
            }

            // foreach (var unit in WordWeights.Values)
            // {
            //     unit.TimeWeight = CalculateTimeWeight(unit.LastUsed);
            // }
        }

        private int CalculateTimeWeight(DateTime lastUsed)
        {
            int daysSinceLastUsed = (int)(DateTime.UtcNow - lastUsed).TotalDays;
    
            // Ensure TimeWeight is never negative; if newly initialized, set it to 0
            return Mathf.Max(0, daysSinceLastUsed);
        }

        
        public void RecordUsage(ILanguageUnit unit)
        {
            unit.LastUsed = DateTime.UtcNow;

            // Recalculate TimeWeight to reflect the reset usage
            unit.TimeWeight = CalculateTimeWeight(unit.LastUsed);
        }



        public void RecordUsage(string identifier)
        {
            EnsureInitialized();
            
            if (letterWeights.TryGetValue(identifier,
                    out LetterData languageUnit))
            {
                languageUnit.LastUsed = DateTime.Now;

                languageUnit.Weight = Math.Max(0, languageUnit.TimeWeight - 0.5f); 
            }
            else
            {
                Debug.Log("Identifier not found in letterWeights: " + identifier);
            }
        }

        public void AdjustWeightsBasedOnPerformance(string identifier, bool isCorrect)
        {
            EnsureInitialized();
            
            if (letterWeights.TryGetValue(identifier, out LetterData languageUnit))
            {
                if (isCorrect)
                {
                    languageUnit.TimeWeight += DynamicDifficultyAdjustmentSettings.WeightIncrement;
                }
                else
                {
                    languageUnit.TimeWeight += DynamicDifficultyAdjustmentSettings.WeightDecrement;
                }

                languageUnit.LastUsed = DateTime.Now;
            }
        }

        public Dictionary<char, int> GetCurrentTimeWeights()
        {
            EnsureInitialized();
            
            var currentWeights = new Dictionary<char, int>();

            foreach (var kvp in letterWeights)
            {
                if (char.TryParse(kvp.Key, out char letter))
                {
                    currentWeights[letter] = (int)kvp.Value.TimeWeight;
                }
            }

            return currentWeights;
        }
        
        public void PrintAllWeights()
        {
            EnsureInitialized();
            
            foreach (var unit in letterWeights)
            {
                Debug.Log($"Identifier: {unit.Key}, TimeWeight: {unit.Value.TimeWeight}");
            }
        }
    }
}
