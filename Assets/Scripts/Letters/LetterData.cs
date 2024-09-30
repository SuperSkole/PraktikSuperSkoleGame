using System;

namespace CORE.Scripts
{
    public class LetterData
    {
        public char Letter { get; set; }
        public int Weight { get; set; }
        public DateTime LastUsed { get; set; }
        public int ErrorCount { get; set; }
        public bool IsVowel { get; set; }
    }
}