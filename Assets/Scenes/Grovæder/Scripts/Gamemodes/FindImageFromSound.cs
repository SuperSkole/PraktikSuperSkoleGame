using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindImageFromSound : IGameMode
{



    /// <summary>
    /// The correct image
    /// </summary>
    string correctImageWord;

    /// <summary>
    /// List of all imagecubes. Should be retrieved from Boardcontroller with method SetLetterCubesAndBoard
    /// </summary>
    List<LetterCube> letterCubes;

    /// <summary>
    /// The lettercubes displaying a letter
    /// </summary>
    List<LetterCube> activeLetterCubes = new List<LetterCube>();

    /// <summary>
    /// number of correct letters currntly displayed
    /// </summary>
    int correctLetterCount;

    /// <summary>
    /// The boardController of the current game
    /// </summary>
    BoardController boardController;

    int correctLetters = 0;

    int maxWrongLetters = 10;

    int minWrongLetters = 1;

    int maxCorrectLetters = 5;

    int minCorrectLetters = 1;

    /// <summary>
    /// Gets the letters for the current game
    /// </summary>


    public void GetLetters()
    {
        correctImageWord = LetterManager.GetRandomLetters(1)[0].ToString();
        //deactives all current active lettercubes
        foreach (LetterCube lC in activeLetterCubes)
        {
            lC.Deactivate();
        }
        int count = Random.Range(minWrongLetters, maxWrongLetters + 1);
        activeLetterCubes.Clear();
        //finds new letterboxes to be activated and assigns them a random letter. If it selects the correct letter the count for it is increased
        for (int i = 0; i < count; i++)
        {
            string letter = LetterManager.GetRandomLetters(1)[0].ToString();
            while (IsCorrectLetter(letter))
            {
                letter = LetterManager.GetRandomLetters(1)[0].ToString();
            }
            LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

            //Check to ensure letters dont spawn below the player and that it is not an allready activated lettercube
            while (activeLetterCubes.Contains(potentialCube) && potentialCube.gameObject.transform.position != boardController.GetPlayer().gameObject.transform.position)
            {
                potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            }
            activeLetterCubes.Add(potentialCube);
            activeLetterCubes[i].Activate(letter);
        }
        //creates a random number of correct letters on the board
        int wrongCubeCount = activeLetterCubes.Count;
        count = Random.Range(minCorrectLetters, maxCorrectLetters + 1);
        for (int i = 0; i < count; i++)
        {
            string letter = correctImageWord.ToLower();
            LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            //Check to ensure letters dont spawn below the player and that it is not an already activated lettercube
            while (activeLetterCubes.Contains(potentialCube))
            {
                potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            }
            activeLetterCubes.Add(potentialCube);
            activeLetterCubes[i + wrongCubeCount].Activate(letter, true);
            correctLetterCount++;
        }
        boardController.SetAnswerText("Tryk [Mellemrum]s tasten for at lytte til Lyden af bogstavet og vælg det rigtige.");


    }

    public bool IsCorrectLetter(string word)
    {
        return word.ToLower() == correctImageWord.ToLower();
    }

    public void ReplaceLetter(LetterCube letter)
    {
        if (IsCorrectLetter(letter.GetLetter()))
        {
            correctLetterCount--;
            boardController.SetAnswerText("Tryk[Mellemrum]s tasten for at lytte til Lyden af bogstavet og vælg det rigtige. " + " Der er " + correctLetterCount + " tilbage.");
        }
        letter.Deactivate();
        activeLetterCubes.Remove(letter);

        LetterCube newLetter;
        //finds a new random letterbox which is not active and is not the one which should be replaced
        while (true)
        {
            newLetter = letterCubes[Random.Range(0, letterCubes.Count)];

            if (newLetter != letter && !activeLetterCubes.Contains(newLetter))
            {
                break;
            }
        }
        activeLetterCubes.Add(newLetter);
        if (correctLetterCount > 0)
        {
            newLetter.Activate(LetterManager.GetRandomLetters(1)[0].ToString());
            while (newLetter.GetLetter() == correctImageWord)
            {
                newLetter.Activate(LetterManager.GetRandomLetters(1)[0].ToString());
            }
        }
        else
        {
            GetLetters();
        }
    }

    public void SetLetterCubesAndBoard(List<LetterCube> letterCubes, BoardController board)
    {
        this.letterCubes = letterCubes;
        boardController = board;
    }

    public void SetMinAndMaxCorrectLetters(int min, int max)
    {
        minCorrectLetters = min;
        maxCorrectLetters = max;
    }

    /// <summary>
    /// Sets the minimum and maximum wrong letters which appears on the board
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SetMinAndMaxWrongLetters(int min, int max)
    {
        minWrongLetters = min;
        maxWrongLetters = max;
    }

    // Start is called before the first frame update
   
}
