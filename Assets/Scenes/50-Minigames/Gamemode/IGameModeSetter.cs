using CORE.Scripts;
using CORE.Scripts.Game_Rules;

namespace Scenes._50_Minigames.Gamemode
{
    
    public interface IGameModeSetter
    {
        /// <summary>
        /// Each script using this should have a switch case that turns a string into a game mode
        /// </summary>
        /// <param name="mode">string to represent what gamemode needs to be created and set</param>
        /// <param name="rules">string to represent what gamerules need to be created and set</param>
        public IGenericGameMode SetMode(string mode);

        public IGameRules SetRules(string rules);
    }
}