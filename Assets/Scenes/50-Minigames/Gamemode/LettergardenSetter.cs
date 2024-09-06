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
        /// <summary>
        /// returns a gamemode of the Symbol Eater type
        /// </summary>
        /// <param name="mode">The mode we are looking for</param>
        /// <returns></returns>
        public IGenericGameMode SetMode(string mode)
        {
            LettergardenGameMode modeReturned;
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
                    Debug.Log("given mode was not among expected options, setting to default mode");
                    modeReturned = new DrawLetters();
                    break;
            }
            return modeReturned;
        }
        /// <summary>
        /// returns a gamerule set
        /// </summary>
        /// <param name="rules">The rules we are looking for</param>
        /// <returns></returns>
        public IGameRules SetRules(string rules)
        {
            IGameRules rulesReturned;

            switch (rules)
            {
                case "vowels":
                    rulesReturned = new FindVowel();
                    break;
                case "consonants":
                    rulesReturned = new FindConsonant();
                    break;
                default:
                    rulesReturned = new SpellWord();
                    break;
            }

            return rulesReturned;
        }
    }
}

