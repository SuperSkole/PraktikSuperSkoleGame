using Analytics;
using CORE;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Letters;
using Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes;
using Scenes._50_Minigames._58_MiniRacingGame.Scripts;
using Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._50_Minigames.Gamemode
{

    public class MiniRacingSetter : IGameModeSetter
    {
       
        public List<IGenericGameMode> gamemodes = new List<IGenericGameMode>()
        {
            null,
            new LevelTwoRace(),
            new LevelThreeRacing(),
            null,
            new LevelFiveRacing()
        };

        public List<IGenericGameMode> letterGamemodes = new List<IGenericGameMode>
        {
            new LevelTwoRace(),
            new LevelThreeRacing(),
            new LevelFiveRacing()
        };


        private readonly List<string> gamerules = new()
        {
            "",
            "",
            "",
            "",
            "",
        };

        /// <summary>
        /// Returns a gamemode based on a string
        /// </summary>
        /// <param name="gamemode">the string representation of the desired gamemode</param>
        /// <returns>a gamemode</returns>
        public IGenericGameMode SetMode(string gamemode)
        {
            IRacingGameMode modeReturned;
            switch (gamemode)
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


        /// <summary>
        /// returns a gamemode of the Monster Tower type
        /// </summary>
        /// <param name="mode">The mode we are looking for</param>
        /// <returns></returns>
        public IGenericGameMode SetMode(int level)
        {
            if (gamemodes.Count > level && level >= 0)
            {
                switch (level)
                {
                    case 1:
                        return null;
                    case 2:

                        return null;
                    default:
                        break;
                }
                return gamemodes[level];
            }
            else
            {
                return null;
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

        /// <summary>
        /// Returns some gamerules based on a string
        /// </summary>
        /// <param name="gamerules">the string representation of the desired gamerules</param>
        /// <returns>some gamerules</returns>
        public IGameRules SetRules(string gamerules)
        {
            IGameRules rulesReturned;
            switch (gamerules)
            {
                default:
                    Debug.Log("given ruleset was not among expected options, returning null");
                    rulesReturned = null;
                    break;
            }
            return rulesReturned;
        }

        public (IGameRules, IGenericGameMode) DetermineGamemodeAndGameRulesToUse(int level)
        {
            ILanguageUnit languageUnit = GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(1)[0];
            IGenericGameMode mode;
            switch (languageUnit.LanguageUnitType)
            {
                case LanguageUnit.Letter:
                    mode = letterGamemodes[Random.Range(0, letterGamemodes.Count)];
                    return (null,mode);
                case LanguageUnit.Word:
                    return (null, null);
                case LanguageUnit.Sentence:
                    return (null,null);
                default:
                    break;
            }
            return (null, null);
        }

    }
}