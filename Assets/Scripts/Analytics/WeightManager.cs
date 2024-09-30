using System.Collections.Generic;
using System.Linq;
using CORE;
using Letters;
using UnityEngine;

namespace Analytics
{
    public class WeightManager : PersistentSingleton<WeightManager>, IWeightManager
    {
        private Dictionary<string, int> entityWeights = new Dictionary<string, int>();
        private readonly ILetterRepository letterRepository;
        private const int DefaultWeight = 50;

        public WeightManager(ILetterRepository letterRepository)
        {
            this.letterRepository = letterRepository;
        }

        /// <summary>
        /// Initializes the weights for a given set of entities, setting a default weight if not already set.
        /// </summary>
        public void InitializeWeights(IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (!entityWeights.ContainsKey(entity.Identifier))
                {
                    entityWeights[entity.Identifier] = DefaultWeight;
                }
            }
        }

        /// <summary>
        /// Sets the weight of a specific entity.
        /// </summary>
        public void SetEntityWeight(IEntity entity, int weight)
        {
            if (entityWeights.ContainsKey(entity.Identifier))
            {
                entityWeights[entity.Identifier] = weight;
            }
            else
            {
                Debug.LogWarning($"Entity '{entity.Identifier}' not found in WeightManager.");
            }
        }

        /// <summary>
        /// Updates the weight of an entity based on whether the action was correct.
        /// </summary>
        public void UpdateWeight(IEntity entity, bool isCorrect)
        {
            if (entityWeights.TryGetValue(entity.Identifier, out int currentWeight))
            {
                entityWeights[entity.Identifier] = isCorrect 
                    ? Mathf.Max(currentWeight - 5, 1) 
                    : Mathf.Min(currentWeight + 5, 99);
            }
            else
            {
                Debug.LogWarning($"Entity '{entity.Identifier}' not found for weight update.");
            }
        }

        /// <summary>
        /// Retrieves the current weights for all entities.
        /// </summary>
        public Dictionary<string, int> GetCurrentWeights()
        {
            return new Dictionary<string, int>(entityWeights);
        }

        /// <summary>
        /// Retrieves the weights of all vowel entities.
        /// </summary>
        public Dictionary<string, int> GetVowelWeights()
        {
            var vowels = new HashSet<string>(letterRepository.GetVowels().Select(v => v.ToString()));
            return entityWeights
                .Where(e => vowels.Contains(e.Key))
                .ToDictionary(e => e.Key, e => e.Value);
        }

        /// <summary>
        /// Retrieves the weights of all consonant entities.
        /// </summary>
        public Dictionary<string, int> GetConsonantWeights()
        {
            var consonants = new HashSet<string>(letterRepository.GetConsonants().Select(c => c.ToString()));
            return entityWeights
                .Where(e => consonants.Contains(e.Key))
                .ToDictionary(e => e.Key, e => e.Value);
        }
    }
}
