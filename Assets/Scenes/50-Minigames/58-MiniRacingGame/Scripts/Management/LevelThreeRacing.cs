using CORE.Scripts;
using Scenes._50_Minigames._58_MiniRacingGame.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelThreeRacing : IRacingGameMode
{
    /// <summary>
    /// Returns the current gamemode
    /// </summary>
    public string returnMode()
    {
        return "Find Vocal In Image";
    }
    /// <summary>
    /// Returns the current objective
    /// </summary>
    public string displayObjective()
    {
        return "K�r igennem den halvdel der har en vokal som er i billedets ord.";
    }


    /// <summary>
    /// Funtion that determins the what the correct word/image is in the current gameMode. 
    /// </summary>
    /// <param name="core"></param>
    public void DetermineWordToUse(RacingCore core)
    {

        if (core.spelledWordsList.Count < 3)
        {

            //Selects a random vocal letter
            do
            {
                core.gameRuleVocal.SetCorrectAnswer();
                core.targetWord = core.gameRuleVocal.GetCorrectAnswer();
            } while (core.spelledWordsList.Contains(core.targetWord));
            if (core.currentMode != GameModes.Mode2)
            {
                core.imageWord = core.gameRuleVocal.GetDisplayAnswer();
                Texture2D image = ImageManager.GetImageFromWord(core.imageWord);
                core.wordsImageMap.Add(core.imageWord, core.QuickSprite(image));
            }
            else
                core.imageWord = "";


            if (!core.imageInitialized)
                core.imageInitialized = true;
            core.PlayWordAudio(core.targetWord);
            core.UpdateWordImageDisplay();

        }
        else
            core.EndGame();
    }
}
