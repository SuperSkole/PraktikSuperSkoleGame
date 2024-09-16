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
    [SerializeField] private int numOfModifiers;

    [SerializeField] TextMeshPro hintField;
    [SerializeField] GameObject modObject;
    [SerializeField] private TMP_InputField playerGuessField;
    private List<TMP_InputField> modifierGuessFields = new List<TMP_InputField>();
    [SerializeField] private GameObject modifierContainer;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private List<int> modifiers = new List<int>();
    private List<char> possibleOperators = new List<char>(){
        '+', '-'
    };

    private List<SoundButton> soundButtons = new List<SoundButton>();
    private List<char> operators = new List<char>();
    private List<int> modifierGuesses = new List<int>();
    private int[] hint = new int[4];

    private int[] answer = new int[4];
    private int integerAnswer;
    private int playerGuess;
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
        //Sets up the hint and finds the lowest number in it
        int lowest = 10;
        string hintString = "";
        for (int j = 0; j < 4; j++)
        {
            hint[j] = Random.Range(0, 10);
            if(hint[j] < lowest)
            {
                lowest = hint[j];
            }
            hintString = hintString + hint[j];
        } 
        hintField.text = hintString;
        //Sets up the modifiers and ensures they never gets final result or the potiential partial result below 0
        int total = 0;
        for(int i = 0; i < numOfModifiers; i++)
        {
            
            //Instantiates the modifier box and sets up various related variables
            GameObject modifierObject = Instantiate(modObject);
            modifierObject.transform.SetParent(modifierContainer.transform);
            char op = possibleOperators[Random.Range(0, possibleOperators.Count)];
            modifierObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = op.ToString();
            operators.Add(op);
            modifiers.Add(Random.Range(1, 10));
            if(op == '-')
            {
                modifiers[i] = - modifiers[i];
            }
            total += modifiers[i];
            //Ensures the total so far is not below 0. If it is a new operator and modifier is found
            while(lowest + total < 0)
            {
                total -= modifiers[i];
                operators[i] = possibleOperators[Random.Range(0, possibleOperators.Count)];
                modifiers[i] = Random.Range(0, 10);
                if(operators[i] == '-')
                {
                    modifiers[i] = - modifiers[i];
                }
                total += modifiers[i];
            }
            //Sets up the sound button with the correct sound file and amount of times it should be played
            soundButtons.Add(modifierObject.transform.GetChild(2).GetComponent<SoundButton>());
            soundButtons[i].audioSource = audioSource;
            soundButtons[i].amount = modifiers[i];

            //Sets up UpdateModifierGuess to listen for updates in the textfield of the modifier block
            modifierGuessFields.Add(modifierObject.transform.GetChild(1).GetComponent<TMP_InputField>());
            modifierGuessFields[i].onValueChanged.AddListener(delegate{UpdateModifierGuess();});
            modifierGuesses.Add(0);
        }
        //Calculates the answer
        answer = hint;
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < modifiers.Count; j++)
            {
                answer[i]+= modifiers[j];
            }
            answer[i] = answer[i] % 10;
        }
        //Converts the answer to an integer for easier comparison
        integerAnswer = 0;
        for(int i = 0; i < answer.Length; i++)
        {
            integerAnswer += answer[i] * Convert.ToInt32(Mathf.Pow(10, answer.Length - i - 1));
        }
    }

    /// <summary>
    /// Called when the field with the players code guess is changed and updates playerguess if the text can be converted to an integer
    /// </summary>
    public void UpdatePlayerGuess()
    {
        int number;
        bool res = Int32.TryParse(playerGuessField.text, out number);
        if(res)
        {
            playerGuess = number;
        }
    }

    /// <summary>
    /// Updates modifiers when one of them changes and if their content can be converted to integers
    /// </summary>
    public void UpdateModifierGuess()
    {
        for(int i = 0; i < modifierGuessFields.Count; i++)
        {
            int number;
            bool res = Int32.TryParse(modifierGuessFields[i].text, out number);
            if(res)
            {
                modifierGuesses[i] = number;
            }
        }
    }

    /// <summary>
    /// Checks if the modifiers used by the player is correct and if their final guess is also correct
    /// </summary>
    public void ValidateGuess()
    {
        //Checks the modifiers
        bool correctModifierGuesses = true;
        bool partialCorrect = false;
        for(int i = 0; i < modifierGuesses.Count; i++)
        {
            if(modifierGuesses[i] != modifiers[i])
            {
                correctModifierGuesses = false;
                break;
            }
            else
            {
                partialCorrect = true;
            }
        }
        //Colors the modifier guess fields according to how correct the player was
        if(correctModifierGuesses)
        {
            foreach(TMP_InputField modifierGuessField in modifierGuessFields)
            {
                modifierGuessField.gameObject.GetComponent<Image>().color = Color.green;
            }
        }
        else if (partialCorrect)
        {
            foreach(TMP_InputField modifierGuessField in modifierGuessFields)
            {
                modifierGuessField.gameObject.GetComponent<Image>().color = Color.yellow;
            }
        }
        else
        {
            foreach(TMP_InputField modifierGuessField in modifierGuessFields)
            {
                modifierGuessField.gameObject.GetComponent<Image>().color = Color.red;
            }
        }

        //Checks the players final answer
        if(playerGuess == integerAnswer)
        {
            playerGuessField.gameObject.GetComponent<Image>().color = Color.green;
            //If modifiers was also correct ends the game
            if(correctModifierGuesses)
            {
                StartCoroutine(WaitBeforeEnd());
            }
        }
        else
        {
            playerGuessField.gameObject.GetComponent<Image>().color = Color.red;
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