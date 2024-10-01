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
        TwoLetters,
        ThreeLetters,
        FourLetters,
    }
    
    public interface ILanguageUnit
    {
        string Identifier { get; } // what letter,word sentence eg. "A", "Cat", "A Black cat "
        LanguageUnit LanguageUnitType { get; }
        float Weight { get; set; }  
        float TimeWeight { get; set; }  
        DateTime LastUsed { get; set; }
    }
}