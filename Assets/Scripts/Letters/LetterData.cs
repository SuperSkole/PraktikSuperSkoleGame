using System;
using Analytics;

namespace Letters
{
    /// <summary>
    /// Represents a data structure for storing information about a letter,
    /// including its identifier, category, weight, and usage statistics.
    /// </summary>
    public class LetterData : ILanguageUnit
    {
        public LanguageUnit LanguageUnitType => LanguageUnit.Letter;
        public string Identifier { get; private set; }
        public LetterCategory Category { get; private set; }
        public LetterCategory ErrorCategory { get; private set; }
        
        public float Weight { get; set; }
        public float TimeWeight { get; set; }
        public float CompositeWeight { get; set; } 
        public DateTime LastUsed { get; set; }
        public int ErrorCount { get; set; }

        /// <summary>
        /// Represents data associated with a letter, including its identifier, category,
        /// weight, time weight, composite weight, last usage time, and error count.
        /// </summary>
        public LetterData(
            string identifier,
            LetterCategory category,
            float weight)
        {
            Identifier = identifier;
            Category = category;
            Weight = weight;
            TimeWeight = 0;        
            CompositeWeight = 0; 
            LastUsed = DateTime.UtcNow;

            ErrorCategory = Category switch
            {
                LetterCategory.Vowel => LetterCategory.Consonant,
                LetterCategory.Consonant => LetterCategory.Vowel,
                _ => ErrorCategory
            };
        }
    }
}