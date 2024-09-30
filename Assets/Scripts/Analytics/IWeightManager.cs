using System.Collections.Generic;

namespace Analytics
{
    public interface IWeightManager
    {
        /// <summary>
        /// Initializes weights for a collection of entities.
        /// </summary>
        /// <param name="entities">The collection of entities to initialize weights for.</param>
        void InitializeWeights(IEnumerable<IEntity> entities);

        /// <summary>
        /// Sets the weight of a specific entity.
        /// </summary>
        /// <param name="entity">The entity whose weight is to be set.</param>
        /// <param name="weight">The weight value to set.</param>
        void SetEntityWeight(IEntity entity, int weight);

        /// <summary>
        /// Updates the weight of a specific entity based on the correctness of an action.
        /// </summary>
        /// <param name="entity">The entity to update weight for.</param>
        /// <param name="isCorrect">Whether the action taken on the entity was correct.</param>
        void UpdateWeight(IEntity entity, bool isCorrect);

        /// <summary>
        /// Gets the current weights for all entities.
        /// </summary>
        /// <returns>A dictionary containing entities and their respective weights.</returns>
        Dictionary<string, int> GetCurrentWeights();

        /// <summary>
        /// Gets weights for all vowel entities.
        /// </summary>
        /// <returns>A dictionary of vowels and their corresponding weights.</returns>
        Dictionary<string, int> GetVowelWeights();

        /// <summary>
        /// Gets weights for all consonant entities.
        /// </summary>
        /// <returns>A dictionary of consonants and their corresponding weights.</returns>
        Dictionary<string, int> GetConsonantWeights();
    }
}