using System.Collections.Generic;
using System.Linq;
using CORE;
using Letters;

namespace Analytics
{
    public class DynamicDifficultyAdjustmentManager : IDynamicDifficultyAdjustmentManager
    {
        private readonly IWeightManager weightManager;
        private readonly ISpacedRepetitionManager spacedRepetitionManager;
        private Dictionary<string, float> compositeWeights;

        private const float PerformanceFactor = 1.0f; // Currently prioritizing only performance
        // private const float PerformanceFactor = 0.7f;
        private const float TimeFactor = 0.3f;

        // Constructor with Dependency Injection
        public DynamicDifficultyAdjustmentManager(IWeightManager weightManager, ISpacedRepetitionManager spacedRepetitionManager)
        {
            this.weightManager = GameManager.Instance.WeightManager;
            this.spacedRepetitionManager = GameManager.Instance.SpacedRepetitionManager;
            compositeWeights = new Dictionary<string, float>();
        }

        /// <summary>
        /// Gets a weighted letter (vowel or consonant) based on composite weight calculations.
        /// </summary>
        public char GetWeightedLetter()
        {
            CalculateCompositeWeights();
            return GetRandomEntityBasedOnWeight(compositeWeights);
        }

        /// <summary>
        /// Calculates the composite weights for each letter based on both performance and time data.
        /// </summary>
        public void CalculateCompositeWeights()
        {
            // var performanceWeights = weightManager.GetCurrentWeights();
            // // var timeWeights = spacedRepetitionManager.GetCurrentWeights();
            //
            // compositeWeights.Clear();
            //
            // foreach (var letterIdentifier in performanceWeights.Keys)
            // {
            //     var performanceWeight = performanceWeights[letterIdentifier];
            //     // var timeWeight = timeWeights.ContainsKey(letterIdentifier) ? timeWeights[letterIdentifier] : 0;
            //
            //     // Calculate composite weight using a weighted formula
            //     // float compositeWeight = (performanceWeight * PerformanceFactor) + (timeWeight * TimeFactor);
            //     float compositeWeight = performanceWeight * PerformanceFactor;
            //
            //     compositeWeights[letterIdentifier] = compositeWeight;
            // }
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
    }
}
