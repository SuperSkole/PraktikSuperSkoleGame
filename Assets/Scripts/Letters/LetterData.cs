using System;
using Analytics;

namespace Letters
{
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

        public LetterData(string identifier, LetterCategory category, float weight)
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