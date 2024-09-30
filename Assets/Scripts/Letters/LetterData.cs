using System;
using Analytics;

namespace Letters
{
    public class LetterData : IEntity
    {
        public char Letter { get; private set; }
        public string Identifier => Letter.ToString(); // Convert char to string for unified interface
        public int Weight { get; set; }
        public DateTime LastUsed { get; set; }
        public int ErrorCount { get; set; }
        public bool IsVowel { get; private set; }

        public LetterData(char letter, bool isVowel, int initialWeight)
        {
            Letter = letter;
            IsVowel = isVowel;
            Weight = initialWeight; 
            LastUsed = DateTime.MinValue; 
            ErrorCount = 0; // No errors recorded initially
        }
    }
}