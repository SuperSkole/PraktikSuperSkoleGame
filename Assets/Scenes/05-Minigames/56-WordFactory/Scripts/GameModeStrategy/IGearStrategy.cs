using System.Collections.Generic;

namespace Scenes._05_Minigames._56_WordFactory.Scripts
{
    /// <summary>
    /// Interface for gear strategies in different game modes.
    /// </summary>
    public interface IGearStrategy
    {
        List<List<char>> GetLettersForGears();
    }
}