using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of IGameMode with the goal of finding all variants of the correct letter on the board.
/// </summary>
public class FindCorrectLetter : MonoBehaviour, IGameMode
{
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

    /// <summary>
    /// Gets the letters for the current game
    /// </summary>
    public void GetLetters()
    {
        //correctLetter = LetterAndWordCollections.GetRandomLetters(1)[0].ToString();
        //deactives all current active lettercubes
        foreach (LetterCube lC in activeLetterCubes){
            lC.Deactivate();
        }
        int count = UnityEngine.Random.Range(1, 11);
        activeLetterCubes.Clear();
        //finds new letterboxes to be activated and assigns them a random letter. If it selects the correct letter the count for it is increased
        for (int i = 0; i < count; i++){
            string letter = null;//LetterAndWordCollections.GetRandomLetters(1)[0].ToString();
            if(IsCorrectLetter(letter)){
                correctLetterCount++;
            }
            LetterCube potentialCube = letterCubes[UnityEngine.Random.Range(0, letterCubes.Count)];

            //Check to ensure letters dont spawn below the player and that it is not an allready activated lettercube
            while(activeLetterCubes.Contains(potentialCube)){
                potentialCube = letterCubes[UnityEngine.Random.Range(0, letterCubes.Count)];
            }
            activeLetterCubes.Add(potentialCube);
            activeLetterCubes[i].Activate(letter);
        }
        //finds a random letterbox for the correct letter which has not already been activated
        LetterCube correctLetterBox;
        while(true){
            correctLetterBox = letterCubes[UnityEngine.Random.Range(0, letterCubes.Count)];
            if(!activeLetterCubes.Contains(correctLetterBox)){
                break;
            }
        }
        correctLetterBox.Activate(correctLetter.ToLower(), true);
        correctLetterCount++;
        activeLetterCubes.Add(correctLetterBox);
        boardController.SetAnswerText("Led efter " + correctLetter + ". Der er " + correctLetterCount + " tilbage.");
    }

    /// <summary>
    /// Checks if the letter is the same as the correct one
    /// </summary>
    /// <param name="letter">The letter which should be checked</param>
    /// <returns>Whether the letter is the correct one</returns>
    public bool IsCorrectLetter(string letter)
    {
        return letter.ToLower() == correctLetter.ToLower();
    }

    /// <summary>
    /// Replaces an active lettercube with another one
    /// </summary>
    /// <param name="letter">The letter which should be replaced</param>
    public void ReplaceLetter(LetterCube letter)
    {
        if(IsCorrectLetter(letter.GetLetter())){
            correctLetterCount--;
            boardController.SetAnswerText("Led efter " + correctLetter + ". Der er " + correctLetterCount + " tilbage.");
        }
        letter.Deactivate();
        activeLetterCubes.Remove(letter);
        
        LetterCube newLetter;
        //finds a new random letterbox which is not active and is not the one which should be replaced
        while(true){
            newLetter = letterCubes[UnityEngine.Random.Range(0, letterCubes.Count)];
            if(newLetter != letter && !activeLetterCubes.Contains(newLetter)){
                break;
            }
        }
        activeLetterCubes.Add(newLetter);
        if(correctLetterCount > 0){
            //newLetter.Activate(LetterAndWordCollections.GetRandomLetters(1)[0].ToString());
            while(newLetter.GetLetter() == correctLetter){
                //newLetter.Activate(LetterAndWordCollections.GetRandomLetters(1)[0].ToString());
            }
        }
        else{
            GetLetters();
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
