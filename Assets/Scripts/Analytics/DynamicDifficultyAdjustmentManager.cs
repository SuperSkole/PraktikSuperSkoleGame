using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CORE;
using Letters;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;
using Words;

namespace Analytics
{
    public static class PlayerLevelMapper
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
        private Dictionary<LanguageUnit, Action<ILanguageUnit, bool>> updateWeightHandlers;
        private Dictionary<LanguageUnit, Action<ILanguageUnit>> timeWeightHandlers;
        
        private readonly IWeightManager weightManager;
        private readonly ISpacedRepetitionManager spacedRepetitionManager;
        private Dictionary<string, float> compositeWeights;
        private ConcurrentDictionary<string, LetterData> letterWeights;
        private ConcurrentDictionary<string, WordData> wordWeights;

        private const float PerformanceFactor = 1.0f; // Currently prioritizing only performance
        // private const float PerformanceFactor = 0.7f;
        private const float TimeFactor = 0.3f;

        private int PlayerLanguageLevel;

        public DynamicDifficultyAdjustmentManager()
        {
            weightManager = GameManager.Instance.WeightManager;
            spacedRepetitionManager = GameManager.Instance.SpacedRepetitionManager;
            compositeWeights = new Dictionary<string, float>();
            //InitializeLanguageUnitHandlers();
            InitializeUpdateWeightHandlers();
            InitializeTimeWeightHandlers();
        }
        
        // Constructor with Dependency Injection
        public DynamicDifficultyAdjustmentManager(IWeightManager weightManager, ISpacedRepetitionManager spacedRepetitionManager)
        {
            this.weightManager = weightManager;
            this.spacedRepetitionManager = spacedRepetitionManager;
            compositeWeights = new Dictionary<string, float>();
            //InitializeLanguageUnitHandlers();
            InitializeUpdateWeightHandlers();
            InitializeTimeWeightHandlers();
        }
        
        public List<ILanguageUnit> GetNextLanguageUnitsBasedOnLevel(int count)
        {
            if (PlayerManager.Instance.PlayerData != null)
            {
                PlayerLanguageLevel = PlayerManager.Instance.PlayerData.PlayerLanguageLevel;
            }
            
            // Calculate composite weights for each letter based on performance and time data
            CalculateCompositeWeights();

            var (unitType, specificType) = PlayerLevelMapper.GetContentTypeForLevel(PlayerLanguageLevel);

            if (unitHandlers.TryGetValue(unitType, out var handler))
            {
                return handler(specificType, count);
            }

            Debug.LogWarning($"Unsupported content type for player level {PlayerLanguageLevel}.");
            return new List<ILanguageUnit>();
        }
        
        public void UpdateLanguageUnitWeight(string identifier, bool isCorrect)
        {
            // Ensure composite weights are calculated before updating
            CalculateCompositeWeights();

            // Find the ILanguageUnit based on the identifier (either letter, word, or sentence)
            var unit = GetLanguageUnitByIdentifier(identifier);
            if (unit != null)
            {
                // Use the specific handler for updating the weight
                if (updateWeightHandlers.TryGetValue(unit.LanguageUnitType, out var updateHandler))
                {
                    updateHandler(unit, isCorrect);
                }

                // Update time weight based on usage
                if (timeWeightHandlers.TryGetValue(unit.LanguageUnitType, out var timeHandler))
                {
                    timeHandler(unit);
                }

                Debug.Log($"Updated weight for {identifier} based on performance: {isCorrect}");
            }
            else
            {
                Debug.LogWarning($"No language unit found for identifier '{identifier}'.");
            }
        }
        
        public ILanguageUnit GetLanguageUnitByIdentifier(string identifier)
        {
            EnsureInitialized();

            if (letterWeights.TryGetValue(identifier, out var letterData))
            {
                return letterData;
            }
            if (wordWeights.TryGetValue(identifier, out var wordData))
            {
                return wordData;
            }
            // If you have sentences later, add similar lookup logic here

            Debug.LogWarning($"Identifier '{identifier}' not found in WeightManager.");
            return null;
        }


        
        /// <summary>
        /// Updates the weight of a language unit based on the player's performance.
        /// </summary>
        public void UpdateLanguageUnitWeight(ILanguageUnit unit, bool isCorrect)
        {
            // Calculate composite weights to ensure data is up-to-date before updating
            CalculateCompositeWeights();

            // Update weight based on performance
            if (updateWeightHandlers.TryGetValue(unit.LanguageUnitType, out var updateHandler))
            {
                updateHandler(unit, isCorrect);
            }

            // Update time weight based on usage
            if (timeWeightHandlers.TryGetValue(unit.LanguageUnitType, out var timeHandler))
            {
                timeHandler(unit);
            }

            Debug.Log($"Updated weight for {unit.Identifier} based on performance and time: {isCorrect}");
        }
        
        /// <summary>
        /// Calculates the composite weights for each letter based on both performance and time data.
        /// </summary>
        private void CalculateCompositeWeights()
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

        private void InitializeTimeWeightHandlers()
        {
            timeWeightHandlers = new Dictionary<LanguageUnit, Action<ILanguageUnit>>
            {
                {
                    LanguageUnit.Letter, (unit) =>
                    {
                        spacedRepetitionManager.RecordUsage(unit.Identifier);
                        Debug.Log($"Updated time weight for letter '{unit.Identifier}'");
                    }
                },
                {
                    LanguageUnit.Word, (unit) =>
                    {
                        spacedRepetitionManager.RecordUsage(unit.Identifier);
                        Debug.Log($"Updated time weight for word '{unit.Identifier}'");

                        // Optionally update time weight for individual letters within the word as well
                        foreach (char letter in unit.Identifier)
                        {
                            string letterIdentifier = letter.ToString();
                            spacedRepetitionManager.RecordUsage(letterIdentifier);
                            Debug.Log($"Updated time weight for letter '{letterIdentifier}' within word '{unit.Identifier}'");
                        }
                    }
                },
                {
                    LanguageUnit.Sentence, (unit) =>
                    {
                        spacedRepetitionManager.RecordUsage(unit.Identifier);
                        Debug.Log($"Updated time weight for sentence '{unit.Identifier}'");

                        // Optionally update time weight for words and letters within the sentence
                        var sentenceWords = unit.Identifier.Split(' ');
                        foreach (var word in sentenceWords)
                        {
                            spacedRepetitionManager.RecordUsage(word);
                            Debug.Log($"Updated time weight for word '{word}' within sentence '{unit.Identifier}'");

                            foreach (char letter in word)
                            {
                                string letterIdentifier = letter.ToString();
                                spacedRepetitionManager.RecordUsage(letterIdentifier);
                                Debug.Log($"Updated time weight for letter '{letterIdentifier}' within word '{word}'");
                            }
                        }
                    }
                }
            };
        }
        
        private void InitializeUpdateWeightHandlers()
        {
            updateWeightHandlers = new Dictionary<LanguageUnit, Action<ILanguageUnit, bool>>
            {
                {
                    LanguageUnit.Letter, (unit, isCorrect) =>
                    {
                        weightManager.UpdateLetterWeight(unit.Identifier, isCorrect);
                        spacedRepetitionManager.RecordUsage(unit.Identifier);
                    }
                },
                {
                    LanguageUnit.Word, (unit, isCorrect) =>
                    {
                        weightManager.UpdateWordWeight(unit.Identifier, isCorrect);
                        spacedRepetitionManager.RecordUsage(unit.Identifier);
                    }
                }
                // TODO: Add support for Sentence
                // ,
                // {
                //     LanguageUnit.Sentence, (unit, isCorrect) =>
                //     {
                //         weightManager.UpdateSentenceWeight(unit.Identifier, isCorrect);
                //         spacedRepetitionManager.RecordUsage(unit.Identifier);
                //     }
                // }
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
        
        




        


        



        // /// <summary>
        // /// Gets a random entity based on the given weights using a weighted random selection algorithm.
        // /// </summary>
        // /// <param name="weights">A dictionary containing entities and their corresponding weights.</param>
        // /// <returns>The selected entity based on weight.</returns>
        // private char GetRandomEntityBasedOnWeight(Dictionary<string, float> weights)
        // {
        //     float totalWeight = weights.Values.Sum();
        //     float randomValue = UnityEngine.Random.Range(0, totalWeight);
        //
        //     float cumulativeWeight = 0;
        //     foreach (var kvp in weights)
        //     {
        //         cumulativeWeight += kvp.Value;
        //         if (randomValue <= cumulativeWeight)
        //         {
        //             return kvp.Key[0]; // Return the first character of the identifier (assuming it’s a letter)
        //         }
        //     }
        //
        //     // In case something goes wrong, return a default value (shouldn't typically reach this point)
        //     return weights.Keys.First()[0];
        // }
        
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