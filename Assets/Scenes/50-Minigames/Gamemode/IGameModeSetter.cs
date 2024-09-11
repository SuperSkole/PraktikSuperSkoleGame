using CORE.Scripts;
using CORE.Scripts.Game_Rules;

namespace Scenes._50_Minigames.Gamemode
{
    
    public interface IGameModeSetter
    {
        /// <summary>
        /// Gets a gamemode based on the given integer based on the players level. The implementation used so far uses a list of gamemodes where index equals the level - 1
        /// </summary>
        /// <param name="level">the index used by the method</param>
        /// <returns>a gamemode or null if no gamemode exists for the given level</returns>
        public IGenericGameMode SetMode(int level);

        /// <summary>
        /// Gets some gamerules based on the given integer based on the players level. The implementation used so far uses a list of gamerules where index equals the level - 1
        /// </summary>
        /// <param name="level">the index used by the method</param>
        /// <returns>some gamerules or null if no gamerules exists for the given level</returns>
        public IGameRules SetRules(int level);

        /// <summary>
        /// Returns a gamemode based on a string
        /// </summary>
        /// <param name="gamemode">the string representation of the desired gamemode</param>
        /// <returns>a gamemode</returns>
        public IGenericGameMode SetMode(string gamemode);

        /// <summary>
        /// Returns some gamerules based on a string
        /// </summary>
        /// <param name="gamerules">the string representation of the desired gamerules</param>
        /// <returns>some gamerules</returns>
        public IGameRules SetRules(string gamerules);
    }
}