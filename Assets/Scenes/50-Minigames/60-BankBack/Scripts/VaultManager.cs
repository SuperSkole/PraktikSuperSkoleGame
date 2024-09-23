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
    [SerializeField] private TextMeshProUGUI lives;
    [SerializeField] private TextMeshProUGUI gameOverText;
    private bool foundCode = false;
    private float  mistakes;

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
        gameOverText.text = "";
        lives.text = "3/3 liv tilbage";
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
        if(!foundCode)
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
                foundCode = true;
                ChangeMaterial(correctMaterial);
                Won();
            }
            //Changes the color to yellow if one or more numbers were correct
            else if(partialCorrect)
            {
                ChangeMaterial(partialCorrectMaterial);
                mistakes += 0.5f;
                UpdateLivesDisplay();
            }
            //Changes the color to red if none of the numbers were correct
            else
            {
                ChangeMaterial(incorrectMaterial);
                mistakes++;
                UpdateLivesDisplay();
            }
            if(mistakes >= 3)
            {
                Lost();
            }
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
    /// Gives the player xp and gold, Sets the gameover text and then starts a coroutine with WaitEeforeEnd
    /// </summary>
    private void Won()
    {
        Instantiate(coinPrefab);
        PlayerEvents.RaiseXPChanged(1);
        PlayerEvents.RaiseGoldChanged(1);
        gameOverText.text = "Du vandt. Du fandt koden til bankboksen";
        StartCoroutine(WaitBeforeEnd());
    }

    /// <summary>
    /// Sets the gameover text and then starts a coroutine with WaitBeforeEnd
    /// </summary>
    private void Lost()
    {
        gameOverText.text = "Du tabte du gættede forkert for mange gange";
        StartCoroutine(WaitBeforeEnd());
    }

    /// <summary>
    /// Waits a bit before returning to mainworld
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitBeforeEnd()
    {
        
        yield return new WaitForSeconds(5);
        SwitchScenes.SwitchToMainWorld();
    }

    /// <summary>
    /// Updates the display of the players remaining lives
    /// </summary>
    private void UpdateLivesDisplay()
    {
        if(mistakes <= 3)
        {
            lives.text = 3 - mistakes + "/3 liv tilbage";
        }
        else
        {
            lives.text = "0/3 liv tilbage";
        }
    }
}