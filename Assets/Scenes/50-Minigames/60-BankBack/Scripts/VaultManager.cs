using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class VaultManager : MonoBehaviour
{
    [SerializeField] private int numOfModifiers;

    [SerializeField] TextMeshPro hintField;
    [SerializeField] GameObject modObject;
    [SerializeField] private TMP_InputField playerGuessField;
    private List<TMP_InputField> modifierGuessFields = new List<TMP_InputField>();
    [SerializeField] private GameObject modifierContainer;

    [SerializeField] private List<int> modifiers = new List<int>();
    private List<char> possibleOperators = new List<char>(){
        '+', '-'
    };
    private List<char> operators = new List<char>();
    private List<int> modifierGuesses = new List<int>();
    private int playerGuess;
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        for(int i = 0; i < numOfModifiers; i++)
        {
            string hint = Random.Range(0, 10000).ToString();
            while(hint.Length < 4)
            {
                hint = "0" + hint;
            }
            hintField.text = hint;
            GameObject modifierObject = Instantiate(modObject);
            modifierObject.transform.SetParent(modifierContainer.transform);
            char op = possibleOperators[Random.Range(0, possibleOperators.Count)];
            modifierObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = op.ToString();
            operators.Add(op);
            modifiers.Add(Random.Range(1, 10));
            modifierGuessFields.Add(modifierObject.transform.GetChild(1).GetComponent<TMP_InputField>());
            modifierGuessFields[i].onValueChanged.AddListener(delegate{UpdateModifierGuess();});
            modifierGuesses.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerGuess()
    {
        int number;
        bool res = Int32.TryParse(playerGuessField.text, out number);
        if(res)
        {
            playerGuess = number;
        }
    }

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

    public void ValidateGuess()
    {
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
    }
}