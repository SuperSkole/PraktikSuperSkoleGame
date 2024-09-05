using System.Collections.Generic;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.GameModeStrategy
{
    /// <summary>
    /// Interface for gear strategies in different game modes.
    /// </summary>
    public interface IGearStrategy
    {
        List<List<char>> GetLettersForGears();
    }
}