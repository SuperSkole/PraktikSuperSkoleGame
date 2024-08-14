using System.Collections.Generic;
using CORE.Scripts;
using UnityEngine;

/// <summary>
/// Implementation of IGameMode with the goal of finding either all vowels or all consonants.
/// </summary>
public class FindLetterType : IGameMode
{
    /// <summary>
    /// The correct lettertype
    /// </summary>
    List<char> correctLetterTypes;

    /// <summary>
    /// List of vowels
    /// </summary>
    List<char> vowels = new List<char>(){
        'A', 'E', 'I', 'O', 'U', 'Y', 'Æ', 'Ø', 'Å'
    };

    /// <summary>
    /// List of consonants
    /// </summary>
    List<char> consonants = new List<char>(){
        'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q',
        'R', 'S', 'T', 'V', 'W', 'X', 'Z'
    };

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
    /// the display name of the letter type the player is looking for
    /// </summary>
    string letterType;

    /// <summary>
    /// Whether it is vowels or consonants the player is looking for
    /// </summary>
    bool vowelOrConsonant;


    int completedRounds = 0;

    /// <summary>
    /// Gets the letters for the current game
    /// </summary>
    public void GetLetters()
    {
        if(Random.Range(0, 2) == 0){
            correctLetterTypes = vowels;
            letterType = "vokaler";
            vowelOrConsonant = true;
        }
        else {
            correctLetterTypes = consonants;
            letterType = "konsonanter";
            vowelOrConsonant = false;
        }
        //deactives all current active lettercubes
        foreach (LetterCube lC in activeLetterCubes){
            lC.Deactivate();
        }
        int count = Random.Range(6, 11);
        activeLetterCubes.Clear();
        //finds new letterboxes to be activated and assigns them a random incorrect letter.
        for (int i = 0; i < count; i++){
            char letter = GetLetter(false);
            LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

            //Check to ensure letters dont spawn below the player and that it is not an allready activated lettercube
            while(activeLetterCubes.Contains(potentialCube)){
                potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            }
            activeLetterCubes.Add(potentialCube);
            activeLetterCubes[i].Activate(letter.ToString());
        }
        //finds some new letterboxes and assigns them a correct letter
        for(int i = 0; i < count - 5; i++){
            char letter = GetLetter(true);
            LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

            //Check to ensure letters arent spawned on an allready activated letter cube.
            while(activeLetterCubes.Contains(potentialCube)){
                potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            }
            activeLetterCubes.Add(potentialCube);
            activeLetterCubes[i].Activate(letter.ToString());
            correctLetterCount++;
        }
        boardController.SetAnswerText("Led efter " + letterType + ". Der er " + correctLetterCount + " tilbage.");
    }

    /// <summary>
    /// Helper method to get either a correct or an incorrect letter without having to know which is which at the place its called
    /// </summary>
    /// <param name="correct">Whether it should be a correct or an incorrect letter</param>
    /// <returns></returns>
    private char GetLetter(bool correct){
        if(vowelOrConsonant){
            if(correct){
                return LetterManager.GetRandomVowels(1)[0];
            }
            else{
                return LetterManager.GetRandomConsonants(1)[0];
            }
        }
        else{
            if(correct){
                return LetterManager.GetRandomConsonants(1)[0];
            }
            else {
                return LetterManager.GetRandomVowels(1)[0];
            }
        }
    }

    /// <summary>
    /// Checks if the letter is of the correct type
    /// </summary>
    /// <param name="letter">The letter which should be checked</param>
    /// <returns>Whether the letter is the correct one</returns>
    public bool IsCorrectLetter(string letter)
    {
        return correctLetterTypes.Contains(letter.ToUpper()[0]);
    }

    /// <summary>
    /// Replaces an active lettercube with another one
    /// </summary>
    /// <param name="letter">The letter which should be replaced</param>
    public void ReplaceLetter(LetterCube letter)
    {
        if(IsCorrectLetter(letter.GetLetter())){
            correctLetterCount--;
            boardController.SetAnswerText("Led efter " + letterType + ". Der er " + correctLetterCount + " tilbage.");
        }
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
        if(correctLetterCount > 0){
            newLetter.Activate(GetLetter(false).ToString());

        }
        else{
            completedRounds++;
            if(completedRounds == 5){
                boardController.Won("Du vandt. Du fandt den korrekte bogstavstype 5 gange");
            }
            else{
                GetLetters();
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
        correctLetterCount = 0;
    }

}
