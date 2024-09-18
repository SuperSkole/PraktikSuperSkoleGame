using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Scenes;
using Scenes._10_PlayerScene.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// Manager for the Bank back entrance minigame
/// </summary>
public class VaultManager : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] private List<SoundButton> soundButtons;
    [SerializeField] private List<GearScript> gearScripts;

    [SerializeField] private Material correctMaterial;
    [SerializeField] private Material partialCorrectMaterial;
    [SerializeField] private Material incorrectMaterial;

    private int[] answer = new int[4];
    /// <summary>
    /// Currently just calls startgame
    /// </summary>
    void Start()
    {
        StartGame();
    }

    /// <summary>
    /// Sets up the various variables used by the minigame
    /// </summary>
    public void StartGame()
    {
        for(int i = 0; i < 4; i++)
        {
            answer[i] = Random.Range(1, 10);
            soundButtons[i].amount = answer[i];
        }
    }

    /// <summary>
    /// Checks if the numbers are all correct, some of them are correct or if none of them are correct. It then colors the teeth of the gears accordingly 
    /// </summary>
    public void ValidateGuess()
    {
        bool correct = true;
        bool partialCorrect = false;
        //Runs through the gears and checks if there are correct and incorrect numbers
        for(int i = 0; i < 4; i++)
        {
            if(correct && answer[i] != gearScripts[i].currentNumber)
            {
                correct = false;
            }
            else if(!partialCorrect && answer[i] == gearScripts[i].currentNumber)
            {
                partialCorrect = true;
            }
        }
        //Changes the color of the teeth to green if all numbers were correct and afterwards prepares to end the game
        if(correct)
        {
            ChangeMaterial(correctMaterial);
            StartCoroutine(WaitBeforeEnd());
        }
        //Changes the color to yellow if one or more numbers were correct
        else if(partialCorrect)
        {
            ChangeMaterial(partialCorrectMaterial);
        }
        //Changes the color to red if none of the numbers were correct
        else
        {
            ChangeMaterial(incorrectMaterial);
        }
    }

    /// <summary>
    /// runs through the gears and calls their changematerial method
    /// </summary>
    /// <param name="material">The material the gears schould use</param>
    private void ChangeMaterial(Material material)
    {
        foreach(GearScript gearScript in gearScripts)
        {
            gearScript.ChangeMaterial(material);
        }
    }

    /// <summary>
    /// Gives the player xp and gold, waits a bit and then returns to main world
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitBeforeEnd()
    {
        Instantiate(coinPrefab);
        PlayerEvents.RaiseXPChanged(1);
        PlayerEvents.RaiseGoldChanged(1);
        yield return new WaitForSeconds(2);
        SwitchScenes.SwitchToMainWorld();
    }
}