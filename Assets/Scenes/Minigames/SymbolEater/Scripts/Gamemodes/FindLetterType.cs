using System.Collections.Generic;
using CORE.Scripts;
using UnityEngine;



namespace Scenes.Minigames.SymbolEater.Scripts.Gamemodes
{

    /// <summary>
    /// Implementation of IGameMode with the goal of finding either all vowels or all consonants.
    /// </summary>
    public class FindLetterType : IGameMode
    {

        List<char> correctLetterTypes;


        List<char> vowels = new List<char>();


        List<char> consonants = new List<char>();

        /// <summary>
        /// Should be retrieved from Boardcontroller with method SetLetterCubesAndBoard
        /// </summary>
        List<LetterCube> letterCubes;

        List<LetterCube> activeLetterCubes = new List<LetterCube>();


        int numberOfCorrectLettersOnBoard;

        BoardController boardController;

        string correctLetterTypeDisplayName;

        /// <summary>
        /// true = vowel, false = consonant
        /// </summary>
        bool isThecorretLetterTypeVowelOrConsonant;


        int completedRounds = 0;

        int minWrongLetters = 6;
        int maxWrongLetters = 10;

        int minCorrectLetters = 1;
        int maxCorrectLetters = 5;

        /// <summary>
        /// Gets the letters for the current game and sets up the correct letter type
        /// </summary>
        public void GetSymbols()
        {
            //Determines which lettertype is the correct one and when sets up the various variables for it. Also retrieves the relevant letter list if it has not been done yet.
            if (Random.Range(0, 2) == 0)
            {
                if (vowels.Count == 0)
                {
                    vowels = LetterManager.GetDanishVowels();
                }
                correctLetterTypes = vowels;
                correctLetterTypeDisplayName = "vokaler";
                isThecorretLetterTypeVowelOrConsonant = true;
            }
            else
            {
                if (consonants.Count == 0)
                {
                    consonants = LetterManager.GetConsonants();
                }
                correctLetterTypes = consonants;
                correctLetterTypeDisplayName = "konsonanter";
                isThecorretLetterTypeVowelOrConsonant = false;
            }
            //deactives all current active lettercubes
            foreach (LetterCube lC in activeLetterCubes)
            {
                lC.Deactivate();
            }
            int count = Random.Range(minWrongLetters, maxWrongLetters);
            activeLetterCubes.Clear();
            //finds new letterboxes to be activated and assigns them a random incorrect letter.
            for (int i = 0; i < count; i++)
            {
                char letter = GetLetter(false);
                LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

                //Check to ensure letters dont spawn below the player and that it is not an allready activated lettercube
                while (activeLetterCubes.Contains(potentialCube))
                {
                    potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                }
                activeLetterCubes.Add(potentialCube);
                activeLetterCubes[i].Activate(letter.ToString());
            }
            //finds some new letterboxes and assigns them a correct letter
            for (int i = 0; i < Random.Range(minCorrectLetters, maxCorrectLetters); i++)
            {
                char letter = GetLetter(true);
                LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

                //Check to ensure letters arent spawned on an allready activated letter cube.
                while (activeLetterCubes.Contains(potentialCube))
                {
                    potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                }
                activeLetterCubes.Add(potentialCube);
                activeLetterCubes[i].Activate(letter.ToString());
                numberOfCorrectLettersOnBoard++;
            }
            boardController.SetAnswerText("Led efter " + correctLetterTypeDisplayName + ". Der er " + numberOfCorrectLettersOnBoard + " tilbage.");
        }

        /// <summary>
        /// Helper method to get either a correct or an incorrect letter without having to know which is which at the place its called
        /// </summary>
        /// <param name="correct">Whether it should be a correct or an incorrect letter</param>
        /// <returns>a random letter</returns>
        private char GetLetter(bool correct)
        {
            if (isThecorretLetterTypeVowelOrConsonant)
            {
                if (correct)
                {
                    return LetterManager.GetRandomVowel();
                }
                else
                {
                    return LetterManager.GetRandomConsonant();
                }
            }
            else
            {
                if (correct)
                {
                    return LetterManager.GetRandomConsonant();
                }
                else
                {
                    return LetterManager.GetRandomVowel();
                }
            }
        }

        /// <summary>
        /// Checks if the letter is of the correct type
        /// </summary>
        /// <param name="letter">The letter which should be checked</param>
        /// <returns>Whether the letter is the correct one</returns>
        public bool IsCorrectSymbol(string letter)
        {
            return correctLetterTypes.Contains(letter.ToUpper()[0]);
        }

        /// <summary>
        /// Replaces an active lettercube with another one
        /// </summary>
        /// <param name="letter">The letter which should be replaced</param>
        public void ReplaceSymbol(LetterCube letter)
        {
            //Checks if the letter is the correct type and counts down the remaing correct letters on the board
            if (IsCorrectSymbol(letter.GetLetter()))
            {
                numberOfCorrectLettersOnBoard--;
                boardController.SetAnswerText("Led efter " + correctLetterTypeDisplayName + ". Der er " + numberOfCorrectLettersOnBoard + " tilbage.");
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
            //Determines if the game is over and if it isnt activates the new letterbox
            if (numberOfCorrectLettersOnBoard > 0)
            {
                newLetter.Activate(GetLetter(false).ToString());

            }
            //checks if the player has completed the game enough times to have won and ends it in that case. Otherwise it starts a new game
            else
            {
                completedRounds++;
                if (completedRounds == 5)
                {
                    boardController.Won("Du vandt. Du fandt den korrekte bogstavstype 5 gange");
                }
                else
                {
                    GetSymbols();
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
            numberOfCorrectLettersOnBoard = 0;
        }

        public void SetMinAndMaxWrongSymbols(int min, int max)
        {
            minWrongLetters = min;
            maxWrongLetters = max;
        }

        public void SetMinAndMaxCorrectSymbols(int min, int max)
        {
            minCorrectLetters = min;
            maxCorrectLetters = max;
        }
    }

}