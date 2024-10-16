using System;
using Analytics;
using UnityEngine;

namespace Words
{
    /// <summary>
    /// Represents a word data entity which includes metadata
    /// such as length, weights, last used time, and error count.
    /// Implements the ILanguageUnit interface.
    /// </summary>
    public class WordData : ILanguageUnit
    {
        public LanguageUnit LanguageUnitType => LanguageUnit.Word;
        public string Identifier { get; private set; }
        public WordLength Length { get; private set; }
        // public WordDifficulty Difficulty { get; private set; }
        public float Weight { get; set; }
        public float TimeWeight { get; set; }
        public float CompositeWeight { get; set; }
        public DateTime LastUsed { get; set; }
        public int ErrorCount { get; set; }

        /// <summary>
        /// Represents a word data entity which includes metadata
        /// such as length, weights, last used time, and error count.
        /// Implements the ILanguageUnit interface.
        /// </summary>
        public WordData(string identifier, WordLength length, float weight)
        {
            Identifier = identifier;
            Length = length;
            Weight = weight;
            TimeWeight = 0; 
            CompositeWeight = 0; 
            LastUsed = DateTime.UtcNow;
        }
    }
}