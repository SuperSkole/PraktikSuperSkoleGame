using System;
using Analytics;
using UnityEngine;

namespace Words
{
    public class WordData : ILanguageUnit
    {
        public LanguageUnit LanguageUnitType => LanguageUnit.Word;
        public string Identifier { get; private set; }
        public WordLength Length { get; private set; }
        public float Weight { get; set; }
        public float TimeWeight { get; set; }
        public float CompositeWeight { get; set; }
        public DateTime LastUsed { get; set; }
        public int ErrorCount { get; set; }

        public WordData(string identifier, WordLength length, float weight)
        {
            Identifier = identifier;
            Weight = weight;
            TimeWeight = 0; 
            CompositeWeight = 0f; 
            LastUsed = DateTime.MinValue;
        }
    }

    public static class WordLengthHelper
    {
        /// <summary>
        /// Gets the WordLength enum value that corresponds to the length of the given word.
        /// </summary>
        /// <param name="word">The word whose length is to be determined.</param>
        /// <returns>The corresponding WordLength enum value.</returns>
        public static WordLength GetWordLength(string word)
        {
            int length = word.Length;

            return length switch
            {
                2 => WordLength.TwoLetters,
                3 => WordLength.ThreeLetters,
                4 => WordLength.FourLetters,
                _ => WordLength.Unknown 
                
            };
        }
    }

}