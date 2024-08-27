using System.Collections.Generic;

namespace Scenes._05_Minigames.WordFactory.Scripts
{
    /// <summary>
    /// Interface for gear strategies in different game modes.
    /// </summary>
    public interface IGearStrategy
    {
        List<List<char>> GetLettersForGears();
    }
}