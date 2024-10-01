using System.Collections.Generic;
using Analytics;

namespace Letters
{
    public interface ILetterRepository
    {
        IEnumerable<char> GetAllLetters();
        IEnumerable<char> GetVowels();
        IEnumerable<char> GetConsonants();
    }
}

