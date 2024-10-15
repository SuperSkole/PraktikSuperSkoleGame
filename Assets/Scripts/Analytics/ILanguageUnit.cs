using System;

namespace Analytics
{
    /// <summary>
    /// Specifies different units of language that can be analyzed.
    /// </summary>
    public enum LanguageUnit
    {
        Letter,
        Word,
        Sentence
    }

    /// <summary>
    /// Specifies categories of letters for language analysis.
    /// </summary>
    public enum LetterCategory
    {
        Vowel,
        Consonant,
        All
    }

    /// <summary>
    /// Specifies different categories of word lengths.
    /// </summary>
    public enum WordLength
    {
        Unknown,
        TwoLetters,
        ThreeLetters,
        FourLetters,
    }

    /// <summary>
    /// Specifies different difficulty levels for words.
    /// </summary>
    public enum WordDifficulty
    {
        Easy,
        Hard,
        Unknown
    }

    /// <summary>
    /// Represents a unit of language, which can be a letter, word, or sentence.
    /// </summary>
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