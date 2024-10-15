using System.Collections.Generic;

namespace Analytics
{
    /// <summary>
    /// Interface for managing dynamic difficulty adjustment (DDA).
    /// Combines input from performance and spaced repetition to determine composite weights.
    /// </summary>
    public interface IDynamicDifficultyAdjustmentManager
    {
        List<ILanguageUnit> GetNextLanguageUnitsBasedOnLevel(int count);
        void UpdateLanguageUnitWeight(string identifier, bool isCorrect);
    }
}