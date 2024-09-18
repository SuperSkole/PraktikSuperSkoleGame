using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using System.Collections.Generic;

using UnityEngine;

namespace Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes
{
    public class RecognizeSoundOfLetter : ISEGameMode
    {
        /// <summary>
        /// Change the soundclip to what we need.
        /// </summary>
        SymbolEaterSoundController currentsoundClip;

        IGameRules gameRules = new FindLetterType();

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
            GameModeHelper.ActivateLetterCubes(count, letterCubes, activeLetterCubes, ActivateCube, false, gameRules, boardController.GetPlayer().transform.position);
            //creates a random number of correct letters on the board
            count = Random.Range(minCorrectLetters, maxCorrectLetters + 1);
            numberOfCorrectLettersOnBoard = count;
            GameModeHelper.ActivateLetterCubes(count, letterCubes, activeLetterCubes, ActivateCube, true, gameRules, boardController.GetPlayer().transform.position);

            boardController.SetAnswerText("Tryk [Mellemrum]s tasten for at lytte til Lyden af bogstavet og v\u00e6lg det rigtige. " + " Der er " + numberOfCorrectLettersOnBoard + " tilbage.");

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
            //Finds sound clip from the Audio Clip Manager.
            AudioClip clip = LetterAudioManager.GetAudioClipFromLetter(gameRules.GetDisplayAnswer().ToLower() + 2);



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
                boardController.SetAnswerText("Tryk[Mellemrum]s tasten for at lytte til Lyden af bogstavet og v\u00e6lg det rigtige. " + " Der er " + numberOfCorrectLettersOnBoard + " tilbage.");
            }
            //Checks if the current game is over or if it should continue the current game
            if (!GameModeHelper.ReplaceOrVictory(letter, letterCubes, activeLetterCubes, false, ActivateCube, IsGameComplete))
            //Checks if a new game should be started or if the player has won 
            {
                correctLetters++;
                boardController.monsterHivemind.IncreaseMonsterSpeed();
                if (correctLetters < 5)
                {
                    boardController.monsterHivemind.ResetSpeed();
                    GetSymbols();
                }
                else
                {
                    foreach (LetterCube letterCube in activeLetterCubes)
                    {
                        letterCube.Deactivate();
                    }
                    //Calculates the multiplier for the xp reward. All values are temporary
                    int multiplier = 1;
                    switch (boardController.difficultyManager.diffculty)
                    {
                        case DiffcultyPreset.CUSTOM:
                        case DiffcultyPreset.EASY:
                            multiplier = 1;
                            break;
                        case DiffcultyPreset.MEDIUM:
                            multiplier = 2;
                            break;
                        case DiffcultyPreset.HARD:
                            multiplier = 4;
                            break;
                    }
                    boardController.Won("Du vandt. Du fandt det korrekte bogstav fem gange", 1 * multiplier, 1 * multiplier);

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

        /// <summary>
        /// Currently not implemented
        /// </summary>
        /// <param name="letterCube"></param>
        /// <param name="correct"></param>
        public void ActivateCube(LetterCube letterCube, bool correct)
        {
            if (correct)
            {
                letterCube.Activate(gameRules.GetCorrectAnswer().ToLower(), true);
            }
            else
            {
                letterCube.Activate(gameRules.GetWrongAnswer());
            }
        }

        /// <summary>
        /// Currently not implemented
        /// </summary>
        /// <returns></returns>
        public bool IsGameComplete()
        {
            if (numberOfCorrectLettersOnBoard == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}