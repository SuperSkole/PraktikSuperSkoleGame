using Scenes._50_Minigames._58_MiniRacingGame.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoRace : IRacingGameMode
{
    /// <summary>
    /// Returns the current gamemode
    /// </summary>
    public string returnMode()
    {
        return "Listen For Vocal";
    }
    /// <summary>
    /// Returns the current objective
    /// </summary>
    public string displayObjective()
    {
        return "Kør igennem den halvdel der lyder som vokalen der bliver sagt.";
    }
}
