using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RecognizeSoundOfLetter : IGameMode
{
    /// <summary>
    /// Change the soundclip to what we need.
    /// </summary>
    SymbolEaterSoundController currentsoundClip;

    /// <summary>
    /// The correct letter
    /// </summary>
    string correctLetter;

    /// <summary>
    /// List of all lettercubes. Should be retrieved from Boardcontroller with method SetLetterCubesAndBoard
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
    public void GetSymbols()
    {
        correctLetter = LetterManager.GetRandomLetters(1)[0].ToString();



        //deactives all current active lettercubes
        foreach (LetterCube lC in activeLetterCubes)
        {
            lC.Deactivate();
        }
        int count = Random.Range(1, 11);
        activeLetterCubes.Clear();
        //finds new letterboxes to be activated and assigns them a random letter. If it selects the correct letter the count for it is increased
        for (int i = 0; i < count; i++)
        {
            string letter = LetterManager.GetRandomLetters(1)[0].ToString();
            if (IsCorrectSymbol(letter))
            {
                correctLetterCount++;
            }
            LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

            //Check to ensure letters dont spawn below the player and that it is not an allready activated lettercube
            while (activeLetterCubes.Contains(potentialCube))
            {
                potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            }
            activeLetterCubes.Add(potentialCube);
            activeLetterCubes[i].Activate(letter);
        }
        //finds a random letterbox for the correct letter which has not already been activated
        LetterCube correctLetterBox;
        while (true)
        {
            correctLetterBox = letterCubes[Random.Range(0, letterCubes.Count)];
            if (!activeLetterCubes.Contains(correctLetterBox))
            {
                break;
            }
        }

        


        correctLetterBox.Activate(correctLetter.ToLower(), true);
        correctLetterCount++;
        activeLetterCubes.Add(correctLetterBox);
        boardController.SetAnswerText("Tryk [Mellemrum]s tasten for at lytte til Lyden af bogstavet og vælg det rigtige. " + " Der er " + correctLetterCount + " tilbage.");

        /// <summary>
        /// Uses the Lettersound.
        /// </summary>

        CurrentLetterSound();
    }

    /// <summary>
    /// Checks if the letter is the same as the correct one
    /// </summary>
    /// <param name="letter">The letter which should be checked</param>
    /// <returns>Whether the letter is the correct one</returns>
    public bool IsCorrectSymbol(string letter)
    {
        return letter.ToLower() == correctLetter.ToLower();
    }

    /// <summary>
    /// dictates what the currentLetterSound is.
    /// </summary>
    public void CurrentLetterSound()
    {
        //bruger correctLetter to find the right sound in tempgrovædersound in resource foulder
        string audioFileName = correctLetter.ToLower() + "_audio";
        
        AudioClip clip = Resources.Load<AudioClip>($"TempGrovæderSound/{audioFileName}");

        //checks whether or not its null.
        if (clip != null)
        {
            if (currentsoundClip == null)
                currentsoundClip = GameObject.FindObjectOfType<SymbolEaterSoundController>();
          
            currentsoundClip.SetSymbolEaterSound(clip); // sends sound to AudioController
        }
        else
        {
            Debug.LogError("Lydklippet blev ikke fundet!");
        }
    }

    /// <summary>
    /// Replaces an active lettercube with another one
    /// </summary>
    /// <param name="letter">The letter which should be replaced</param>
    public void ReplaceSymbol(LetterCube letter)
    {
        if (IsCorrectSymbol(letter.GetLetter()))
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
            while (newLetter.GetLetter() == correctLetter)
            {
                newLetter.Activate(LetterManager.GetRandomLetters(1)[0].ToString());
            }
        }
        else
        {
            GetSymbols();
        }
    }

    /// <summary>
    /// Gets the list of lettercubes and the boardController from the boardcontroller
    /// </summary>
    /// <param name="letterCubes">List of lettercubes</param>
    /// <param name="board">the board connected to the lettercubes</param>
    /// <summary>
    /// Gets the list of lettercubes and the boardController from the boardcontroller
    /// </summary>
    /// <param name="letterCubes">List of lettercubes</param>
    /// <param name="board">the board connected to the lettercubes</param>
    public void SetLetterCubesAndBoard(List<LetterCube> letterCubes, BoardController board)
    {
        this.letterCubes = letterCubes;
        boardController = board;
    }

    /// <summary>
    /// Sets the minimum and maximum correct letters which appears on the board
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SetMinAndMaxCorrectSymbols(int min, int max)
    {
        minCorrectLetters = min;
        maxCorrectLetters = max;
    }

    /// <summary>
    /// Sets the minimum and maximum wrong letters which appears on the board
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SetMinAndMaxWrongSymbols(int min, int max)
    {
        minWrongLetters = min;
        maxWrongLetters = max;
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
