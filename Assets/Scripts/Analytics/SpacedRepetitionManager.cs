using System;
using CORE;
using CORE.Scripts;

namespace Analytics
{
    public class SpacedRepetitionManager : PersistentSingleton<SpacedRepetitionManager>, ISpacedRepetitionManager
    {
        private readonly IWeightManager weightManager;
        private readonly TimeSpan repetitionInterval = TimeSpan.FromDays(7);

        // gamemaanger will on gamestart call for checking data and adding weight for every passed day since last use
        
        // method for getting timed weight
        // method for teliing a letter has been used
        
    }
}