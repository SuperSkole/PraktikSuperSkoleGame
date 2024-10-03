using System.Collections;
using System.Collections.Generic;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes;
using Scenes._50_Minigames.Gamemode;
using UnityEditor;
using UnityEngine;

public class BankFrontSetter : IGameModeSetter
{
    public IGenericGameMode SetMode(int level)
    {
        return new SortAndCountAll();
    }

    public IGenericGameMode SetMode(string gamemode)
    {
        IGenericGameMode modeToBeUsed;
        switch(gamemode)
        {
            case "sortandcount":
                modeToBeUsed = new SortAndCountAll();
                break;
            case "sortandcountonesandtwos":
                modeToBeUsed = new SortAndCountOnesAndTwos();
                break;
            case "sortandcountAllExceptDecimals":
                modeToBeUsed = new SortAndCountAllExceptDecimals();
                break;
            case "sort":
                modeToBeUsed = new Sort();
                break;
            case "count":
                modeToBeUsed = new Count();
                break;
            case "sortanimals":
                modeToBeUsed = new SortAnimals();
                break;
            default:
                Debug.Log("unknown gamemode. Returning the default gamemode");
                modeToBeUsed = new SortAndCountAll();
                break;
        }
        return modeToBeUsed;
    }

    public IGameRules SetRules(int level)
    {
        return new FindCorrectLetter();
    }

    public IGameRules SetRules(string gamerules)
    {
        return new FindCorrectLetter();
    }
}