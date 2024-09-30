using System.Collections.Generic;

namespace Letters
{
    public interface IRandomLetterSelector
    {
        // Get Randoms
        char GetRandomVowel(IEnumerable<char> vowels);
        char GetRandomConsonant(IEnumerable<char> consonants);
        char GetRandomLetter(IEnumerable<char> letters);
        //char GetWeightedRandomLetter(Dictionary<char, float> weightedLetters);
    }
}

