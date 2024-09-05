using System.Collections.Generic;
using CORE.Scripts;

namespace Scenes.Minigames.LetterGarden.Scripts.Gamemodes
{
    /// <summary>
    /// Interface for gamemodes in the lettergarden minigame
    /// </summary>
    public interface LettergardenGameMode: IGenericGameMode
    {
        /// <summary>
        /// gets symbols the player should draw
        /// </summary>
        /// <param name="amount">How many symbols the player should draw</param>
        /// <returns>A list of splined symbols the player should draw</returns>
        public List<SplineSymbolDataHolder> GetSymbols(int amount);
    }
}