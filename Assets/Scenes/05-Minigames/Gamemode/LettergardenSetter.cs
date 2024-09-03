using CORE.Scripts.GameRules;
using CORE.Scripts;
using Scenes.Minigames.SymbolEater.Scripts.Gamemodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes.GameMode;
using Scenes.Minigames.LetterGarden.Scripts.Gamemodes;

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
            case "DrawCapitalLetters":
                modeReturned = new DrawCapitalLetters();
                break;
            case "DrawLowercaseLetters":
                modeReturned = new DrawLowercaseLetters();
                break;
            case "DrawLetters":
                modeReturned = new DrawLetters();
                break;
            case "DrawNumbers":
                modeReturned = new DrawNumbers();
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
            default:
                rulesReturned = new SpellWord();
                break;
        }

        return rulesReturned;
    }
}
