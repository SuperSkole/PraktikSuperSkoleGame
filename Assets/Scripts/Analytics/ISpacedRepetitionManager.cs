using System.Collections.Generic;

namespace Analytics
{
    public interface ISpacedRepetitionManager
    {
        Dictionary<char, int> GetCurrentWeights();
    }
}