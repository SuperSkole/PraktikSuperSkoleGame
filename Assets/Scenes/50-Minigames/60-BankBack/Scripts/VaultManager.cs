using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Scenes;
using Scenes._10_PlayerScene.Scripts;
using Scenes._50_Minigames._65_MonsterTower.Scrips;
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
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private TextMeshProUGUI lives;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private AlarmScript alarmScript;
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private TextMeshProUGUI desiredInput;
    [SerializeField] private GameObject ui;
    [SerializeField] private VaultOpener vaultOpener;
    private bool waitingForEnd;
    private int desiredInputIndex;

    private List<string> desiredInputNames = new List<string>()
    {
        "1000'ere", "100'ere", "10'ere", "1'ere"
    };
    private bool foundCode = false;
    private float  mistakes;

    private int[] answer = new int[4];
    private List<int> usedDesiredInputIndices = new List<int>();
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
        lives.text = "3/3 liv";
        for(int i = 0; i < 4; i++)
        {
            answer[i] = Random.Range(1, 10);
            soundButtons[i].amount = answer[i];
        }
        codeText.text = "Koden er " + answer[0] + answer[1] + answer[2] + answer[3];
        desiredInputIndex = Random.Range(0, desiredInputNames.Count);
        desiredInput.text = "Indsæt " + desiredInputNames[desiredInputIndex];
        usedDesiredInputIndices.Add(desiredInputIndex);
    }

    /// <summary>
    /// Checks if the numbers are all correct, some of them are correct or if none of them are correct. It then colors the teeth of the gears accordingly 
    /// </summary>
    public void ValidateGuess()
    {
        if(!foundCode)
        {
            //Changes the color of the teeth to green if all numbers were correct and afterwards prepares to end the game
            bool correct = true;
            for(int i = 0; i < answer.Length; i++)
            {
                if(usedDesiredInputIndices.Contains(i) && !(answer[i] == gearScripts[i].currentNumber))
                {
                    correct = false;
                    break;
                }
                else if(!usedDesiredInputIndices.Contains(i) && answer[i] == gearScripts[i].currentNumber)
                {
                    correct = false;
                    break;
                }
            }
            if(correct)
            {
                ChangeMaterial(correctMaterial);
                if(usedDesiredInputIndices.Count == 4)
                {
                    vaultOpener.StartMove();
                    Won();
                    foundCode = true;
                }
                else
                {
                    StartCoroutine(ResetMaterial());
                    desiredInputIndex = Random.Range(0, desiredInputNames.Count);
                    while(usedDesiredInputIndices.Contains(desiredInputIndex))
                    {
                        desiredInputIndex = Random.Range(0, desiredInputNames.Count);
                    }
                    usedDesiredInputIndices.Add(desiredInputIndex);
                    desiredInput.text = "Indsæt " + desiredInputNames[desiredInputIndex];
                }
            }
            //Changes the color to red if none of the numbers were correct
            else
            {
                ChangeMaterial(incorrectMaterial);
                mistakes++;
                alarmScript.DetermineAlarm(mistakes);
                UpdateLivesDisplay();
            }
            if(mistakes >= 3)
            {
                Lost();
            }
        }
    }

    /// <summary>
    /// Old Validate Guess. Saved as comment in case the original gamemode gets reimplemented
    /// Checks if the numbers are all correct, some of them are correct or if none of them are correct. It then colors the teeth of the gears accordingly 
    /// </summary>
    /*public void ValidateGuessOld()
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
                alarmScript.DetermineAlarm(mistakes);
                UpdateLivesDisplay();
            }
            //Changes the color to red if none of the numbers were correct
            else
            {
                ChangeMaterial(incorrectMaterial);
                mistakes++;
                alarmScript.DetermineAlarm(mistakes);
                UpdateLivesDisplay();
            }
            if(mistakes >= 3)
            {
                Lost();
            }
        }
    }*/

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
        StartCoroutine(SpawnCoins());
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
        waitingForEnd = true;
        yield return new WaitForSeconds(5);
        waitingForEnd = false;
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

    /// <summary>
    /// Resets the material of the gears after 2 seconds to the default material
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(2);
        foreach(GearScript gearScript in gearScripts)
        {
            gearScript.ChangeMaterial(defaultMaterial);
        }
    }

    /// <summary>
    /// Spawns coins continually until the game has ended
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnCoins()
    {
        while(waitingForEnd)
        {
            GameObject coin = Instantiate(coinPrefab);
            Vector3 newPos = new Vector3(Random.Range(100, 1820), Random.Range(100, 980), coin.transform.position.z);
            coin.transform.SetParent(ui.transform, false);
            coin.GetComponent<RectTransform>().position = newPos;
            yield return new WaitForSeconds(0.01f);
        }
        
    }
}