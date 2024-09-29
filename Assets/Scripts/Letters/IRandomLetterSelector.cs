using System.Collections.Generic;

namespace CORE.Scripts
{
    public interface IRandomLetterSelector
    {
        char GetRandomLetter(IEnumerable<char> letters);
        char GetWeightedRandomLetter(Dictionary<char, int> weightedLetters);
    }
}

