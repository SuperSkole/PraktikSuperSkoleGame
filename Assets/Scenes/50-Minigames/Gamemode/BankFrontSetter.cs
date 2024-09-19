using System.Collections;
using System.Collections.Generic;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes;
using Scenes._50_Minigames.Gamemode;
using UnityEngine;

public class BankFrontSetter : IGameModeSetter
{
    public IGenericGameMode SetMode(int level)
    {
        return new SortAndCount();
    }

    public IGenericGameMode SetMode(string gamemode)
    {
        return new SortAndCount();
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
