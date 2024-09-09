using CORE.Scripts;
using CORE.Scripts.Game_Rules;

namespace Scenes._50_Minigames.Gamemode
{
    
    public interface IGameModeSetter
    {
        /// <summary>
        /// Each script using this should have a switch case that turns a string into a game mode
        /// </summary>
        /// <param name="level">The level of the player</param>
        /// <returns></returns>
        public IGenericGameMode SetMode(int level);

        public IGameRules SetRules(int level);
    }
}