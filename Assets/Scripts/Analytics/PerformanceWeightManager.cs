using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CORE;
using Letters;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;
using Words;
using Random = UnityEngine.Random;

namespace Analytics
{
    public class PerformanceWeightManager : PersistentSingleton<PerformanceWeightManager>, IPerformanceWeightManager
    {
        private ConcurrentDictionary<string, LetterData> letterWeights;
        private ConcurrentDictionary<string, WordData> wordWeights;

        /// <summary>
        /// Ensures that letterWeights is initialized before use.
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

        public IEnumerable<KeyValuePair<string, ILanguageUnit>> GetAllLanguageUnits()
        {
            EnsureInitialized();
            
            foreach (var kvp in letterWeights)
            {
                yield return new KeyValuePair<string, ILanguageUnit>(kvp.Key, kvp.Value);
            }
            
            foreach (var kvp in wordWeights)
            {
                yield return new KeyValuePair<string, ILanguageUnit>(kvp.Key, kvp.Value);
            }

            // TODO When sentences are implemented
            // foreach (var kvp in sentenceWeights)
            // {
            //     yield return new KeyValuePair<string, ILanguageUnit>(kvp.Key, kvp.Value);
            // }
        }


        /// <summary>
        /// Initializes the weights from PlayerManager's PlayerData, setting a default weight if not already set.
        /// This method should be called after PlayerManager has been initialized.
        /// </summary>
        public void InitializeLetterWeights()
        {
            // Link letterWeights to PlayerManager's PlayerData
            letterWeights = PlayerManager.Instance.PlayerData.LettersWeights;

            // Initialize all letters from letter repository with default weight if they are not already present
            var allLetters = LetterRepository.GetAllLetters()
                .OrderBy(letter => letter.ToString().ToUpper()) // Sort the letters in alphabetical order (case insensitive)
                .ToList();

            // Get sets of vowels and consonants from the repository
            var vowels = new HashSet<char>(LetterRepository.GetVowels()); 
            var consonants = new HashSet<char>(LetterRepository.GetConsonants()); 

            foreach (var letter in allLetters)
            {
                string upperIdentifier = letter.ToString().ToUpper();
                string lowerIdentifier = letter.ToString().ToLower();
        
                // Determine the category for the current letter
                LetterCategory category;
                if (vowels.Contains(char.ToUpper(letter)))
                {
                    category = LetterCategory.Vowel;
                }
                else if (consonants.Contains(char.ToUpper(letter)))
                {
                    category = LetterCategory.Consonant;
                }
                else
                {
                    // Fallback in case it's not classified
                    category = LetterCategory.All; 
                }

                // Add uppercase version of the letter if not already present
                if (!letterWeights.ContainsKey(upperIdentifier))
                {
                    var upperLetterData = new LetterData(upperIdentifier,
                        category, DynamicDifficultyAdjustmentSettings.InitialWeight);
                    letterWeights.TryAdd(upperIdentifier, upperLetterData);
                    //Debug.Log($"Added uppercase letter: {upperIdentifier} with category '{category}' and initial weight {DynamicDifficultyAdjustmentSettings.InitialWeight}");
                }

                // Add lowercase version of the letter if not already present
                if (!letterWeights.ContainsKey(lowerIdentifier))
                {
                    var lowerLetterData = new LetterData(lowerIdentifier,
                        category, DynamicDifficultyAdjustmentSettings.InitialWeight);
                    letterWeights.TryAdd(lowerIdentifier, lowerLetterData);
                    // Debug.Log($"Added lowercase letter: {lowerIdentifier} with category '{category}' and initial weight {DynamicDifficultyAdjustmentSettings.InitialWeight}");
                }
    
                //     // if we dont want upper and lower
                //     string identifier = letter.ToString().ToUpper();
                //
                //     if (!letterWeights.ContainsKey(identifier))
                //     {
                //         var letterData = new LetterData(identifier, LetterCategory.All, InitialWeight);
                //         letterWeights.TryAdd(identifier, letterData);
                //      }
            }
        }

        /// <summary>
        /// Initializes the wordWeights dictionary using player data.
        /// Adds all words from the WordRepository with a default weight if they are not already present.
        /// </summary>
        public void InitializeWordWeights()
        {
            // Initialize wordWeights from PlayerData
            wordWeights = PlayerManager.Instance.PlayerData.WordWeights;

            // Initialize all words from WordRepository with default weight if not already present
            var allWords = WordRepository.GetAllWords();

            foreach (var wordData in allWords)
            {
                string identifier = wordData.Identifier;

                if (!wordWeights.ContainsKey(identifier))
                {
                    wordWeights.TryAdd(identifier, wordData);
                    //Debug.Log($"Added word: {identifier}, Length: {wordData.Length}, Initial weight: {wordData.Weight}");
                }
            }
        }

        /// <summary>
        /// Sets the weight of a specific entity.
        /// </summary>
        public void SetEntityWeight(string identifier, int weight)
        {
            EnsureInitialized();
            
            // Check if the identifier is for a letter
            if (letterWeights.TryGetValue(identifier, out var letterData))
            {
                letterData.Weight = weight;
                letterData.CompositeWeight = weight;
                Debug.Log("Set weight for letter: " + identifier + " to " + weight);
            }
            // Check if the identifier is for a word
            else if (wordWeights.TryGetValue(identifier, out var wordData))
            {
                wordData.Weight = weight;
                wordData.CompositeWeight = weight;
                Debug.Log("Set weight for word: " + identifier + " to " + weight);
            }
            else
            {
                Debug.LogWarning($"Identifier '{identifier}' not found in either letterWeights or wordWeights.");
            }
        }
        
        public List<ILanguageUnit> GetNextLetters(LetterCategory category, int count)
        {
            EnsureInitialized();
            
            // Debug: Check if letterWeights is initialized and populated
            Debug.Log($"Total letters in letterWeights: {letterWeights.Count}");
    
            if (letterWeights.Count == 0)
            {
                Debug.LogError("letterWeights is empty after initialization.");
            }

            // Filter units for letters based on the category (vowel, consonant, etc.)
            var filteredUnits = (category == LetterCategory.All)
                ? letterWeights.Values
                    .Cast<
                        ILanguageUnit>() // Include all letters when 'All' is specified
                : letterWeights.Values
                    .Where(letter =>
                        letter.Category == category);
            
            List<ILanguageUnit> filteredList = filteredUnits.ToList();              

            if (filteredList.Count == 0)
            {
                Debug.LogWarning($"No units found for category {category}. Ensure initialization is done correctly.");
            }

            return GetHighestWeightedUnitsInRandomOrder(filteredList, count);
        }

        public List<ILanguageUnit> GetNextWords(WordLength length, int count)
        {
            EnsureInitialized();
            
            // Debug: Check if wordWeights is initialized and populated
            Debug.Log($"Total letters in wordWeights: {wordWeights.Count}");
    
            if (wordWeights.Count == 0)
            {
                Debug.LogError("wordWeights is empty after initialization.");
            }

            // Filter units for words based on the word length (2 letters, 3 letters, etc.)
            var filteredUnits = wordWeights.Values
                .Where(unit => unit.Length == length)
                .Cast<ILanguageUnit>() 
                .ToList();
            
            if (filteredUnits.Count == 0)
            {
                Debug.LogWarning($"No units found for category {length}. Ensure initialization is done correctly.");
            }

            return GetHighestWeightedUnitsInRandomOrder(filteredUnits, count);
        }

        private List<ILanguageUnit> GetHighestWeightedUnitsInRandomOrder(List<ILanguageUnit> units, int count)
        {
            // Sort units by composite weight in descending order, if weights are equal, randomize the order of the equal weight units
            var sortedUnits = units
                .GroupBy(unit => unit.CompositeWeight)
                .OrderByDescending(group => group.Key) // Sort by weight (highest first)
                .SelectMany(group => group.OrderBy(_ => Random.value)) // Randomize within each group of the same weight
                .ToList();

            // return the chosen amount of units
            return sortedUnits.Take(count).ToList();
        }

        
        public List<ILanguageUnit> GetNextLanguageUnitsByTypeAndCategory(LanguageUnit type, LetterCategory category, int count)
        {
            EnsureInitialized();
    
            Debug.Log($"GetNextLanguageUnits called with type: {type}, category: {category}, count: {count}");

            // filter units by type
            var filteredUnits = letterWeights.Values
                .Where(u => u.LanguageUnitType == type);

            Debug.Log($"Filtered units count after type filter: {filteredUnits.Count()}");

            if (type == LanguageUnit.Letter)
            {
                IEnumerable<string> validIdentifiers = category switch
                {
                    LetterCategory.Vowel => LetterRepository.GetVowels().Select(c => c.ToString().ToUpper()),
                    LetterCategory.Consonant => LetterRepository.GetConsonants().Select(c => c.ToString().ToUpper()),
                    _ => letterWeights.Keys
                };

                filteredUnits = filteredUnits.Where(u => validIdentifiers.Contains(u.Identifier));
                Debug.Log($"Filtered units count after category filter: {filteredUnits.Count()}");
            }

            var unitList = filteredUnits.ToList();
            if (!unitList.Any())
            {
                Debug.LogWarning($"No units found for type {type} and category {category}.");
                return new List<ILanguageUnit>();
            }

            // sort units by weight, then order randomly within each weight group
            var sortedUnits = unitList
                .GroupBy(u => u.CompositeWeight)
                .OrderByDescending(g => g.Key)
                .SelectMany(g => g.OrderBy(u => Random.value))
                .Cast<ILanguageUnit>()
                .ToList();

            Debug.Log($"Total sorted units count: {sortedUnits.Count}");

            // return the asked for count of units
            var result = sortedUnits.Take(count).ToList();

            Debug.Log($"Returning {result.Count} units.");
            foreach (var unit in result)
            {
                Debug.Log($"Unit: {unit.Identifier}, Weight: {unit.Weight}");
            }

            return result;
        }
        
        /// <summary>
        /// Updates the weight of a letter based on whether the action was correct.
        /// </summary>
        public void UpdateLetterWeight(string identifier, bool isCorrect)
        {
            EnsureInitialized();

            if (letterWeights.TryGetValue(identifier, out LetterData currentUnit))
            {
                currentUnit.Weight = isCorrect
                    ? Mathf.Max(currentUnit.Weight - DynamicDifficultyAdjustmentSettings.WeightDecrement, DynamicDifficultyAdjustmentSettings.MinWeight)
                    : Mathf.Min(currentUnit.Weight + DynamicDifficultyAdjustmentSettings.WeightIncrement, DynamicDifficultyAdjustmentSettings.MaxWeight);

                Debug.Log($"Updated weight for letter '{identifier}' to {currentUnit.Weight}");
            }
            else
            {
                Debug.LogWarning($"Letter '{identifier}' not found for weight update.");
            }
        }

        /// <summary>
        /// Updates the weight of a word based on whether the action was correct.
        /// </summary>
        /// <summary>
        /// Updates the weight of a word and also adjusts the weights of individual letters within the word.
        /// </summary>
        public void UpdateWordWeight(string identifier, bool isCorrect)
        {
            EnsureInitialized();

            if (wordWeights.TryGetValue(identifier, out WordData currentWord))
            {
                // Adjust the weight of the entire word
                currentWord.Weight = isCorrect
                    ? Mathf.Max(currentWord.Weight - DynamicDifficultyAdjustmentSettings.WeightDecrement, DynamicDifficultyAdjustmentSettings.MinWeight)
                    : Mathf.Min(currentWord.Weight + DynamicDifficultyAdjustmentSettings.WeightIncrement, DynamicDifficultyAdjustmentSettings.MaxWeight);

                // // Update LastUsed to the current time
                // currentWord.LastUsed = DateTime.UtcNow;

                // Update individual letter weights and LastUsed based on the word's performance
                foreach (char letter in identifier)
                {
                    string letterIdentifier = letter.ToString();
                    if (letterWeights.TryGetValue(letterIdentifier, out LetterData letterData))
                    {
                        letterData.Weight = isCorrect
                            ? Mathf.Max(letterData.Weight - DynamicDifficultyAdjustmentSettings.WeightDecrement, DynamicDifficultyAdjustmentSettings.MinWeight)
                            : Mathf.Min(letterData.Weight + DynamicDifficultyAdjustmentSettings.WeightIncrement, DynamicDifficultyAdjustmentSettings.MaxWeight);

                        // // Update LastUsed for the letter
                        // letterData.LastUsed = DateTime.UtcNow;
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Word '{identifier}' not found for weight update.");
            }
        }

        // TODO use when we have sentences
        // /// <summary>
        // /// Updates the weight of a sentence and also adjusts the weights of individual words and letters within the sentence.
        // /// </summary>
        // public void UpdateSentenceWeight(string sentenceIdentifier, bool isCorrect)
        // {
        //     EnsureInitialized();
        //
        //     if (sentenceWeights.TryGetValue(sentenceIdentifier, out SentenceData currentSentence))
        //     {
        //         // Adjust the weight of the entire sentence
        //         currentSentence.Weight = isCorrect
        //             ? Mathf.Max(currentSentence.Weight - DynamicDifficultyAdjustmentSettings.WeightDecrement, DynamicDifficultyAdjustmentSettings.MinWeight)
        //             : Mathf.Min(currentSentence.Weight + DynamicDifficultyAdjustmentSettings.WeightIncrement, DynamicDifficultyAdjustmentSettings.MaxWeight);
        //
        //         Debug.Log($"Updated weight for sentence '{sentenceIdentifier}' to {currentSentence.Weight}");
        //
        //         // Update individual word weights based on the sentence's performance
        //         foreach (var word in currentSentence.Words)
        //         {
        //             if (wordWeights.TryGetValue(word, out WordData wordData))
        //             {
        //                 // Update word weight
        //                 wordData.Weight = isCorrect
        //                     ? Mathf.Max(wordData.Weight - DynamicDifficultyAdjustmentSettings.WeightDecrement, DynamicDifficultyAdjustmentSettings.MinWeight)
        //                     : Mathf.Min(wordData.Weight + DynamicDifficultyAdjustmentSettings.WeightIncrement, DynamicDifficultyAdjustmentSettings.MaxWeight);
        //
        //                 Debug.Log($"Updated weight for word '{word}' to {wordData.Weight}");
        //
        //                 // Update individual letter weights based on the word's performance
        //                 foreach (char letter in word)
        //                 {
        //                     string letterIdentifier = letter.ToString().ToUpper();
        //                     if (letterWeights.TryGetValue(letterIdentifier, out LetterData letterData))
        //                     {
        //                         // Update letter weight
        //                         letterData.Weight = isCorrect
        //                             ? Mathf.Max(letterData.Weight - DynamicDifficultyAdjustmentSettings.WeightDecrement, DynamicDifficultyAdjustmentSettings.MinWeight)
        //                             : Mathf.Min(letterData.Weight + DynamicDifficultyAdjustmentSettings.WeightIncrement, DynamicDifficultyAdjustmentSettings.MaxWeight);
        //
        //                         Debug.Log($"Updated weight for letter '{letterIdentifier}' to {letterData.Weight}");
        //                     }
        //                     else
        //                     {
        //                         Debug.LogWarning($"Letter '{letterIdentifier}' not found for weight update.");
        //                     }
        //                 }
        //             }
        //             else
        //             {
        //                 Debug.LogWarning($"Word '{word}' not found for weight update.");
        //             }
        //         }
        //     }
        //     else
        //     {
        //         Debug.LogWarning($"Sentence '{sentenceIdentifier}' not found for weight update.");
        //     }
        // }

        public void PrintAllWeights()
        {
            EnsureInitialized();
            
            foreach (var unit in letterWeights)
            {
                Debug.Log($"Identifier: {unit.Key}, Weight: {unit.Value.Weight}");
            }
        }
    }
}