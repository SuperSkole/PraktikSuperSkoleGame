using Analytics;
using CORE;
using CORE.Scripts;
using Scenes._50_Minigames._65_MonsterTower.Scrips;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level4_POD : IPODGameMode
{
    
    private string previousRetrievedAnswer;

   

    /// <summary>
    /// Will be called by the PathOfDangerManager to create a platform with the correct answer
    /// </summary>
    /// <param name="str">the correct answer will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void SetCorrectAnswer(string str, PathOfDangerManager manager)
    {


      
                manager.textOnPlatform.text = str.ToUpper();
            
        


    }

    /// <summary>
    /// Will be called by the PathOfDangerManager to create a platform with an (usually random) incorrect answer
    /// </summary>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void SetWrongAnswer(PathOfDangerManager manager, string correctAnswer)
    {
        var rndLetterWithKey = LetterManager.GetRandomLetter();


        while (rndLetterWithKey == correctAnswer.ToCharArray()[0])
        {
            rndLetterWithKey = LetterManager.GetRandomLetter();
        }

        manager.textOnPlatform.text = rndLetterWithKey.ToString();



        manager.imageKey = rndLetterWithKey.ToString();


    }

    /// <summary>
    /// Sets the answer key, which will tell the player which platform is correct. Uses the opposite medium of SetCorrectAnswer
    /// </summary>
    /// <param name="str">The answer key will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void GetDisplayAnswer(string str, PathOfDangerManager manager)
    {

       
        AudioClip clip = LetterAudioManager.GetAudioClipFromLetter(str + "1");

        manager.hearLetterButtonAudioClip = clip;

    }

    /// <summary>
    /// create a series of answers
    /// </summary>
    /// <param name="count">number of answers to create</param>
    /// <returns>Returns a set of answers strings to be used by the PathOfDangerManager</returns>
    public string[] GenerateAnswers(int count)
    {


        List<ILanguageUnit> languageUnits = GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(80);

        List<ILanguageUnit> letters=new List<ILanguageUnit>();

        foreach (var item in languageUnits)
        {
            if(item.LanguageUnitType==LanguageUnit.Letter)
            {
                letters.Add(item);
            }
        }

        string[] returnedString = new string[count];
        for (int i = 0; i < count; i++)
        {

            //Code to make sure that the previous answer is not getting repeated imediatly after. 

            returnedString[i] = letters[Random.Range(0, 10)].Identifier;

            while (returnedString[i]==previousRetrievedAnswer)
            {
          
                    returnedString[i] = letters[Random.Range(0, 10)].Identifier;

            }

            previousRetrievedAnswer = returnedString[i];
        }

   

        return returnedString;
    }
    /// <summary>
    /// changes the prefab of the PathOfDangerManager so we only apply 1 image to the platforms
    /// </summary>
    /// <param name="manager">a reference back to the PathOfDangerManager</param>
    public void SetAnswerPrefab(PathOfDangerManager manager)
    {
       
        manager.answerHolderPrefab = manager.textHolderPrefab;

        manager.textOnPlatform = manager.textHolderPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        manager.descriptionText.text = " Tryk på MellemRum knappen for at hoppe. Tryk på F for at høre et bogstav. Hop på det rigtige bogstav";
    }
}
