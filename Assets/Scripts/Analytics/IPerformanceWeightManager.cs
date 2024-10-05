using System.Collections.Generic;

namespace Analytics
{
    public interface IPerformanceWeightManager
    {
        /// <summary>
        /// Initializes the weights from PlayerManager's PlayerData, setting a default weight if not already set.
        /// This method should be called after PlayerManager has been initialized.
        /// </summary>
        void InitializeLetterWeights();
        
        /// <summary>
        /// Ensures that the weights are initialized before they are used.
        /// </summary>
        void EnsureInitialized();
        
        IEnumerable<KeyValuePair<string, ILanguageUnit>> GetAllLanguageUnits();
        List<ILanguageUnit> GetNextLetters(LetterCategory category, int count);
        List<ILanguageUnit> GetNextWords(WordLength length, int count);
        void UpdateLetterWeight(string identifier, bool isCorrect);
        void UpdateWordWeight(string identifier, bool isCorrect);
    }
}