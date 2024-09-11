using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._50_Minigames._58_MiniRacingGame.Scripts;
using Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes;
using Scenes.Minigames.LetterGarden.Scripts.Gamemodes;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._50_Minigames.Gamemode
{
    
    public class MiniRacingSetter : IGameModeSetter
    {
        private List<string> gamemodes = new List<string>()
        {
            "",
            "Listen For Vocal",
            "Find Vocal In Image",
            "",
            "Listen For Consonant",
        };


        private List<string> gamerules = new List<string>()
        {
            "",
            "",
            "",
            "",
            "",
        };
        /// <summary>
        /// returns a gamemode of the Monster Tower type
        /// </summary>
        /// <param name="mode">The mode we are looking for</param>
        /// <returns></returns>
        public IGenericGameMode SetMode(int level)
        {
            if (level >= gamemodes.Count)
            {
                return null;
            }
            else
            {
                IRacingGameMode modeReturned;
                string mode = gamemodes[level];
                switch (mode)
                {
                    case "Listen For Vocal":
                        modeReturned = new LevelTwoRace();
                        break;

                    case "Find Vocal In Image":
                        modeReturned = new LevelThreeRacing();
                        break;

                    case "Listen For Consonant":
                        modeReturned = new LevelFiveRacing();

                        break;
                    default:
                        Debug.Log("given mode was not among expected options, returning null");
                        modeReturned = null;
                        break;
                }
                return modeReturned;
            }
        }

        /// <summary>
        /// returns a gamerule set
        /// </summary>
        /// <param name="rules">The rules we are looking for</param>
        /// <returns></returns>
        public IGameRules SetRules(int level)
        {
            if (level >= gamerules.Count)
            {
                return null;
            }
            else
            {
                IGameRules rulesReturned;
                string rules = gamerules[level];
                switch (rules)
                {
                    default:
                        Debug.Log("given ruleset was not among expected options, returning null");
                        rulesReturned = null;
                        break;
                }
                return rulesReturned;
            }
        }
    }
}