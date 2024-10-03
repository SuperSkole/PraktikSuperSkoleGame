using System.Collections.Generic;

namespace Analytics
{
    public interface ISpacedRepetitionManager
    {
     //   Dictionary<char, int> GetCurrentWeights();
     void RecordUsage(string unitIdentifier);
     void RecordUsage(string identifier, bool isCorrect);
    }
}