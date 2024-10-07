using System.Collections.Generic;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._00_Bootstrapper;
using UnityEngine;

namespace Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes
{
    /// <summary>
    /// Implementation of IGameMode with the goal of spelling a word based on an image.
    /// </summary>
    public class SpellWordFromImage : ISEGameMode
    {

        int correctWords = 0;

        Queue<char> foundLetters = new Queue<char>();

        int minWrongLetters = 6;

        int maxWrongLetters = 10;

        List<LetterCube> activeLetterCubes = new List<LetterCube>();

        List<LetterCube> letterCubes = new List<LetterCube>();

        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        BoardController boardController;

        IGameRules gameRules;

        string foundWordPart = "";

        string oldWord = "";

        bool wordsLoaded = false;

        int activationIndex = 0;

        /// <summary>
        /// Gets the letters for the current game
        /// </summary>
        public void GetSymbols()
        { 
            foundWordPart = "";
            oldWord = "";
            activationIndex = 0;
            //Checks if data has been loaded and if it has it begins preparing the board. Otherwise it waits on data being loaded before restarting
            if(DataLoader.IsDataLoaded)
            {
                gameRules.SetCorrectAnswer();
                oldWord = gameRules.GetDisplayAnswer();
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
            //If the words are loaded then it starts generating the board
            if(wordsLoaded)
            {
                //deactives all current active lettercubes
                foreach (LetterCube lC in activeLetterCubes)
                {
                    lC.Deactivate();
                }
                int count = Random.Range(minWrongLetters, maxWrongLetters + 1);
                activeLetterCubes.Clear();
                
                //finds new letterboxes to be activated and assigns them a random incorrect letter.
                GameModeHelper.ActivateLetterCubes(count, letterCubes, activeLetterCubes, ActivateCube, false, gameRules, boardController.GetPlayer().transform.position);
                //finds some new letterboxes and assigns them a correct letter
                for(int i = 0; i < gameRules.GetDisplayAnswer().Length; i++)
                {
                    GameModeHelper.ActivateLetterCube(letterCubes, activeLetterCubes, ActivateCube, true);
                }
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
            
            if(gameRules.IsCorrectSymbol(letter) && gameRules.GetCorrectAnswer()[0] != gameRules.GetDisplayAnswer()[gameRules.GetDisplayAnswer().Length - 1])
            {
                foundLetters.Enqueue(letter[0]);
                gameRules.SetCorrectAnswer();
                return true;
            }
            else if(gameRules.IsCorrectSymbol(letter))
            {
                foundLetters.Enqueue(letter.ToLower()[0]);
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
            //Updates the display of letters which the player has already found
            if(foundLetters.Count > 0 && letter.GetLetter() == foundLetters.Peek().ToString())
            {
                foundWordPart += foundLetters.Dequeue();
                boardController.SetAnswerText(foundWordPart);
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
                    boardController.Won("Du vandt. Du stavede rigtigt 5 gange", multiplier * 1, multiplier * 1);
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
        /// sets the game rules of the game. Currently only support SpellWord
        /// </summary>
        /// <param name="gameRules">game rules to be used by the game mode</param>
        public void SetGameRules(IGameRules gameRules)
        {
            this.gameRules = gameRules;
        }

        /// <summary>
        /// Activates the given cube
        /// </summary>
        /// <param name="letterCube">The lettercube to be activated</param>
        /// <param name="correct">Whether the symbol should be correct</param>
        public void ActivateCube(LetterCube letterCube, bool correct)
        {
            if(correct)
            {
                letterCube.Activate(gameRules.GetDisplayAnswer()[activationIndex].ToString());
            }
            else 
            {
                letterCube.Activate(gameRules.GetWrongAnswer());
            }
        }

        /// <summary>
        /// Checks whether the whole word has been found
        /// </summary>
        /// <returns></returns>
        public bool IsGameComplete()
        {
            return foundWordPart.Length == gameRules.GetDisplayAnswer().Length;
        }

        /// <summary>
        /// Not used
        /// </summary>
        public void UpdateLanguageUnitWeight()
        {
            
        }
    }
}