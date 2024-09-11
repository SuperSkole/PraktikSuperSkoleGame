using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes;
using UnityEngine;
using System.Collections.Generic;

namespace Scenes._50_Minigames.Gamemode
{
    public class SymbolEaterSetter: IGameModeSetter
    {
        private List<string> gamemodes = new List<string>()
        {
            "findsymbols",
            "",
            "SymbolEaterLevel3",
            "Level4_SymbolEater",
            "Level5_SymbolEater"
        };


        private List<string> gamerules = new List<string>()
        {
            "findvowels",
            "",
            "GetVowelFromPic",
            "Level4_SymbolEater",
            "Level5_SymbolEater"
        };
        /// <summary>
        /// returns a gamemode of the Symbol Eater type
        /// </summary>
        /// <param name="mode">The mode we are looking for</param>
        /// <returns></returns>
        public IGenericGameMode SetMode(int level)
        {
            if(level >= gamemodes.Count)
            {
                return null;
            }
            else
            {
                ISEGameMode modeReturned;
                string mode = gamemodes[level];
                switch (mode)
                {
                    case "spellword":
                        modeReturned = new SpellWordFromImage();
                        break;
                    case "imagetosound":
                        modeReturned = new FindImageFromSound();
                        break;
                    case "recognizesoundofletter":
                        modeReturned = new RecognizeSoundOfLetter();
                        break;
                    case "recognizenameofletter":
                        modeReturned = new RecognizeNameOfLetter();
                        break;
                    case "findnumber":
                        modeReturned = new FindNumber();
                        break;
                    case "findsymbol":
                        modeReturned = new FindSymbol();
                        break;
                    case "findsymbols":
                        modeReturned = new FindSymbols();
                        break;
                    case "findfirstletterfromimage":
                        modeReturned = new FindFirstLetterFromImage();
                        break;
                    case "spellincorrectword":
                        modeReturned = new SpellIncorrectWord();
                        break;
                    case "SymbolEaterLevel3":
                        modeReturned = new SymbolEaterLevel3();
                        break;

                    case "Level4_SymbolEater":
                        modeReturned = new Level4_SymbolEater();
                        break;

                    case "Level5_SymbolEater":
                        modeReturned = new Level5_SymbolEater();
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
            if(level >= gamemodes.Count)
            {
                return null;
            }
            else
            {
                IGameRules rulesReturned;
                string rules = gamerules[level];
                switch (rules)
                {
                    case "spellword":
                        rulesReturned = new SpellWord();
                        break;

                    case "findnumberseries":
                        rulesReturned = new FindNumberSeries();
                        break;
                    case "findcorrectletter":
                        rulesReturned = new FindCorrectLetter();
                        break;
                    case "findlettertype":
                        rulesReturned = new FindLetterType();
                        break;
                    case "findnextletter":
                        rulesReturned = new FindNextLetter();
                        break;
                    case "findfirstletter":
                        rulesReturned = new FindFirstLetter();
                        break;
                    case "findincorrectwords":
                        rulesReturned = new FindIncorrectWords();
                        break;
                    case "findvowels":
                        rulesReturned = new FindVowel();
                        break;
                    case "findconsonants":
                        rulesReturned = new FindConsonant();
                        break;
                    case "GetVowelFromPic":
                        rulesReturned = new FindLetterInPicture();
                        break;

                    case "Level4_SymbolEater":
                        rulesReturned = new FindFMNSConsonantBySound();
                        break;
                    case "Level5_SymbolEater":
                        rulesReturned = new FindFMNSConsonantBySound();
                        break;
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
