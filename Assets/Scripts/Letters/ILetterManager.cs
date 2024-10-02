using System.Collections.Generic;

namespace CORE.Scripts
{
    public interface ILetterManager
    {
        // Get Randoms
        char GetRandomVowel();
        char GetRandomConsonant();
        char GetRandomLetter();
        
        // Get weighted randoms
        char GetWeightedVowel();
        char GetWeightedConsonant();
        char GetWeightedLetter();
        
        // adjust and work with weighted letters
        void InitializeWeights();
        void SetLetterWeight(char letter, int weight);
        void UpdateLetterWeight(char letter, bool isCorrect);
        int GetLetterWeight(char letter);
    }
}