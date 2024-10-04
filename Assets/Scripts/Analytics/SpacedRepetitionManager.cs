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

        
        
        // public void InitializeTimeWeights(int playerLevel)
        // {
        //     switch (playerLevel)
        //     {
        //         case 0:
        //             // Initialize only vowels at level 0
        //             foreach (var vowel in LetterRepository.GetVowels())
        //             {
        //                 if (!letterWeights.ContainsKey(vowel.ToString()))   
        //                 {
        //                     letterWeights[vowel.ToString()] = new LetterData
        //                     {
        //                         Identifier = vowel,
        //                         LastUsed = DateTime.Now,
        //                         TimeWeight = 0
        //                     };
        //                 }
        //             }
        //             break;
        //
        //         case 1:
        //             // Initialize consonants at level 1
        //             foreach (var consonant in letterRepository.GetConsonants())
        //             {
        //                 if (!letterWeights.ContainsKey(consonant))
        //                 {
        //                     letterWeights[consonant] = new LetterData
        //                     {
        //                         Identifier = consonant,
        //                         LastUsed = DateTime.Now,
        //                         TimeWeight = 0
        //                     };
        //                 }
        //             }
        //             break;
        //
        //         case 3:
        //             // Initialize words for level 3 and above
        //             foreach (var word in wordRepository.GetWordsByLength(2))
        //             {
        //                 if (!wordWeights.ContainsKey(word))
        //                 {
        //                     wordWeights[word] = new WordData
        //                     {
        //                         Identifier = word,
        //                         LastUsed = DateTime.Now,
        //                         TimeWeight = 0
        //                     };
        //                 }
        //             }
        //             break;
        //
        //         // Add further levels and initializations as needed
        //     }
        //
        //     Debug.Log($"Initialized time weights for level {playerLevel}");
        // }

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


        public void RecordUsage(string unitIdentifier) { throw new NotImplementedException(); }

        public void RecordUsage(string identifier, bool isCorrect)
        {
            EnsureInitialized();

            if (letterWeights.TryGetValue(identifier, out var letterData))
            {
                UpdateTimeWeight(letterData, isCorrect);
            }
            // else if (wordWeights.TryGetValue(identifier, out var wordData))
            // {
            //     UpdateTimeWeight(wordData, isCorrect);
            // }

            Debug.Log($"Recorded usage for '{identifier}', success: {isCorrect}");
        }

        private void UpdateTimeWeight(ILanguageUnit unit, bool isCorrect)
        {
            // Basic time weight calculation inspired by Anki's repetition model
            // TODO - Implement a more sophisticated algorithm for spaced repetition
            TimeSpan elapsedTime = DateTime.Now - unit.LastUsed;

            if (isCorrect)
            {
                // Increase the interval between repetitions
                unit.TimeWeight = Math.Max(unit.TimeWeight - (int)elapsedTime.TotalDays, 0); // Lower weight indicates less priority
            }
            else
            {
                // Decrease the interval (weight increases) for more frequent practice
                unit.TimeWeight += (int)elapsedTime.TotalDays * 2; // Incorrect responses have double impact
            }

            // Update the last used date
            unit.LastUsed = DateTime.Now;

            Debug.Log($"Updated time weight for '{unit.Identifier}', new time weight: {unit.TimeWeight}");
        }


        // public void AdjustWeightsBasedOnPerformance(string identifier, bool isCorrect)
        // {
        //     EnsureInitialized();
        //     
        //     if (letterWeights.TryGetValue(identifier, out LetterData languageUnit))
        //     {
        //         if (isCorrect)
        //         {
        //             languageUnit.TimeWeight += DynamicDifficultyAdjustmentSettings.WeightIncrement;
        //         }
        //         else
        //         {
        //             languageUnit.TimeWeight += DynamicDifficultyAdjustmentSettings.WeightDecrement;
        //         }
        //
        //         languageUnit.LastUsed = DateTime.Now;
        //     }
        // }

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
