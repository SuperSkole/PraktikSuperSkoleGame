using CORE.Scripts.Game_Rules;
using CORE.Scripts;
using Scenes.Minigames.LetterGarden.Scripts.Gamemodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._50_Minigames.Gamemode
{
    public class LetterGardenSetter: IGameModeSetter
    {
        private List<string> gamemodes = new List<string>()
        {
            "drawwithbee",
            "drawithoutbee",
            "drawwithorwithoutbee",
            "drawwithbee",
            "drawithoutbee"
        };

        private List<string> gamerules = new List<string>()
        {
            "vowels",
            "vowels",
            "vowels",
            "consonants"
        };
        /// <summary>
        /// returns a gamemode of the Symbol Eater type
        /// </summary>
        /// <param name="mode">The mode we are looking for</param>
        /// <returns></returns>
        public IGenericGameMode SetMode(int level)
        {
            //Checks if level is inside the scope of the gamemodes list
            if (level >= gamemodes.Count)
            {
                return null;
            }
            LettergardenGameMode modeReturned;
            string mode = gamemodes[level];
            switch (mode)
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
                    Debug.Log("given mode was not among expected options, returning null");
                    modeReturned = null;
                    break;
            }
            return modeReturned;
        }
        /// <summary>
        /// returns a gamerule set
        /// </summary>
        /// <param name="rules">The rules we are looking for</param>
        /// <returns></returns>
        public IGameRules SetRules(int level)
        {
            //Checks if level is inside the scope of the gamerules list
            if (level >= gamerules.Count)
            {
                return null;
            }
            IGameRules rulesReturned;
            string rules = gamerules[level];
            switch (rules)
            {
                case "vowels":
                    rulesReturned = new FindVowel();
                    break;
                case "consonants":
                    rulesReturned = new FindConsonant();
                    break;
                default:
                    Debug.Log("given mode was not among expected options, returning null");
                    rulesReturned = null;
                    break;
            }

            return rulesReturned;
        }
    }
}

