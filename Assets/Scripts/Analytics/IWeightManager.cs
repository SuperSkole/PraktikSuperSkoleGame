using System.Collections.Generic;

namespace CORE.Scripts
{
    public interface IWeightManager
    {
        void InitializeWeights(IEnumerable<char> letters);
        Dictionary<char, int> GetCurrentWeights();
        void UpdateWeight(char letter, bool isCorrect);
    }
}

