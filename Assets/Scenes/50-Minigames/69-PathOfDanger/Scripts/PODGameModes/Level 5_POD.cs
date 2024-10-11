using Analytics;
using CORE;
using CORE.Scripts;
using Scenes._50_Minigames._65_MonsterTower.Scrips;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level5_POD : IPODGameMode
{
  
    private string previousRetrievedAnswer;


    /// <summary>
    /// Will be called by the PathOfDangerManager to create a platform with the correct answer
    /// </summary>
    /// <param name="str">the correct answer will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void SetCorrectAnswer(string str, PathOfDangerManager manager)
    {
        manager.soloImage.texture = ImageManager.GetImageFromLetter(str.ToLower());


    }

    /// <summary>
    /// Will be called by the PathOfDangerManager to create a platform with an (usually random) incorrect answer
    /// </summary>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void SetWrongAnswer(PathOfDangerManager manager, string correctAnswer)
    {

        var rndImageWithKey = ImageManager.GetRandomImageWithKey();

        while (rndImageWithKey.Item2 == correctAnswer)
        {
            rndImageWithKey = ImageManager.GetRandomImageWithKey();
        }

        manager.soloImage.texture = rndImageWithKey.Item1;
        manager.imageKey = rndImageWithKey.Item2;



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

        List<ILanguageUnit> letters = new List<ILanguageUnit>();

        foreach (var item in languageUnits)
        {
            if (item.LanguageUnitType == LanguageUnit.Letter)
            {
                letters.Add(item);
            }
        }
        string[] returnedString = new string[count];
        for (int i = 0; i < count; i++)
        {

            returnedString[i] = letters[Random.Range(0, 10)].Identifier;

            bool checkIfAvailable = true;

            while (checkIfAvailable)
            {
                switch (returnedString[i].ToLower())
                {
                    case "y":
                        returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                        break;

                    case "z":
                        returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                        break;

                    case "w":
                        returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                        break;

                    case "c":
                        returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                        break;

                    case "q":
                        returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                        break;

                    case "x":
                        returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                        break;

                    default:
                        checkIfAvailable = false;
                        break;
                }
            }



             checkIfAvailable = true;

            while (checkIfAvailable)
            {
                switch (returnedString[i].ToLower())
                {
                    case "y":
                        returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                        break;

                    case "z":
                        returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                        break;

                    case "w":
                        returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                        break;

                    case "c":
                        returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                        break;

                    case "q":
                        returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                        break;

                    case "x":
                        returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                        break;

                    default:
                        checkIfAvailable = false;
                        break;
                }
            }



            //Code to make sure that the previous answer is not getting repeated imediatly after. 
            while (returnedString[i] == previousRetrievedAnswer)
            {


               


                returnedString[i] = letters[Random.Range(0, 10)].Identifier;

                checkIfAvailable = true;

                while (checkIfAvailable)
                {
                    switch (returnedString[i].ToLower())
                    {
                        case "y":
                            returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                            break;

                        case "z":
                            returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                            break;

                        case "w":
                            returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                            break;

                        case "c":
                            returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                            break;

                        case "q":
                            returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                            break;

                        case "x":
                            returnedString[i] = letters[Random.Range(0, 10)].Identifier;
                            break;

                        default:
                            checkIfAvailable = false;
                            break;
                    }
                }
                

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

        
        manager.answerHolderPrefab = manager.singleImageHolderPrefab;
        manager.soloImage = manager.singleImageHolderPrefab.transform.GetChild(0).GetComponent<RawImage>();

        manager.descriptionText.text = " Tryk p� MellemRum knappen for at hoppe. Tryk p� F for at h�re et bogstav. Hop p� billedet som starter med bogstavet du h�rte";
    }
}
