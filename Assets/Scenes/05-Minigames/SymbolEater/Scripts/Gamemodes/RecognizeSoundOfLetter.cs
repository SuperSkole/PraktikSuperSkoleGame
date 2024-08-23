using CORE.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Minigames.SymbolEater.Scripts.Gamemodes
{
    public class RecognizeSoundOfLetter : IGameMode
    {
        /// <summary>
        /// Change the soundclip to what we need.
        /// </summary>
        SymbolEaterSoundController currentsoundClip;

        IGameRules gameRules = new CORE.Scripts.GameRules.FindLetterType();

        /// <summary>
        /// Should be retrieved from Boardcontroller with method SetLetterCubesAndBoard
        /// </summary>
        List<LetterCube> letterCubes;

        List<LetterCube> activeLetterCubes = new List<LetterCube>();

        int numberOfCorrectLettersOnBoard;

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
            gameRules.SetCorrectAnswer();
            //deactives all current active lettercubes
            foreach (LetterCube lC in activeLetterCubes)
            {
                lC.Deactivate();
            }
            int count = Random.Range(minWrongLetters, maxWrongLetters + 1);
            activeLetterCubes.Clear();
            //finds new letterboxes to be activated and assigns them a random wrong letter.
            for (int i = 0; i < count; i++)
            {
                string letter = gameRules.GetWrongAnswer();
                LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

                //Check to ensure letters dont spawn below the player and that it is not an already activated lettercube
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
                string letter = gameRules.GetCorrectAnswer();
                LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                //Check to ensure letters dont spawn below the player and that it is not an already activated lettercube
                while (activeLetterCubes.Contains(potentialCube))
                {
                    potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                }
                activeLetterCubes.Add(potentialCube);
                activeLetterCubes[i + wrongCubeCount].Activate(letter, true);
                numberOfCorrectLettersOnBoard++;
            }
            boardController.SetAnswerText("Tryk [Mellemrum]s tasten for at lytte til Lyden af bogstavet og vælg det rigtige. " + " Der er " + numberOfCorrectLettersOnBoard + " tilbage.");

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
            return gameRules.IsCorrectSymbol(letter);
        }

        /// <summary>
        /// dictates what the currentLetterSound is.
        /// </summary>
        public void CurrentLetterSound()
        {
            //bruger correctLetter to find the right sound in tempSymbolEatersound in resource foulder
            string audioFileName = gameRules.GetDisplayAnswer().ToLower() + "_audio";

            AudioClip clip = Resources.Load<AudioClip>($"TempGrov�derSound/{audioFileName}");

            //checks whether or not its null.
            if (clip != null)
            {
                if (currentsoundClip == null)
                {
                    currentsoundClip = GameObject.FindObjectOfType<SymbolEaterSoundController>();
                }

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
            //Checks if the symbol on the lettercube is the correct one
            if (IsCorrectSymbol(letter.GetLetter()))
            {
                numberOfCorrectLettersOnBoard--;
                boardController.SetAnswerText("Tryk[Mellemrum]s tasten for at lytte til Lyden af bogstavet og v�lg det rigtige. " + " Der er " + numberOfCorrectLettersOnBoard + " tilbage.");
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
            //Checks if the game should continue. if it should a new random incorrect letter is shown on the new letterblock
            if (numberOfCorrectLettersOnBoard > 0)
            {
                newLetter.Activate(gameRules.GetWrongAnswer());
            }

            //Checks if a new game should be started or if the player has won
            else
            {
                correctLetters++;
                if (correctLetters < 5)
                {
                    GetSymbols();
                }
                else
                {
                    foreach (LetterCube letterCube in activeLetterCubes)
                    {
                        letterCube.Deactivate();
                    }
                    boardController.Won("Du vandt. Du fandt det korrekte bogstav fem gange");
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

        /// <summary>
        /// Temporarily unused until relevant game rules have been implemented
        /// </summary>
        /// <param name="gameRules">Game rules to be be used by the game mode</param>
        public void SetGameRules(IGameRules gameRules)
        {
            this.gameRules = gameRules;
        }
    }
}

