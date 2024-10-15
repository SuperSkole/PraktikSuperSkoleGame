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
    /// <summary>
    /// Provides mapping of player levels to a list of language unit types and associated categories.
    /// </summary>
    public static class PlayerLevelMapper
    {
        /// <summary>
        /// A static dictionary mapping player levels to their respective types of language units
        /// and additional parameters. Each key is an integer representing the player's level,
        /// and the corresponding value is a list of tuples. Each tuple consists of a
        /// LanguageUnit enum value and an associated object, which provides additional
        /// context or parameters relevant for that language unit.
        /// </summary>
        public static readonly Dictionary<int, List<(LanguageUnit, object)>> LevelMapping = new Dictionary<int, List<(LanguageUnit, object)>>
        {
            {
                0, new List<(LanguageUnit, object)>
                {
                    (LanguageUnit.Letter, LetterCategory.Vowel)
                }
            },
            {
                1, new List<(LanguageUnit, object)>
                {
                    (LanguageUnit.Letter, LetterCategory.Consonant)
                }
            },
            {
                2, new List<(LanguageUnit, object)>
                {
                    (LanguageUnit.Letter, LetterCategory.All)
                }
            },
            {
                3, new List<(LanguageUnit, object)> 
                {
                    (LanguageUnit.Word, WordLength.TwoLetters),
                    (LanguageUnit.Letter, LetterCategory.All) 
                } 
            },
            {
                4, new List<(LanguageUnit, object)> 
                {
                    (LanguageUnit.Word, WordLength.TwoLetters),
                    (LanguageUnit.Letter, LetterCategory.All),
                    (LanguageUnit.Word, WordLength.ThreeLetters),
                }
            },
            {
                5, new List<(LanguageUnit, object)> 
                {
                    (LanguageUnit.Word, WordLength.TwoLetters),
                    (LanguageUnit.Letter, LetterCategory.All),
                    (LanguageUnit.Word, WordLength.ThreeLetters),
                    (LanguageUnit.Word, WordLength.FourLetters),
                }
            },
            {
                6, new List<(LanguageUnit, object)>
                {
                    (LanguageUnit.Word, WordLength.TwoLetters),
                    (LanguageUnit.Letter, LetterCategory.All),
                    (LanguageUnit.Word, WordLength.ThreeLetters),
                    (LanguageUnit.Word, WordLength.FourLetters),
                    (LanguageUnit.Sentence, null)
                }
            }
        };

        /// <summary>
        /// Retrieves a list of content types associated with a specific player level.
        /// </summary>
        /// <param name="playerLevel">The level of the player for which content types need to be retrieved.</param>
        /// <returns>A list of tuples where each tuple contains a LanguageUnit and an associated object representing the content type for the specified player level.</returns>
        /// <exception cref="ArgumentException">Thrown when the provided player level is not valid.</exception>
        public static List<(LanguageUnit, object)> GetContentTypesForLevel(
            int playerLevel)
        {
            if (LevelMapping.TryGetValue(playerLevel, out var contentTypes))
            {
                return contentTypes;
            }

            throw new ArgumentException($"Invalid player level: {playerLevel}");
        }
    }

    /// <summary>
    /// Manages dynamic difficulty adjustment by evaluating player performance and adjusting
    /// the difficulty of language units accordingly. This includes managing composite weights
    /// and spaced repetition to ensure a balanced and adaptive learning experience.
    /// </summary>
    public class DynamicDifficultyAdjustmentManager : PersistentSingleton<DynamicDifficultyAdjustmentManager>, IDynamicDifficultyAdjustmentManager
    {
        private Dictionary<LanguageUnit, Func<object, int, List<ILanguageUnit>>> unitHandlers;
        private Dictionary<LanguageUnit, Action<ILanguageUnit, bool>> updateWeightHandlers;
        private Dictionary<LanguageUnit, Action<ILanguageUnit>> timeWeightHandlers;
        
        private Dictionary<string, float> compositeWeights;
        private ConcurrentDictionary<string, LetterData> letterWeights;
        private ConcurrentDictionary<string, WordData> wordWeights;
        
        public int playerLanguageLevel;
        //private const float PerformanceFactor = 1.0f; // Currently prioritizing only performance
        private const float PerformanceFactor = 0.7f;
        private const float TimeFactor = 0.3f;
        
        private readonly IPerformanceWeightManager performanceWeightManager;
        private readonly ISpacedRepetitionManager spacedRepetitionManager;


        public DynamicDifficultyAdjustmentManager()
        {
            performanceWeightManager = GameManager.Instance.PerformanceWeightManager;
            spacedRepetitionManager = GameManager.Instance.SpacedRepetitionManager;
            compositeWeights = new Dictionary<string, float>();
            
            // Initialize handlers for language units
            InitializeLanguageUnitHandlers();
            InitializeUpdateWeightHandlers();
            InitializeTimeWeightHandlers();
        }
        
        // Constructor with Dependency Injection for unit testing
        public DynamicDifficultyAdjustmentManager(IPerformanceWeightManager performanceWeightManager, ISpacedRepetitionManager spacedRepetitionManager)
        {
            this.performanceWeightManager = performanceWeightManager;
            this.spacedRepetitionManager = spacedRepetitionManager;
            compositeWeights = new Dictionary<string, float>();
            
            // Initialize handlers for language units
            InitializeLanguageUnitHandlers();
            InitializeUpdateWeightHandlers();
            InitializeTimeWeightHandlers();
        }

        /// <summary>
        /// Call this to get the next set of language units based on the player's current level.
        /// </summary>
        /// <param name="count">The number of language units to retrieve.</param>
        /// <returns>A list of language units appropriate for the player's level.</returns>
        public List<ILanguageUnit> GetNextLanguageUnitsBasedOnLevel(int count)
        {
            if (PlayerManager.Instance.PlayerData != null)
            {
                playerLanguageLevel = PlayerManager.Instance.PlayerData.PlayerLanguageLevel;
                //playerLanguageLevel = 3; //test level
                if (playerLanguageLevel != 5) CheckAndUpdatePlayerLevel();
            }

            // Calculate composite weights for each language unit
            CalculateCompositeWeights();

            // Get the content types for the player's level
            var contentTypes = PlayerLevelMapper.GetContentTypesForLevel(playerLanguageLevel);
            var allUnits = new List<ILanguageUnit>();

            // Fetch units for each content type
            int numberOfUnitsToFetch = 10;
            foreach (var (unitType, specificType) in contentTypes)
            {
                // Use the specific handler for the content type
                if (unitHandlers.TryGetValue(unitType, out var handler))
                {
                    // Fetch x units for the content type
                    var units = handler(specificType, numberOfUnitsToFetch);
                    allUnits.AddRange(units);
                }
            }

            // Remove duplicates based on Identifier
            var uniqueUnits = allUnits
                .GroupBy(u => u.Identifier)
                .Select(g => g.First())
                .ToList();

            // Sort the combined list by CompositeWeight
            uniqueUnits = uniqueUnits
                .OrderByDescending(u => u.CompositeWeight)
                .ToList();

            // Take the top 'count' units
            var selectedUnits = uniqueUnits.Take(count).ToList();

            return selectedUnits;
        }

        /// <summary>
        /// Call this to Update the weight of a language unit based on the players performance.
        /// </summary>
        /// <param name="identifier">The unique identifier for the language unit. e.g. "A", "Cat", "A Black cat "</param>
        /// <param name="isCorrect">Indicates whether the performance on the unit was correct.</param>
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

        /// <summary>
        /// Retrieves a language unit based on the provided identifier.
        /// </summary>
        /// <param name="identifier">The identifier of the language unit to be retrieved.</param>
        /// <returns>The language unit that matches the given identifier or null if no match is found.</returns
        private ILanguageUnit GetLanguageUnitByIdentifier(string identifier)
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
            // TODO sentence

            Debug.LogWarning($"Identifier '{identifier}' not found in WeightManager.");
            return null;
        }

        /// <summary>
        /// Checks the current player's language level and updates it if necessary based on their performance and predefined level mappings.
        /// </summary>
        /// <remarks>
        /// This method ensures that the player's language level is up-to-date with their current performance metrics.
        /// It retrieves relevant language units for the player's current level, calculates the average weight of these units,
        /// and determines whether the player should be leveled up.
        /// </remarks>
        /// <exception cref="KeyNotFoundException">Thrown when no mapping is found for the player's current level.</exception>
        public void CheckAndUpdatePlayerLevel()
        {
            EnsureInitialized();

            int currentLevel = PlayerManager.Instance.PlayerData.PlayerLanguageLevel;

            // Use LevelMapping to determine the relevant content types and parameters for the current level
            if (PlayerLevelMapper.LevelMapping.TryGetValue(currentLevel, out var contentTypes))
            {
                List<ILanguageUnit> allRelevantUnits = new List<ILanguageUnit>();

                // Get the relevant language units for each content type
                foreach (var (contentType, additionalParameter) in contentTypes)
                {
                    List<ILanguageUnit> relevantUnits = GetUnitsByContentType((contentType, additionalParameter));
                    allRelevantUnits.AddRange(relevantUnits);
                }

                if (allRelevantUnits.Count == 0)
                {
                    Debug.LogWarning($"No relevant units found for level {currentLevel}.");
                    return;
                }

                // Remove duplicates if any
                var uniqueUnits = allRelevantUnits.GroupBy(u => u.Identifier).Select(g => g.First()).ToList();

                // Calculate the average weight for the relevant units
                float averageWeight = CalculateAverageWeight(uniqueUnits);

                // Determine if the player has performed well enough to level up
                if (averageWeight <= DynamicDifficultyAdjustmentSettings.LevelUpThreshold)
                {
                    // Update the player's language level and initialize time weights for the new level
                    Debug.Log("Player has leveled up!");
                    PlayerManager.Instance.PlayerData.PlayerLanguageLevel++;
                    InitializeTimeWeights(PlayerManager.Instance.PlayerData.PlayerLanguageLevel); 
                    Debug.Log($"Player has leveled up to level {PlayerManager.Instance.PlayerData.PlayerLanguageLevel} with average weight: {averageWeight}");
                }
                else
                {
                    Debug.Log($"Player remains at level {currentLevel} with average weight: {averageWeight}");
                }
            }
            else
            {
                Debug.LogWarning($"No mapping found for player level {currentLevel}.");
            }
        }

        /// <summary>
        /// Initializes the time weights for the language units associated with the specified player level.
        /// This involves retrieving relevant content types and their corresponding units, removing duplicates, and setting initial time weights based on their usage history.
        /// </summary>
        /// <param name="playerLevel">The current level of the player, used to fetch relevant content types and their associated units.</param>
        /// <exception cref="KeyNotFoundException">Thrown when no content types are found for the provided player level.</exception>
        private void InitializeTimeWeights(int playerLevel)
        {
            // Get the content types and additional parameters for the current level from LevelMapping
            if (PlayerLevelMapper.LevelMapping.TryGetValue(playerLevel, out var contentTypes))
            {
                List<ILanguageUnit> allRelevantUnits = new List<ILanguageUnit>();

                // Get the relevant language units for each content type
                foreach (var (contentType, additionalParameter) in contentTypes)
                {
                    List<ILanguageUnit> relevantUnits = GetUnitsByContentType((contentType, additionalParameter));
                    allRelevantUnits.AddRange(relevantUnits);
                }

                // Remove duplicates if any
                var uniqueUnits = allRelevantUnits.GroupBy(u => u.Identifier).Select(g => g.First()).ToList();

                // Initialize time weights for the relevant units
                foreach (var unit in uniqueUnits)
                {
                    if (unit.LastUsed == DateTime.MinValue)
                    {
                        unit.LastUsed = DateTime.Now; // Set initial time
                        unit.TimeWeight = 0; // Set initial time weight
                    }
                }
            }
            else
            {
                Debug.LogWarning($"No mapping found for player level {playerLevel}.");
            }
        }

        /// <summary>
        /// Retrieves a list of language units based on a specified content type and additional parameter.
        /// </summary>
        /// <param name="contentType">A tuple containing the LanguageUnit type and an associated object representing additional parameters to filter the language units.</param>
        /// <returns>A list of language units matching the specified content type and additional filtering parameters.</returns>
        private List<ILanguageUnit> GetUnitsByContentType(
            (LanguageUnit, object) contentType)
        {
            var (unitType, parameter) = contentType;

            return unitType switch
            {
                LanguageUnit.Letter when parameter is LetterCategory category =>
                    category == LetterCategory.All
                        ? letterWeights.Values.Cast<ILanguageUnit>().ToList()
                        : letterWeights.Values
                            .Where(letter => letter.Category == category)
                            .Cast<ILanguageUnit>()
                            .ToList(),

                LanguageUnit.Word when parameter is WordLength wordLength => wordWeights.Values
                    .Where(word => word.Length == wordLength)
                    .Cast<ILanguageUnit>()
                    .ToList(),

                _ => new List<ILanguageUnit>()
            };
        }

        /// <summary>
        /// Calculates the average weight of a given list of language units.
        /// </summary>
        /// <param name="units">A list of ILanguageUnit instances whose average weight needs to be calculated.</param>
        /// <returns>The average weight of the provided language units. If the list is empty or null, returns <c>float.MaxValue</c> to prevent level up.</returns>
        private float CalculateAverageWeight(List<ILanguageUnit> units)
        {
            if (units == null || units.Count == 0)
            {
                return float.MaxValue; // No units to calculate, return maximum to prevent level up
            }

            float totalWeight = units.Sum(unit => unit.Weight);
            return totalWeight / units.Count;
        }

        
        /// <summary>
        /// Calculates the composite weights for each letter based on both performance and time data.
        /// </summary>
        private void CalculateCompositeWeights()
        {
            // Ensure the weights have been initialized before starting calculations
            performanceWeightManager.EnsureInitialized();

            // Iterate over all language units (letters, words, etc.) in the letterWeights collection
            foreach (var kvp in performanceWeightManager.GetAllLanguageUnits())
            {
                // Get the current language unit, which could be LetterData, WordData, etc.
                var languageUnit = kvp.Value;

                // Access performance and time weights directly from the language unit
                float performanceWeight = languageUnit.Weight;
                float timeWeight = languageUnit.TimeWeight;

                // Calculate composite weight using the weighted formula
                languageUnit.CompositeWeight = (performanceWeight * PerformanceFactor) + (timeWeight * TimeFactor);

                //Debug.Log($"Composite Weight for {languageUnit.Identifier}: {languageUnit.CompositeWeight}");
            }
            
            //PrintAllWeights();
        }

        /// <summary>
        /// Initializes the handlers for different types of language units.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown when the specific type provided to a handler does not match the expected type for the language unit.
        /// </exception>
        private void InitializeLanguageUnitHandlers()
        {
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

        /// <summary>
        /// Initializes the handlers responsible for managing time-based weights of language units.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if a language unit type is not handled properly.</exception>
        private void InitializeTimeWeightHandlers()
        {
            timeWeightHandlers = new Dictionary<LanguageUnit, Action<ILanguageUnit>>
            {
                {
                    LanguageUnit.Letter, (unit) =>
                    {
                        spacedRepetitionManager.UpdateLastUsedAndTimeWeight(unit.Identifier);
                        Debug.Log($"Updated time weight for letter '{unit.Identifier}'");
                    }
                },
                {
                    LanguageUnit.Word, (unit) =>
                    {
                        spacedRepetitionManager.UpdateLastUsedAndTimeWeight(unit.Identifier);
                        //Debug.Log($"Updated time weight for word '{unit.Identifier}'");

                        // TODO Optionally update time weight for individual letters within the word as well
                        foreach (char letter in unit.Identifier)
                        {
                            string letterIdentifier = letter.ToString();
                            spacedRepetitionManager.UpdateLastUsedAndTimeWeight(letterIdentifier);
                            Debug.Log($"Updated time weight for letter '{letterIdentifier}' within word '{unit.Identifier}'");
                        }
                    }
                },
                {
                    LanguageUnit.Sentence, (unit) =>
                    {
                        spacedRepetitionManager.UpdateLastUsedAndTimeWeight(unit.Identifier);
                        //Debug.Log($"Updated time weight for sentence '{unit.Identifier}'");

                        // TODO Optionally update time weight for words and letters within the sentence
                        var sentenceWords = unit.Identifier.Split(' ');
                        foreach (var word in sentenceWords)
                        {
                            spacedRepetitionManager.UpdateLastUsedAndTimeWeight(word);
                            Debug.Log($"Updated time weight for word '{word}' within sentence '{unit.Identifier}'");

                            foreach (char letter in word)
                            {
                                string letterIdentifier = letter.ToString();
                                spacedRepetitionManager.UpdateLastUsedAndTimeWeight(letterIdentifier);
                                Debug.Log($"Updated time weight for letter '{letterIdentifier}' within word '{word}'");
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Initializes the dictionary of handlers for updating the weights of various language units.
        /// </summary>
        private void InitializeUpdateWeightHandlers()
        {
            updateWeightHandlers = new Dictionary<LanguageUnit, Action<ILanguageUnit, bool>>
            {
                {
                    LanguageUnit.Letter, (unit, isCorrect) =>
                    {
                        performanceWeightManager.UpdateLetterWeight(unit.Identifier, isCorrect);
                        //spacedRepetitionManager.UpdateLastUsedAndTimeWeight(unit.Identifier);
                    }
                },
                {
                    LanguageUnit.Word, (unit, isCorrect) =>
                    {
                        performanceWeightManager.UpdateWordWeight(unit.Identifier, isCorrect);
                        //spacedRepetitionManager.UpdateLastUsedAndTimeWeight(unit.Identifier);
                    }
                }
                // TODO: Add support for Sentence
                // ,
                // {
                //     LanguageUnit.Sentence, (unit, isCorrect) =>
                //     {
                //         weightManager.UpdateSentenceWeight(unit.Identifier, isCorrect);
                //         //spacedRepetitionManager.RecordUsage(unit.Identifier);
                //     }
                // }
            };
        }

        /// <summary>
        /// Retrieves a list of language units of type letters from a specified category.
        /// </summary>
        /// <param name="category">The category of letters to be retrieved (e.g., Vowel, Consonant, All).</param>
        /// <param name="count">The number of letter language units to retrieve.</param>
        /// <returns>A list of language units that match the specified category and count.</returns>
        private List<ILanguageUnit> GetLetters(
            LetterCategory category,
            int count)
        {
            return performanceWeightManager.GetNextLetters(category, count);
        }

        /// <summary>
        /// Retrieves a list of words based on the specified word length and count.
        /// </summary>
        /// <param name="length">The length of the words to be retrieved.</param>
        /// <param name="count">The number of words to be retrieved.</param>
        /// <returns>A list of ILanguageUnit instances representing the words of the specified length.</returns>
        private List<ILanguageUnit> GetWords(WordLength length, int count)
        {
            return performanceWeightManager.GetNextWords(length, count);
        }

        /// <summary>
        /// Ensures that the weight dictionaries for letters and words are initialized
        /// by loading data from the PlayerManager's PlayerData. If they are not already
        /// initialized, this method populates them with the appropriate data.
        /// </summary>
        public void EnsureInitialized()
        {
            if (letterWeights == null)
            {
                letterWeights = PlayerManager.Instance.PlayerData.LettersWeightsProperty;
            }
            
            if (wordWeights == null)
            {
                wordWeights = PlayerManager.Instance.PlayerData.WordWeightsProperty;
            }
            
            // TODO Sentence weights
            // if (sentenceWeights == null)
            // {
            //     sentenceWeights = PlayerManager.Instance.PlayerData.sentenceWeights;
            // }
        }
        
        /// <summary>
        /// debug tool
        /// Prints all letter weights to the debug log.
        /// This method ensures that the weights are initialized before printing.
        /// </summary>
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