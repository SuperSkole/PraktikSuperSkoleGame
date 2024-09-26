using CORE.Scripts;
using Scenes._50_Minigames._65_MonsterTower.Scrips;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPODGameMode : IGenericGameMode
{
    /// <summary>
    /// Will be called by the Path of Danger Manager to create a platform with the correct answer
    /// </summary>
    /// <param name="str">the correct answer will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void SetCorrectAnswer(string str, PathOfDangerManager manager);

    /// <summary>
    /// Will be called by the Path of Danger Manager to create a platform with an (usually random) incorrect answer
    /// </summary>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void SetWrongAnswer(PathOfDangerManager manager, string correctAnswer);

    /// <summary>
    /// Set's the answer key, which will tell the player which platform is correct. Uses the opposite medium of SetCorrectAnswer
    /// </summary>
    /// <param name="str">The answer key will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void GetDisplayAnswer(string str, PathOfDangerManager manager);

    /// <summary>
    /// create a series of answers
    /// </summary>
    /// <param name="count">number of answers to create</param>
    /// <returns>Returns a set of answers strings to be used by the PathOfDangerManager</returns>
    public string[] GenerateAnswers(int count);

    /// <summary>
    /// will pick the correct prefab from prefabs in the PathOfDangerManager, then set that to be the PathOfDangerManagers AnswerHolderPrefab
    /// </summary>
    /// <param name="manager">references back to the PathOfDangerManager so it can pick one of the prefabs and use it for the answerholder</param>
    public void SetAnswerPrefab(PathOfDangerManager manager);
}

