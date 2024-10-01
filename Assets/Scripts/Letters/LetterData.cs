using System;
using Analytics;

namespace Letters
{
    public class LetterData : ILanguageUnit
    {
        public string Identifier { get; private set; }
        public LetterCategory Category { get; private set; }
        public LanguageUnit LanguageUnitType => LanguageUnit.Letter;
        public float Weight { get; set; }
        public float TimeWeight { get; set; }
        public DateTime LastUsed { get; set; }
        public int ErrorCount { get; set; }

        public LetterData(string identifier, LetterCategory category, float weight)
        {
            Identifier = identifier;
            Category = category;
            Weight = weight;
            LastUsed = DateTime.MinValue;
        }
    }
}