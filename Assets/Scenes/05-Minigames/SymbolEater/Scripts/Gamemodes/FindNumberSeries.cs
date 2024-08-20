
using System.Collections.Generic;
using CORE.Scripts;
using UnityEngine;

/// <summary>
/// Implementation of IGameMode with the goal of finding the numbers in a number series
/// </summary>
public class FindNumberSeries : IGameMode
{
    /// <summary>
    /// The number of completed number series
    /// </summary>
    int correctSeries = 0;

    /// <summary>
    /// The starting point of the series
    /// </summary>
    int startNum;

    /// <summary>
    /// The end point of the series
    /// </summary>
    int endNum;

    /// <summary>
    /// The current number the player is looking for
    /// </summary>
    int currentNum;

    /// <summary>
    /// The minimum value of wrong numbers
    /// </summary>
    int minWrongNumber;

    /// <summary>
    /// The maximum value of wrong numbers
    /// </summary>
    int maxWrongNumber;

    /// <summary>
    /// The minmum wrong numbers to appear
    /// </summary>
    int minWrongNumbers = 6;

    /// <summary>
    /// The maximum wrong numbers to appear
    /// </summary>
    int maxWrongNumbers = 10;

    /// <summary>
    /// Minimum length of the number series
    /// </summary>
    int minLength;

    /// <summary>
    /// Maximum length of the number series
    /// </summary>
    int maxLength;

    
    /// <summary>
    /// List of all lettercubes. Should be retrieved from Boardcontroller with method SetLetterCubesAndBoard
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

    /// <summary>
    /// Gets the letters for the current game
    /// </summary>
    public void GetSymbols()
    {
        startNum = Random.Range(1, 95);
        currentNum = startNum;
        endNum = Random.Range(startNum + minLength, startNum + maxLength);
        //deactives all current active lettercubes
        foreach (LetterCube lC in activeLetterCubes){
            lC.Deactivate();
        }
        int count = Random.Range(minWrongNumbers, maxWrongNumbers);
        activeLetterCubes.Clear();
        maxWrongNumber = endNum + 5;
        minWrongNumber = startNum - 5;

        if(maxWrongNumber > 99){
            maxWrongNumber = 99;
        }
        if(minWrongNumber < 1){
            minWrongNumber = 1;
        }
        if(count + endNum - startNum > 100){
            count = (100 - endNum - startNum)/2;
        }
        //finds new letterboxes to be activated and assigns them a random incorrect letter.
        for (int i = 0; i < count; i++){
            LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

            //Check to ensure letters dont spawn below the player and that it is not an allready activated lettercube
            while(activeLetterCubes.Contains(potentialCube)){
                potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            }
            activeLetterCubes.Add(potentialCube);
            activeLetterCubes[i].Activate(Random.Range(minWrongNumber, maxWrongNumbers + 1).ToString());
        }
        //finds some new letterboxes and assigns them a correct letter
        for(int i = startNum; i < endNum + 1; i++){
            int number = i;
            LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

            //Check to ensure letters arent spawned on an allready activated letter cube.
            while(activeLetterCubes.Contains(potentialCube)){
                potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            }
            activeLetterCubes.Add(potentialCube);
            potentialCube.Activate(number.ToString());
        }
        boardController.SetAnswerText("Find tallene fra " + startNum + " til " + endNum + " i rækkefølge. Du har foreløbigt ikke fundet nogen");
    }


    /// <summary>
    /// Checks if the letter is of the correct type and updates the letter the player should find. 
    /// </summary>
    /// <param name="letter">The letter which should be checked</param>
    /// <returns>Whether the letter is the correct one</returns>
    public bool IsCorrectSymbol(string letter)
    {
        if(letter == currentNum.ToString()){
            boardController.SetAnswerText("Find tallene fra " + startNum + " til " + endNum + " i rækkefølge. Du har foreløbigt fundet tallene op til " + letter);
            currentNum++;
            return true;

        }
        else{
            return false;
        }
        
    }

    /// <summary>
    /// Replaces an active lettercube with another one
    /// </summary>
    /// <param name="letter">The letter which should be replaced</param>
    public void ReplaceSymbol(LetterCube letter)
    {
        if(letter.active){
            int oldNumber = System.Convert.ToInt32(letter.GetLetter());

            letter.Deactivate();
            activeLetterCubes.Remove(letter);
            
            LetterCube newLetter;
            //finds a new random letterbox which is not active and is not the one which should be replaced
            while(true){
                newLetter = letterCubes[Random.Range(0, letterCubes.Count)];
                if(newLetter != letter && !activeLetterCubes.Contains(newLetter)){
                    break;
                }
            }
            activeLetterCubes.Add(newLetter);
            if(currentNum <= endNum){
                int nL = Random.Range(minWrongNumber, maxWrongNumber + 1);
                //currentLetter = word[currentIndex];
                if(oldNumber > currentNum && oldNumber < endNum){
                    nL = oldNumber;
                }
                newLetter.Activate(nL.ToString());

            }
            else{
                correctSeries++;
                if(correctSeries == 3){
                    boardController.Won("Du vandt. Du fandt 3 talrækker");
                }
                else {
                    GetSymbols();
                }
            }
        }
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
    /// Determines the minimum and maximum length of the number series
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SetMinAndMaxCorrectSymbols(int min, int max)
    {
        minLength = min;
        maxLength = max;
        
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