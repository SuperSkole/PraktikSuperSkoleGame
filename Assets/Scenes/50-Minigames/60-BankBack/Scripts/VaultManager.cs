using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VaultManager : MonoBehaviour
{
    [SerializeField] private int numOfModifiers;

    [SerializeField] TextMeshPro hintField;
    [SerializeField] GameObject modObject;
    [SerializeField] private TMP_InputField playerGuessField;
    [SerializeField] private TMP_InputField modifierGuessField;
    [SerializeField] private GameObject modifierContainer;

    private int modifier;
    private int modifierGuess;
    private int playerGuess;
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        while(modifierContainer.transform.childCount > 0)
        {
            Destroy(modifierContainer.transform.GetChild(0).gameObject);
        }
        for(int i = 0; i < numOfModifiers; i++)
        {
            GameObject modifierObject = Instantiate(modObject);
            modifierObject.transform.SetParent(modifierContainer.transform);
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
        int number;
        bool res = Int32.TryParse(modifierGuessField.text, out number);
        if(res)
        {
            modifierGuess = number;
        }
    }
}
