using CORE.Scripts.Game_Rules;
using System.Collections.Generic;
using UnityEngine;


namespace Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes
{
    /// <summary>
    /// Implementation of IGameMode with the goal of finding the numbers in a number series
    /// </summary>
    public class FindNumber : ISEGameMode
    {

        int correctSeries = 0;
        int minWrongNumbers = 6;
        int maxWrongNumbers = 10;

        string currentNumber;

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

        bool setup = false;

        /// <summary>
        /// Activates the given cube with a value depending on if it needs to be correct or not
        /// </summary>
        /// <param name="letterCube">the letter cube to be activated</param>
        /// <param name="correct">whether it needs to be correct</param>
        public void ActivateCube(LetterCube letterCube, bool correct)
        {
            if(correct)
            {
                letterCube.Activate(gameRules.GetCorrectAnswer(), true);
            }
            else
            {
                if(setup)
                {
                    letterCube.Activate(gameRules.GetWrongAnswer());
                }
                else
                {
                    bool foundCorrectAnswer = false;
                    foreach(LetterCube letter in activeLetterCubes)
                    {
                        if(letter.GetLetter() == gameRules.GetCorrectAnswer())
                        {
                            foundCorrectAnswer = true;
                            break;
                        }
                    }
                    if(!foundCorrectAnswer)
                    {
                        letterCube.Activate(gameRules.GetCorrectAnswer());
                    }
                    else 
                    {
                        letterCube.Activate(gameRules.GetWrongAnswer());
                    }
                }
            }
        }

        /// <summary>
        /// Gets the letters for the current game
        /// </summary>
        public void GetSymbols()
        {
            setup = true;
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
            GameModeHelper.ActivateLetterCubes(count, letterCubes, activeLetterCubes, ActivateCube, false, gameRules, boardController.GetPlayer().transform.position);
            GameModeHelper.ActivateLetterCube(letterCubes, activeLetterCubes, ActivateCube, true);
            
            boardController.SetAnswerText(gameRules.GetDisplayAnswer());
            setup = false;
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
                if(!gameRules.SequenceComplete())
                {
                    gameRules.SetCorrectAnswer();
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }
        

        /// <summary>
        /// Returns whether the current numberseries is complete
        /// </summary>
        /// <returns>Whether the current numberseries is complete</returns>
        public bool IsGameComplete()
        {
            if(gameRules.SequenceComplete() && gameRules.GetCorrectAnswer() == currentNumber)
            {
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
            currentNumber = letter.GetLetter();
            if(!GameModeHelper.ReplaceOrVictory(letter, letterCubes, activeLetterCubes, false, ActivateCube, IsGameComplete)){
                boardController.monsterHivemind.IncreaseMonsterSpeed();
                correctSeries++;
                    if(correctSeries == 3)
                    {
                        //Calculates the multiplier for the xp reward. All values are temporary
                        int multiplier = 1;
                        switch(boardController.difficultyManager.diffculty){
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
                        boardController.Won("Du vandt. Du fandt 3 talrækker", 1 * multiplier, 1 * multiplier);
                    }
                    else 
                    {
                    boardController.monsterHivemind.ResetSpeed();
                    GetSymbols();
                    }
            }
        }

        /// <summary>
        /// Sets the game rules used by the board
        /// </summary>
        /// <param name="gameRules">The game rules used by the game mode</param>
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
            minWrongNumbers = min * 2;
            maxWrongNumbers = max * 2;
        }
    }
}