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
    public class WeightManager : PersistentSingleton<WeightManager>, IWeightManager
    {
        private ConcurrentDictionary<string, LetterData> letterWeights;
        private ConcurrentDictionary<string, WordData> wordWeights;


        public WeightManager(ILetterRepository letterRepository)
        {
           
        }
        
        public WeightManager()
        {
           
        }
        
        /// <summary>
        /// Ensures that letterWeights is initialized before use.
        /// </summary>
        public void EnsureInitialized()
        {
            if (letterWeights == null)
            {
                letterWeights = PlayerManager.Instance.PlayerData.LettersWeightsProperty;
            }
        }

        public IEnumerable<KeyValuePair<string, ILanguageUnit>> GetAllLanguageUnits() { throw new NotImplementedException(); }

        /// <summary>
        /// Initializes the weights from PlayerManager's PlayerData, setting a default weight if not already set.
        /// This method should be called after PlayerManager has been initialized.
        /// </summary>
        public void InitializeWeights()
        {
            // Link letterWeights to PlayerManager's PlayerData
            letterWeights = PlayerManager.Instance.PlayerData.LettersWeights;

            // Initialize all letters from letter repository with default weight if they are not already present
            var allLetters = LetterRepository.GetAllLetters();

            foreach (var letter in allLetters)
            {
                string upperIdentifier = letter.ToString().ToUpper();
                string lowerIdentifier = letter.ToString().ToLower();

                if (!letterWeights.ContainsKey(upperIdentifier))
                {
                    var upperLetterData = new LetterData(upperIdentifier,
                        LetterCategory.All, DynamicDifficultyAdjustmentSettings.InitialWeight);
                    letterWeights.TryAdd(upperIdentifier, upperLetterData);
                    Debug.Log(
                        $"Added uppercase letter: {upperIdentifier} with initial weight {DynamicDifficultyAdjustmentSettings.InitialWeight}");
                }

                if (!letterWeights.ContainsKey(lowerIdentifier))
                {
                    var lowerLetterData = new LetterData(lowerIdentifier,
                        LetterCategory.All, DynamicDifficultyAdjustmentSettings.InitialWeight);
                    letterWeights.TryAdd(lowerIdentifier, lowerLetterData);
                    Debug.Log(
                        $"Added lowercase letter: {lowerIdentifier} with initial weight {DynamicDifficultyAdjustmentSettings.InitialWeight}");
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
            
            // Link WordWeights to PlayerData's WordWeights
            wordWeights = PlayerManager.Instance.PlayerData.WordWeights;

            // TODO - Initialize words with default weight if not present
            
            // // Initialize words with default weight if not present 
            // foreach (var word in wordRepository.GetAllWords())  
            // {
            //     if (!wordWeights.ContainsKey(word))
            //     {
            //         var wordData = new WordData(word, WeightSettings.InitialWeight);
            //         wordWeights.TryAdd(word, wordData);
            //         Debug.Log($"Added word: {word} with initial weight {WeightSettings.InitialWeight}");
            //     }
            // }
        }
        
        // public IEnumerable<KeyValuePair<string, ILanguageUnit>> GetAllLanguageUnits()
        // {
        //     EnsureInitialized();
        //     return ILanguageUnit;
        // }


        /// <summary>
        /// Sets the weight of a specific entity.
        /// </summary>
        public void SetEntityWeight(ILanguageUnit entity, int weight)
        {
            EnsureInitialized();
            
            if (letterWeights.ContainsKey(entity.Identifier))
            {
                letterWeights[entity.Identifier].Weight = weight;
            }
            else
            {
                Debug.LogWarning($"Entity '{entity.Identifier}' not found in WeightManager.");
            }
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

                Debug.Log($"Updated weight for word '{identifier}' to {currentWord.Weight}");

                // Update individual letter weights based on the word's performance
                foreach (char letter in identifier)
                {
                    string letterIdentifier = letter.ToString(); //we do not toupper, so we can check capital and not letters
                    if (letterWeights.TryGetValue(letterIdentifier, out LetterData letterData))
                    {
                        letterData.Weight = isCorrect
                            ? Mathf.Max(letterData.Weight - DynamicDifficultyAdjustmentSettings.WeightDecrement, DynamicDifficultyAdjustmentSettings.MinWeight)
                            : Mathf.Min(letterData.Weight + DynamicDifficultyAdjustmentSettings.WeightIncrement, DynamicDifficultyAdjustmentSettings.MaxWeight);

                        Debug.Log($"Updated weight for letter '{letterIdentifier}' to {letterData.Weight}");
                    }
                    else
                    {
                        Debug.LogWarning($"Letter '{letterIdentifier}' not found for weight update.");
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


        /// <summary>
        /// Updates the weight of an entity based on whether the action was correct.
        /// </summary>
        public void UpdateWeight(ILanguageUnit entity, bool isCorrect)
        {
            EnsureInitialized();
            
            if (letterWeights.TryGetValue(entity.Identifier, out LetterData currentUnit))
            {
                currentUnit.Weight = isCorrect
                    ? Mathf.Max(currentUnit.Weight - DynamicDifficultyAdjustmentSettings.WeightDecrement, DynamicDifficultyAdjustmentSettings.MinWeight)
                    : Mathf.Min(currentUnit.Weight + DynamicDifficultyAdjustmentSettings.WeightIncrement, DynamicDifficultyAdjustmentSettings.MaxWeight);
            }
            else
            {
                Debug.LogWarning($"Entity '{entity.Identifier}' not found for weight update.");
            }
        }
        
        public void UpdateWeight(string identifier, bool isCorrect)
        {
            EnsureInitialized();
        
            if (letterWeights.TryGetValue(identifier, out LetterData currentUnit))
            {
                currentUnit.Weight = isCorrect
                    ? Mathf.Max(currentUnit.Weight - DynamicDifficultyAdjustmentSettings.WeightDecrement, DynamicDifficultyAdjustmentSettings.MinWeight)
                    : Mathf.Min(currentUnit.Weight + DynamicDifficultyAdjustmentSettings.WeightIncrement, DynamicDifficultyAdjustmentSettings.MaxWeight);
            }
            else
            {
                Debug.LogWarning($"Identifier {identifier} not found in letter weights.");
            }
        }
        
        public List<ILanguageUnit> GetNextLetters(LetterCategory category, int count)
        {
            EnsureInitialized();

            // Filter units for letters based on the category (vowel, consonant, etc.)
            var filteredUnits = letterWeights.Values
                .Where(unit => unit.Category == category)
                .Cast<ILanguageUnit>() 
                .ToList();

            return GetHighestWeightedUnits(filteredUnits, count);
        }

        public List<ILanguageUnit> GetNextWords(WordLength length, int count)
        {
            EnsureInitialized();

            // Filter units for words based on the word length (2 letters, 3 letters, etc.)
            var filteredUnits = wordWeights.Values
                .Where(unit => unit.Length == length)
                .Cast<ILanguageUnit>() 
                .ToList();

            return GetHighestWeightedUnits(filteredUnits, count);
        }





        
        private bool MatchesSpecificType(ILanguageUnit unit, object specificType)
        {
            // Determine type of specificType and match accordingly
            return specificType switch
            {
                LetterCategory letterCategory when unit is LetterData letterData => letterData.Category == letterCategory,
                WordLength wordLength when unit is WordData wordData => wordData.Length == wordLength,
                _ => false
            };
        }

        private List<ILanguageUnit> GetHighestWeightedUnits(List<ILanguageUnit> units, int count)
        {
            // Sort units by composite weight in descending order
            var sortedUnits = units
                .GroupBy(unit => unit.CompositeWeight)
                .OrderByDescending(group => group.Key) // Sort by weight (highest first)
                .SelectMany(group => group.OrderBy(_ => UnityEngine.Random.value)) // Randomize within each group of the same weight
                .ToList();

            // Take the top 'count' elements
            return sortedUnits.Take(count).ToList();
        }

        
        public List<ILanguageUnit> GetNextLanguageUnits(LanguageUnit type, LetterCategory category, int count)
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
                .GroupBy(u => u.Weight)
                .OrderByDescending(g => g.Key)
                .SelectMany(g => g.OrderBy(u => UnityEngine.Random.value))
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
        
        public List<ILanguageUnit> GetNextLanguageUnits(LanguageUnit type, int count)
        {
            EnsureInitialized();
    
            // Get all elements of the requested type
            var filteredUnits = letterWeights.Values
                .Where(u => u.LanguageUnitType == type)
                .ToList();

            // Sort and take the requested count
            var sortedUnits = filteredUnits
                .GroupBy(u => u.Weight)
                .OrderByDescending(g => g.Key)
                .SelectMany(g => g.OrderBy(u => UnityEngine.Random.value))
                .Take(count)
                .Cast<ILanguageUnit>()
                .ToList();

            return sortedUnits;
        }



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