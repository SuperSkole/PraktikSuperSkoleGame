using CORE.Scripts.GameRules;
using CORE.Scripts;
using Scenes.Minigames.SymbolEater.Scripts.Gamemodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenes.GameMode;

public class SymbolEaterSetter: IGameModeSetter
{
    public IGenericGameMode SetMode(string mode)
    {
        ISEGameMode modeReturned;
        switch (mode)
        {
            case "imagetosound":
                modeReturned = new FindImageFromSound();
                break;



            default:
                Debug.Log("given mode was not among expected options, setting to default mode");
                modeReturned = new FindImageFromSound();
                break;
        }
        return modeReturned;
    }

    public IGameRules SetRules(string rules)
    {
        IGameRules rulesReturned;

        switch (rules)
        {
            default:
                Debug.Log("given ruleset was not among expected options, setting to default ruleset");
                rulesReturned = new SpellWord();
                break;
        }

        return rulesReturned;
    }
}
