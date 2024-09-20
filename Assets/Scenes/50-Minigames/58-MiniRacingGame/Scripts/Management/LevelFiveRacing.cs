using Scenes._50_Minigames._58_MiniRacingGame.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFiveRacing : IRacingGameMode
{
    /// <summary>
    /// Returns the current gamemode
    /// </summary>
    public string returnMode()
    {
        return "Listen For Consonant";
    }
    /// <summary>
    /// Returns the current objective
    /// </summary>
    public string displayObjective()
    {
        return "K\u00f8r igennem den halvdel der lyder som konsonanten der bliver sagt.";
    }
    /// <summary>
    /// Sets what the correct word/image is for the current gameMode.
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void DetermineWordToUse(RacingCore core)
    {
        if (core.spelledWordsList.Count < 3)
        {
            /// Selects a random consonant
            do
            {
                core.targetWord = core.level5Consonants[Random.Range(0, core.level5Consonants.Length)];
            } while (core.spelledWordsList.Contains(core.targetWord));
            core.imageWord = "";

            if (!core.imageInitialized)
                core.imageInitialized = true;
            core.PlayWordAudio(core.targetWord);
            core.UpdateWordImageDisplay();
        }
        else
        {
            core.EndGame();
        }


    }
}
