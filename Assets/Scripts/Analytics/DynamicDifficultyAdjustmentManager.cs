using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CORE;
using Letters;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

namespace Analytics
{
    public class PlayerLevelMapper
    {
        private static readonly Dictionary<int, (LanguageUnit, object)> LevelMapping = new Dictionary<int, (LanguageUnit, object)>
        {
            { 0, (LanguageUnit.Letter, LetterCategory.Vowel) },         // Level 0: Vowels
            { 1, (LanguageUnit.Letter, LetterCategory.Consonant) },     // Level 1: Consonants
            { 2, (LanguageUnit.Letter, LetterCategory.All) },           // Level 2: All Letters
            { 3, (LanguageUnit.Word, WordLength.TwoLetters) },          // Level 3: Two-Letter Words
            { 4, (LanguageUnit.Word, WordLength.ThreeLetters) },        // Level 4: Three-Letter Words
            { 5, (LanguageUnit.Word, WordLength.FourLetters) },         // Level 5: Four-Letter Words
            { 6, (LanguageUnit.Sentence, 5) },                          // Level 6: Sentences
        };

        /// <summary>
        /// Gets the corresponding content type for a given player level.
        /// </summary>
        /// <param name="playerLevel">The player's level.</param>
        /// <returns>A tuple containing the LanguageUnit type and its specific category or length.</returns>
        public static (LanguageUnit, object) GetContentTypeForLevel(int playerLevel)
        {
            if (LevelMapping.TryGetValue(playerLevel, out var contentType))
            {
                return contentType;
            }

            throw new ArgumentException($"Invalid player level: {playerLevel}");
        }
    }
    
    public class DynamicDifficultyAdjustmentManager : PersistentSingleton<DynamicDifficultyAdjustmentManager>, IDynamicDifficultyAdjustmentManager
    {
        private Dictionary<LanguageUnit, Func<object, int, List<ILanguageUnit>>> unitHandlers;
        
        private readonly IWeightManager weightManager;
        private readonly ISpacedRepetitionManager spacedRepetitionManager;
        private Dictionary<string, float> compositeWeights;
        private ConcurrentDictionary<string, LetterData> letterWeights;

        private const float PerformanceFactor = 1.0f; // Currently prioritizing only performance
        // private const float PerformanceFactor = 0.7f;
        private const float TimeFactor = 0.3f;

        public DynamicDifficultyAdjustmentManager()
        {
            weightManager = GameManager.Instance.WeightManager;
            spacedRepetitionManager = GameManager.Instance.SpacedRepetitionManager;
            compositeWeights = new Dictionary<string, float>();
            InitializeLanguageUnitHandlers();
        }
        
        // Constructor with Dependency Injection
        public DynamicDifficultyAdjustmentManager(IWeightManager weightManager, ISpacedRepetitionManager spacedRepetitionManager)
        {
            this.weightManager = weightManager;
            this.spacedRepetitionManager = spacedRepetitionManager;
            compositeWeights = new Dictionary<string, float>();
            InitializeLanguageUnitHandlers();
        }
        
        private void InitializeLanguageUnitHandlers()
        {
            // Initialize handlers for each LanguageUnit type, adding type checks to ensure safety
            unitHandlers = new Dictionary<LanguageUnit, Func<object, int, List<ILanguageUnit>>>
            {
                {
                    LanguageUnit.Letter, (specificType, count) =>
                    {
                        if (specificType is LetterCategory letterCategory)
                        {
                            return GetLetters(letterCategory, count);
                        }

                        throw new ArgumentException($"Expected a LetterCategory but got {specificType.GetType()}");
                    }
                },
                {
                    LanguageUnit.Word, (specificType, count) =>
                    {
                        if (specificType is WordLength wordLength)
                        {
                            return GetWords(wordLength, count);
                        }

                        throw new ArgumentException($"Expected a WordLength but got {specificType.GetType()}");
                    }
                }
            };
        }

        
        private List<ILanguageUnit> GetLetters(LetterCategory category, int count)
        {
            return weightManager.GetNextLetters(category, count);
        }

        private List<ILanguageUnit> GetWords(WordLength length, int count)
        {
            return weightManager.GetNextWords(length, count);
        }


        public List<ILanguageUnit> GetNextLanguageUnitsBasedOnLevel( int count)
        {
            int playerLevel = 0;
            if (PlayerManager.Instance.PlayerData != null)
            {
                playerLevel = PlayerManager.Instance.PlayerData.LanguageLevel;
            }
            
            // Calculate composite weights for each letter based on performance and time data
            CalculateCompositeWeights();

            var (unitType, specificType) = PlayerLevelMapper.GetContentTypeForLevel(playerLevel);

            if (unitHandlers.TryGetValue(unitType, out var handler))
            {
                return handler(specificType, count);
            }

            Debug.LogWarning($"Unsupported content type for player level {playerLevel}.");
            return new List<ILanguageUnit>();
        }


        /// <summary>
        /// Calculates the composite weights for each letter based on both performance and time data.
        /// </summary>
        public void CalculateCompositeWeights()
        {
            // Ensure the weights have been initialized before starting calculations
            weightManager.EnsureInitialized();

            // Iterate over all language units (letters, words, etc.) in the letterWeights collection
            foreach (var kvp in weightManager.GetAllLanguageUnits())
            {
                // Get the current language unit, which could be LetterData, WordData, etc.
                var languageUnit = kvp.Value;

                // Access performance and time weights directly from the language unit
                float performanceWeight = languageUnit.Weight;
                float timeWeight = languageUnit.TimeWeight;

                // Calculate composite weight using the weighted formula
                languageUnit.CompositeWeight = (performanceWeight * PerformanceFactor) + (timeWeight * TimeFactor);

                Debug.Log($"Composite Weight for {languageUnit.Identifier}: {languageUnit.CompositeWeight}");
            }
        }



        /// <summary>
        /// Gets a random entity based on the given weights using a weighted random selection algorithm.
        /// </summary>
        /// <param name="weights">A dictionary containing entities and their corresponding weights.</param>
        /// <returns>The selected entity based on weight.</returns>
        private char GetRandomEntityBasedOnWeight(Dictionary<string, float> weights)
        {
            float totalWeight = weights.Values.Sum();
            float randomValue = UnityEngine.Random.Range(0, totalWeight);

            float cumulativeWeight = 0;
            foreach (var kvp in weights)
            {
                cumulativeWeight += kvp.Value;
                if (randomValue <= cumulativeWeight)
                {
                    return kvp.Key[0]; // Return the first character of the identifier (assuming it’s a letter)
                }
            }

            // In case something goes wrong, return a default value (shouldn't typically reach this point)
            return weights.Keys.First()[0];
        }
        
        public void EnsureInitialized()
        {
            if (letterWeights == null)
            {
                letterWeights = PlayerManager.Instance.PlayerData.LettersWeightsProperty;
            }
        }
        
        public void PrintAllWeights()
        {
            EnsureInitialized();
            
            foreach (var unit in letterWeights)
            {
                Debug.Log($"Identifier: {unit.Key}, TimeWeight: {unit.Value.CompositeWeight}");
            }
        }
    }
}
