using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes;
using UnityEngine;

namespace Scenes._50_Minigames.Gamemode
{
    public class SymbolEaterSetter: IGameModeSetter
    {
        /// <summary>
        /// returns a gamemode of the Symbol Eater type
        /// </summary>
        /// <param name="mode">The mode we are looking for</param>
        /// <returns></returns>
        public IGenericGameMode SetMode(string mode)
        {
            ISEGameMode modeReturned;
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
                default:
                    Debug.Log("given mode was not among expected options, setting to default mode");
                    modeReturned = new FindImageFromSound();
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
                default:
                    Debug.Log("given ruleset was not among expected options, setting to default ruleset");
                    rulesReturned = new SpellWord();
                    break;
            }

            return rulesReturned;
        }
    }
}
