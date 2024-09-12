using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes;
using UnityEngine;
using System.Collections.Generic;

namespace Scenes._50_Minigames.Gamemode
{
    public class SymbolEaterSetter: IGameModeSetter
    {
        private List<IGenericGameMode> gamemodes = new List<IGenericGameMode>()
        {
            new FindSymbols(),
            null,
            new SymbolEaterLevel3(),
            new Level4_SymbolEater(),
            new Level5_SymbolEater()
        };


        private List<IGameRules> gamerules = new List<IGameRules>()
        {
            new FindVowel(),
            null,
            new FindLetterInPicture(),
            new FindFMNSConsonantBySound(),
            new FindFMNSConsonantBySound()
        };
        /// <summary>
        /// returns a gamemode of the Symbol Eater type
        /// </summary>
        /// <param name="level">The level to be used as index</param>
        /// <returns>the gamemode of the level given or null if it is outside the indexes of the list</returns>
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
            ISEGameMode modeReturned;
            switch (gamemode)
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

        /// <summary>
        /// returns a gamerule set
        /// </summary>
        /// <param name="level">The level to use as index for the desired gamerules</param>
        /// <returns></returns>
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
