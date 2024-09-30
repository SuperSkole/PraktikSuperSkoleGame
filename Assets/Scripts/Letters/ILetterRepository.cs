using System.Collections.Generic;

namespace Letters
{
    public interface ILetterRepository
    {
        IEnumerable<char> GetAllLetters();
        IEnumerable<char> GetVowels();
        IEnumerable<char> GetConsonants();
    }
}

