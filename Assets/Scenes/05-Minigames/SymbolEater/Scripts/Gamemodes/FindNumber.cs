
using System.Collections.Generic;
using CORE.Scripts;
using UnityEngine;
using CORE.Scripts.GameRules;

/// <summary>
/// Implementation of IGameMode with the goal of finding the numbers in a number series
/// </summary>
public class FindNumber : IGameMode
{

    int correctSeries = 0;
    int minWrongNumbers = 6;
    int maxWrongNumbers = 10;

    /// <summary>
    /// Should be retrieved from Boardcontroller with method SetLetterCubesAndBoard
    /// </summary>
    List<LetterCube> letterCubes;

    /// <summary>
    /// The lettercubes displaying a letter
    /// </summary>
    List<LetterCube> activeLetterCubes = new List<LetterCube>();

    /// <summary>
    /// The boardController of the current game
    /// </summary>
    BoardController boardController;

    IGameRules gameRules = new FindNumberSeries();

    /// <summary>
    /// Gets the letters for the current game
    /// </summary>
    public void GetSymbols()
    {
        //Sets up the number series
        gameRules.SetCorrectAnswer();
        //deactives all current active lettercubes
        foreach (LetterCube lC in activeLetterCubes)
        {
            lC.Deactivate();
        }
        //sets up variables relating to the number of wrong numbers on the board
        int count = Random.Range(minWrongNumbers, maxWrongNumbers);
        activeLetterCubes.Clear();
        
        //finds new letterboxes to be activated and assigns them a random incorrect number.
        for (int i = 0; i < count; i++)
        {
            LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

            //Check to ensure numbers dont spawn below the player and that it is not an allready activated lettercube
            while(activeLetterCubes.Contains(potentialCube))
            {
                potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            }
            activeLetterCubes.Add(potentialCube);
            activeLetterCubes[i].Activate(gameRules.GetWrongAnswer());
        }
        LetterCube potentialCorrectCube = letterCubes[Random.Range(0, letterCubes.Count)];

        //Check to ensure letters arent spawned on an allready activated letter cube.
        while(activeLetterCubes.Contains(potentialCorrectCube))
        {
            potentialCorrectCube = letterCubes[Random.Range(0, letterCubes.Count)];
        }
        activeLetterCubes.Add(potentialCorrectCube);
        potentialCorrectCube.Activate(gameRules.GetCorrectAnswer());
        
        boardController.SetAnswerText(gameRules.GetDisplayAnswer());
    }


    /// <summary>
    /// Checks if the letter is of the correct type and updates the letter the player should find. 
    /// </summary>
    /// <param name="letter">The letter which should be checked</param>
    /// <returns>Whether the letter is the correct one</returns>
    public bool IsCorrectSymbol(string letter)
    {
        if(gameRules.IsCorrectSymbol(letter))
        {
            
            boardController.SetAnswerText(gameRules.GetDisplayAnswer());
            gameRules.SetCorrectAnswer();
            
            return true;
        }
        else
        {
            return false;
        }
        
    }

    /// <summary>
    /// Replaces an active lettercube with another one
    /// </summary>
    /// <param name="letter">The letter which should be replaced</param>
    public void ReplaceSymbol(LetterCube letter)
    {
        if(letter.active)
        {
            letter.Deactivate();
            activeLetterCubes.Remove(letter);
            LetterCube newLetter;
            //finds a new random letterbox which is not active and is not the one which should be replaced
            while(true)
            {
                newLetter = letterCubes[Random.Range(0, letterCubes.Count)];
                if(newLetter != letter && !activeLetterCubes.Contains(newLetter))
                {
                    break;
                }
            }
            //Checks if the end of the series is reached. If not it activates a new lettercube
            if(!gameRules.SequenceComplete())
            {
                int nL = System.Convert.ToInt32(gameRules.GetWrongAnswer());
                bool containsCorrectNumber = false;
                foreach(LetterCube letterCube in activeLetterCubes)
                {
                    if(gameRules.IsCorrectSymbol(letterCube.GetLetter()))
                    {
                        containsCorrectNumber = true;
                        break;
                    }
                }
                if(!containsCorrectNumber)
                {
                    nL = System.Convert.ToInt32(gameRules.GetCorrectAnswer());
                }
                newLetter.Activate(nL.ToString());
                activeLetterCubes.Add(newLetter);
            }
            //Determines if the player has won. If they have it starts a new game
            else
            {
                correctSeries++;
                if(correctSeries == 3)
                {
                    boardController.Won("Du vandt. Du fandt 3 talrækker");
                }
                else 
                {
                    GetSymbols();
                }
            }
        }
    }

    public void SetGameRules(IGameRules gameRules)
    {
        this.gameRules = gameRules;
    }

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
    /// Currently Does nothing
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SetMinAndMaxCorrectSymbols(int min, int max)
    {
        
    }

    /// <summary>
    /// Sets the minimum and maximum wrong letters which appears on the board
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SetMinAndMaxWrongSymbols(int min, int max)
    {
        minWrongNumbers = min;
        maxWrongNumbers = max;
    }
}