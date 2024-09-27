using System.Collections.Generic;

namespace CORE.Scripts
{
    public interface ILetterManager
    {
        char GetRandomLetter();
        char GetRandomVowel();
        char GetRandomConsonant();
        char GetWeightedLetter();
        void UpdateLetterWeight(char letter, bool isCorrect);
    }
}