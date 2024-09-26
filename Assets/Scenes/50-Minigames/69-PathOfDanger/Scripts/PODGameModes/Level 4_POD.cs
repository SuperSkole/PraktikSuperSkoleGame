using CORE.Scripts;
using Scenes._50_Minigames._65_MonsterTower.Scrips;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level4_POD : MonoBehaviour,IPODGameMode
{
    List<char> FMNSConsonants = LetterManager.GetFMNSConsonants();



    /// <summary>
    /// Will be called by the PathOfDangerManager to create a platform with the correct answer
    /// </summary>
    /// <param name="str">the correct answer will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void SetCorrectAnswer(string str, PathOfDangerManager manager)
    {


        foreach (var item in FMNSConsonants)
        {
            if (item == str.ToCharArray()[0])
            {
                manager.textOnPlatform.text = item.ToString();
            }
        }


    }

    /// <summary>
    /// Will be called by the PathOfDangerManager to create a platform with an (usually random) incorrect answer
    /// </summary>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void SetWrongAnswer(PathOfDangerManager manager, string correctAnswer)
    {
        var rndVowelWithKey = LetterManager.GetRandomFMNSConsonant();

        while (rndVowelWithKey == correctAnswer.ToCharArray()[0])
        {
            rndVowelWithKey = LetterManager.GetRandomFMNSConsonant();
        }

        manager.textOnPlatform.text = rndVowelWithKey.ToString();



        manager.imageKey = rndVowelWithKey.ToString();


    }

    /// <summary>
    /// Sets the answer key, which will tell the player which platform is correct. Uses the opposite medium of SetCorrectAnswer
    /// </summary>
    /// <param name="str">The answer key will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void GetDisplayAnswer(string str, PathOfDangerManager manager)
    {



        AudioClip clip = LetterAudioManager.GetAudioClipFromLetter(str + "1");

        manager.hearLetterButtonAudioSource.GetComponent<AudioSource>().clip = clip;

    }

    /// <summary>
    /// create a series of answers
    /// </summary>
    /// <param name="count">number of answers to create</param>
    /// <returns>Returns a set of answers strings to be used by the PathOfDangerManager</returns>
    public string[] GenerateAnswers(int count)
    {


        string[] returnedString = new string[count];
        for (int i = 0; i < count; i++)
        {
            returnedString[i] = LetterManager.GetRandomFMNSConsonant().ToString();
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

        manager.descriptionText.text = " Tryk på MellemRum knappen for at hoppe og høre et bogstav. Hop på det rigtige bogstav";
    }
}
