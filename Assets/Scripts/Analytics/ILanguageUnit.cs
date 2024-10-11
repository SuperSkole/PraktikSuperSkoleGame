using System;

namespace Analytics
{
    public enum LanguageUnit 
    {
        Letter,
        Word,
        Sentence
    }

    public enum LetterCategory
    {
        Vowel,
        Consonant,
        All
    }

    public enum WordLength
    {
        Unknown,
        TwoLetters,
        ThreeLetters,
        FourLetters,
    }
    
    public enum WordDifficulty
    {
        Easy,
        Hard,
        Unknown
    }
    
    public interface ILanguageUnit
    {
        string Identifier { get; } // what letter,word or sentence e.g. "A", "Cat", "A Black cat "
        LanguageUnit LanguageUnitType { get; }
        float Weight { get; set; }  
        float TimeWeight { get; set; }  
        public float CompositeWeight { get; set; }
        DateTime LastUsed { get; set; }
        public int ErrorCount { get; set; }
    }
}