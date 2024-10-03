using System.Collections.Generic;

namespace Analytics
{
    public interface IWeightManager
    {
        /// <summary>
        /// Initializes the weights from PlayerManager's PlayerData, setting a default weight if not already set.
        /// This method should be called after PlayerManager has been initialized.
        /// </summary>
        void InitializeWeights();

        /// <summary>
        /// Ensures that the weights are initialized before they are used.
        /// </summary>
        void EnsureInitialized();

        IEnumerable<KeyValuePair<string, ILanguageUnit>> GetAllLanguageUnits();

        // /// <summary>
        // /// Initializes weights for a collection of entities.
        // /// </summary>
        // /// <param name="languageUnits">The collection of entities to initialize weights for.</param>
        // void InitializeWeights(IEnumerable<ILanguageUnit> languageUnits);

        // /// <summary>
        // /// Sets the weight of a specific entity.
        // /// </summary>
        // /// <param name="entity">The entity whose weight is to be set.</param>
        // /// <param name="weight">The weight value to set.</param>
        // void SetEntityWeight(ILanguageUnit entity, int weight);

        /// <summary>
        /// Retrieves the next set of language units based on the specified parameters.
        /// </summary>
        /// <param name="type">The type of language unit to retrieve (e.g., letter, word, sentence).</param>
        /// <param name="category">The category of letter to filter by (e.g., vowel, consonant, all).</param>
        /// <param name="count">The number of language units to retrieve.</param>
        /// <returns>A list of language units that match the specified criteria.</returns>
        // public List<ILanguageUnit> GetNextLanguageUnits(
        //     LanguageUnit type,
        //     LetterCategory category,
        //     int count);
        //
        // public List<ILanguageUnit> GetNextLanguageUnits(
        //     LanguageUnit type,
        //     WordLength length,
        //     int count);

        // public List<ILanguageUnit> GetNextLanguageUnits(
        //     int playerLevel,
        //     int count);

        /// <summary>
        /// Updates the weight of a language unit identified by the given identifier.
        /// </summary>
        /// <param name="identifier">The unique identifier of the language unit to update.</param>
        /// <param name="isCorrect"></param>
        void UpdateWeight(string identifier, bool isCorrect);

        /// <summary>
        /// Updates the weight of a specific entity based on the correctness of an action.
        /// </summary>
        /// <param name="entity">The entity to update weight for.</param>
        /// <param name="isCorrect">Whether the action taken on the entity was correct.</param>
        void UpdateWeight(ILanguageUnit entity, bool isCorrect);

        // /// <summary>
        // /// Gets the current weights for all entities.
        // /// </summary>
        // /// <returns>A dictionary containing entities and their respective weights.</returns>
        // Dictionary<string, int> GetCurrentWeights();
        //
        // /// <summary>
        // /// Gets weights for all vowel entities.
        // /// </summary>
        // /// <returns>A dictionary of vowels and their corresponding weights.</returns>
        // Dictionary<string, int> GetVowelWeights();
        //
        // /// <summary>
        // /// Gets weights for all consonant entities.
        // /// </summary>
        // /// <returns>A dictionary of consonants and their corresponding weights.</returns>
        // Dictionary<string, int> GetConsonantWeights();
        List<ILanguageUnit> GetNextLetters(LetterCategory category, int count);
        List<ILanguageUnit> GetNextWords(WordLength length, int count);
        void UpdateLetterWeight(string identifier, bool isCorrect);
        void UpdateWordWeight(string identifier, bool isCorrect);
    }
}