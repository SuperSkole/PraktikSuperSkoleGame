using CORE.Scripts;
using Scenes._50_Minigames._67_WordProductionLine.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionLineManager : MonoBehaviour
{

    [SerializeField]
    private ProductionLineObjectPool objectPool;

    [SerializeField]
    private string correctWord = "";

    [SerializeField]
    private string correctLetter;

    private bool correctLetterSpawned = false;

    
    void Start()
    {
        SetCorrectLetter();
    }

   
    void Update()
    {
        
    }


    /// <summary>
    /// gets one letter and checks if its correct or not
    /// </summary>
    /// <returns></returns>
    public string GetLetter()
    {
       
        int randomNumber = Random.Range(0, objectPool.amountToPool - objectPool.amountSpawned);
        if (!correctLetterSpawned && randomNumber == 0)
        {
            correctLetterSpawned = true;
            return correctLetter;
        }
        string randomLetter = LetterManager.GetRandomLetter().ToString();

        while (randomLetter == correctLetter)
        {
            randomLetter = LetterManager.GetRandomLetter().ToString();
        }

        return randomLetter;
    }

    public void WasCorrect(string letter)
    {
        if (letter == correctLetter)
        {
            objectPool.amountSpawned = 0;
            correctLetterSpawned = false;
        }
    }

    /// <summary>
    /// finds correct letter.
    /// </summary>
    private void SetCorrectLetter()
    {
       // correctWord = WordsForImagesManager.GetRandomWordForImage();

        correctWord = "Lars";

        correctLetter = correctWord[0].ToString();
        
    }
}
