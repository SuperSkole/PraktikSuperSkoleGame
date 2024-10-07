using System.Collections.Generic;
using System.Linq;
using CORE.Scripts;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using CORE.Scripts.Game_Rules;
using Scenes._00_Bootstrapper;

namespace Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes
{
    /// <summary>
    /// Implementation of IGameMode with the goal of finding the first letter of a word
    /// </summary>
    public class FindFirstLetterFromImage : ISEGameMode
    {

        int correctWords = 0;

        int minWrongLetters = 6;

        int maxWrongLetters = 10;

        List<LetterCube> activeLetterCubes = new List<LetterCube>();

        List<LetterCube> letterCubes = new List<LetterCube>();

        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        BoardController boardController;

        IGameRules gameRules;

        bool wordsLoaded = false;

        bool foundLetter;

        /// <summary>
        /// Gets the letters for the current game
        /// </summary>
        public void GetSymbols()
        { 
            //Checks if data has been loaded and if it has it begins preparing the board. Otherwise it waits on data being loaded before restarting
            if(DataLoader.IsDataLoaded)
            {
                gameRules.SetCorrectAnswer();
                if(sprites.ContainsKey(gameRules.GetDisplayAnswer())){
                    boardController.SetImage(sprites[gameRules.GetDisplayAnswer()]);
                }
                else
                {
                    Texture2D texture = ImageManager.GetImageFromWord(gameRules.GetDisplayAnswer());
                    sprites.Add(gameRules.GetDisplayAnswer(), Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f));
                    boardController.SetImage(sprites[gameRules.GetDisplayAnswer()]);
                }
                wordsLoaded = true;
            }
            else 
            {
                boardController.StartImageWait(GetSymbols);
            }
            //If the wordsOrLetters are loaded then it starts generating the board
            if(wordsLoaded)
            {
                foundLetter = false;
                //deactives all current active lettercubes
                foreach (LetterCube lC in activeLetterCubes)
                {
                    lC.Deactivate();
                }
                int count = Random.Range(minWrongLetters, maxWrongLetters + 1);
                activeLetterCubes.Clear();
                
                //finds new letterboxes to be activated and assigns them a random incorrect letter.
                GameModeHelper.ActivateLetterCubes(count, letterCubes, activeLetterCubes, ActivateCube, false, gameRules, boardController.GetPlayer().transform.position);
                //finds a new letterbox and assigns it the correct letter
                GameModeHelper.ActivateLetterCube(letterCubes, activeLetterCubes, ActivateCube, true);
                boardController.SetAnswerText("");
            }
        }
        /// <summary>
        /// Checks if the letter is of the correct type and updates the letter the player should find. 
        /// </summary>
        /// <param name="letter">The letter which should be checked</param>
        /// <returns>Whether the letter is the correct one</returns>
        public bool IsCorrectSymbol(string letter)
        {
            return gameRules.IsCorrectSymbol(letter);
        }

        /// <summary>
        /// Replaces an active lettercube with another one
        /// </summary>
        /// <param name="letter">The letter which should be replaced</param>
        public void ReplaceSymbol(LetterCube letter)
        {
            //Updates the display of letters which the player has already found
            if(IsCorrectSymbol(letter.GetLetter()))
            {
                foundLetter = true;
            }
            //Checks if the game is over. If it is it informs the boardcontroller that the game is over. Otherwise it just restarts with a new word.
            if(!GameModeHelper.ReplaceOrVictory(letter, letterCubes, activeLetterCubes, true, ActivateCube, IsGameComplete))
            {
                correctWords++;
                boardController.monsterHivemind.IncreaseMonsterSpeed();
                if (correctWords == 5)
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
                    gameRules.SetCorrectAnswer();
                    boardController.Won("Du vandt. Du fandt det f\u00f8rste bogstav 5 gange", multiplier * 1, multiplier * 1);
                }
                else
                {
                    boardController.monsterHivemind.ResetSpeed();
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
        }

        /// <summary>
        /// Currently does nothing
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
            minWrongLetters = min;
            maxWrongLetters = max;
        }


        /// <summary>
        /// sets the game rules of the game. Currently only support FindFirstLetter
        /// </summary>
        /// <param name="gameRules">game rules to be used by the game mode</param>
        public void SetGameRules(IGameRules gameRules)
        {
            this.gameRules = gameRules;
        }

        /// <summary>
        /// Activates a lettercube with either a correct or an incorrect letter
        /// </summary>
        /// <param name="letterCube">the lettercube to be activated</param>
        /// <param name="correct">whether the letter should be correct</param>
        public void ActivateCube(LetterCube letterCube, bool correct)
        {
            if(correct)
            {
                letterCube.Activate(gameRules.GetCorrectAnswer());
            }
            else 
            {
                letterCube.Activate(gameRules.GetWrongAnswer());
            }
        }

        /// <summary>
        /// checks if the correct letter has been found
        /// </summary>
        /// <returns>whether foundletter is true</returns>
        public bool IsGameComplete()
        {
            return foundLetter;
        }
    }
}