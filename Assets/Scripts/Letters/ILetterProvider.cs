using System.Collections.Generic;

namespace CORE.Scripts
{
    public interface ILetterProvider
    {
        IEnumerable<char> GetAllLetters();
        IEnumerable<char> GetVowels();
        IEnumerable<char> GetConsonants();
    }
}

