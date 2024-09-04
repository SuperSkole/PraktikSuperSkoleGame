using CORE.Scripts.GameRules;
using CORE.Scripts;
using Scenes.Minigames.SymbolEater.Scripts.Gamemodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes.GameMode;

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
            default:
                Debug.Log("given ruleset was not among expected options, setting to default ruleset");
                rulesReturned = new SpellWord();
                break;
        }

        return rulesReturned;
    }
}
