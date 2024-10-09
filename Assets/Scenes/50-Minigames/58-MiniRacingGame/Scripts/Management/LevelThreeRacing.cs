using CORE.Scripts;
using Letters;
using Scenes._50_Minigames._58_MiniRacingGame.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

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
        return "K\u00F8r gennem porten der passer til vokalen i billedet";
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
                if(core.languageUnits.Count > 0 && core.languageUnits[0].LanguageUnitType == Analytics.LanguageUnit.Word)
                {
                    string word = core.dynamicGameRules.GetSecondaryAnswer();
                    char vowel = word[0];
                    List<char> vowels = LetterRepository.GetVowels().ToList();
                    foreach(char letter in word)
                    {
                        if(vowels.Contains(char.ToUpper(letter)))
                        {
                            vowel = letter;
                            break;
                        }
                    }
                    core.targetWord = vowel.ToString();
                }
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