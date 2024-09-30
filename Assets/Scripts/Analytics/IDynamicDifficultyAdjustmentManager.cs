namespace Analytics
{
    /// <summary>
    /// Interface for managing dynamic difficulty adjustment (DDA).
    /// Combines input from performance and spaced repetition to determine composite weights.
    /// </summary>
    public interface IDynamicDifficultyAdjustmentManager
    {
        /// <summary>
        /// Retrieves a weighted vowel based on the composite weight calculation.
        /// </summary>
        /// <returns>The selected vowel character.</returns>
        char GetWeightedVowel();

        /// <summary>
        /// Retrieves a weighted consonant based on the composite weight calculation.
        /// </summary>
        /// <returns>The selected consonant character.</returns>
        char GetWeightedConsonant();

        /// <summary>
        /// Retrieves a weighted letter based on the composite weight calculation.
        /// </summary>
        /// <returns>The selected letter character.</returns>
        char GetWeightedLetter();

        /// <summary>
        /// Calculates the composite weight for all letters based on performance and time data.
        /// </summary>
        void CalculateCompositeWeights();

        /// <summary>
        /// Gets the next letter based on the composite weight calculation.
        /// </summary>
        /// <returns>The next letter to present to the user.</returns>
        //char GetNextLetter();
    }
}