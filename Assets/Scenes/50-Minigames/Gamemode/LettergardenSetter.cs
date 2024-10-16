using CORE.Scripts.Game_Rules;
using CORE.Scripts;
using Scenes.Minigames.LetterGarden.Scripts.Gamemodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CORE;
using Analytics;

namespace Scenes._50_Minigames.Gamemode
{
    public class LetterGardenSetter: IGameModeSetter
    {
        public List<IGenericGameMode> gamemodes = new List<IGenericGameMode>()
        {
            new DrawWithBee(),
            new DrawWithoutBee(),
            new DrawWithOrWithOutBee(),
        };

        private List<IGameRules> gamerules = new List<IGameRules>()
        {
            new DynamicGameRules(),
            new DynamicGameRules(),
            new DynamicGameRules(),
            new DynamicGameRules(),
            new DynamicGameRules()
        };

        public (IGameRules, IGenericGameMode) DetermineGamemodeAndGameRulesToUse(int level)
        {
            //GameManager.Instance.PerformanceWeightManager.SetEntityWeight("ø", 60);
            List<ILanguageUnit> languageUnits = GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(80);

            if(languageUnits[0].LanguageUnitType == Analytics.LanguageUnit.Letter)
            {
                DynamicGameRules dynamicGameRules = new DynamicGameRules();
                List<ILanguageUnit> filteredList = new List<ILanguageUnit>();
                foreach(ILanguageUnit languageUnit in languageUnits)
                {
                    if(languageUnit.LanguageUnitType == LanguageUnit.Letter)
                    {
                        filteredList.Add(languageUnit);
                    }
                }
                dynamicGameRules.AddFilteredList(filteredList);
                return(dynamicGameRules, gamemodes[Random.Range(0, gamemodes.Count)]);
            }
            //Lettergarden only supports letters
            else
            {
                return(null, null);
            }
        }


        /// <summary>
        /// returns a gamemode of the lettergarden type
        /// </summary>
        /// <param name="level">the playerlevel used as index on the gamemode list</param>
        /// <returns>the gamemode for the level if it exists. otherwise it returns null</returns>
        public IGenericGameMode SetMode(int level)
        {
            if(gamemodes.Count > level && level >= 0)
            {
                return gamemodes[level];
            }
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a gamemode based on a given string
        /// </summary>
        /// <param name="gamemode">the string representation of the given gamemode</param>
        /// <returns>the desired gamemode or the default one if the desired gamemode could not be found</returns>
        public IGenericGameMode SetMode(string gamemode)
        {
            LettergardenGameMode modeReturned;
            switch (gamemode)
            {
                case "drawcapitalLetters":
                    modeReturned = new DrawCapitalLetters();
                    break;
                case "drawlowercaseLetters":
                    modeReturned = new DrawLowercaseLetters();
                    break;
                case "drawletters":
                    modeReturned = new DrawLetters();
                    break;
                case "drawnumbers":
                    modeReturned = new DrawNumbers();
                    break;
                case "drawwithbee":
                    modeReturned = new DrawWithBee();
                    break;
                case "drawithoutbee":
                    modeReturned = new DrawWithoutBee();
                    break;
                case "drawwithorwithoutbee":
                    modeReturned = new DrawWithOrWithOutBee();
                    break;
                default:
                    Debug.LogError("given mode was not among expected options, returning default gamemode");
                    modeReturned = new DrawCapitalLetters();
                    break;
                }
                return modeReturned;
        }

        /// <summary>
        /// Gets gamerules based on the level based index given
        /// </summary>
        /// <param name="level">the level used to find the gamerules</param>
        /// <returns>the gamerules of the level if they exists. Otherwise returns null</returns>
        public IGameRules SetRules(int level)
        {
            if(gamerules.Count > level && level >= 0)
            {
                return gamerules[level];
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
        /// <returns>the desired gamerules. Otherwise returns the default set</returns>
        public IGameRules SetRules(string gamerules)
        {
            IGameRules rulesReturned;
            switch (gamerules)
            {
                case "vowels":
                    rulesReturned = new FindVowel();
                    break;
                case "consonants":
                    rulesReturned = new FindConsonant();
                    break;
                default:
                    Debug.LogError("given mode was not among expected options, returning default gamemode");
                    rulesReturned = new FindVowel();
                    break;
            }
            return rulesReturned;
        }
    }
}

