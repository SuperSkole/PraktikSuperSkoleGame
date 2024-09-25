using System.Collections;
using System.Collections.Generic;
using System.Threading;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using UnityEngine;

namespace Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes
{
    /// <summary>
    /// Implementation of ISEGamemode where the player should spell incorrect two letter words
    /// </summary>
    public class SpellIncorrectWord : ISEGameMode
    {
        IGameRules gamerules;
        List<LetterCube> lettercubes;
        List<LetterCube> activeLettercubes = new List<LetterCube>();
        BoardController board;
        int minWrongLetters;

        int maxWrongLetters;

        string currentWord = "";

        int completedWords = 0;

        int vowelCount = 0;

        /// <summary>
        /// Activates a cube as either a vowel or a consonant
        /// </summary>
        /// <param name="letterCube">the letter cube to be activated</param>
        /// <param name="correct">whether it is a vowel or a consonant. true = vowel</param>
        public void ActivateCube(LetterCube letterCube, bool correct)
        {
            if(correct)
            {
                letterCube.Activate(gamerules.GetCorrectAnswer());
                vowelCount++;
            }
            else if(vowelCount < 3)
            {
                letterCube.Activate(gamerules.GetCorrectAnswer());
                vowelCount++;
            }
            else
            {
                letterCube.Activate(gamerules.GetWrongAnswer());
            }
            
        }

        /// <summary>
        /// Activates a random number of lettercubes.
        /// </summary>
        public void GetSymbols()
        {
            int count = Random.Range(minWrongLetters, maxWrongLetters);
            GameModeHelper.ActivateLetterCubes(count, lettercubes, activeLettercubes, ActivateCube, false, gamerules, board.GetPlayer().transform.position);
            count = Random.Range(minWrongLetters, maxWrongLetters);
            GameModeHelper.ActivateLetterCubes(count, lettercubes, activeLettercubes, ActivateCube, true, gamerules, board.GetPlayer().transform.position);
            board.SetAnswerText("Lav "+ (5 - completedWords) +" vr\u00f8vleord");
        }

        /// <summary>
        /// Checks whether the word is completed and in that case checks if it is an incorrect word
        /// </summary>
        /// <param name="symbol">the symbol to be added to the word</param>
        /// <returns>Either that the letter could be added or whether the letters the player has found is an incorrect word</returns>
        public bool IsCorrectSymbol(string symbol)
        {
            //Adds symbol to the word if the word is empty
            if(currentWord.Length == 0)
            {
                currentWord = symbol;
                board.SetAnswerText("Lav "+ (5 - completedWords) +" vr\u00f8vleord.\n" + currentWord);
                return true;
            }
            //Checks whether the letters form an incorrect word if it is the second letter to be added
            else if(currentWord.Length == 1)
            {
                currentWord += symbol;
                board.SetAnswerText("Lav "+ (5 - completedWords) +" vr\u00f8vleord.");
                return gamerules.IsCorrectSymbol(currentWord);
                
            }
            //Backup in case the player walks over a lettercube before replacesymbol have been run a second time
            else 
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the player has created a new word.
        /// </summary>
        /// <returns>whether completedwords equals 5</returns>
        public bool IsGameComplete()
        {
            return completedWords == 5;
        }

        /// <summary>
        /// Checks whether various variables should be updated
        /// </summary>
        /// <param name="symbol"></param>
        public void ReplaceSymbol(LetterCube symbol)
        {
            //Checks if the word is two letters long and if it is an incorrect word.
            if(currentWord.Length == 2 && gamerules.IsCorrectSymbol(currentWord))
            {
                completedWords++;
                board.SetAnswerText("Lav "+ (5 - completedWords) +" vr\u00f8vleord med mindst en vokal");
            }
            //Resets the word if it is two letters long
            if(currentWord.Length == 2)
            {
                currentWord = "";
            }
            if(LetterManager.GetDanishVowels().Contains(symbol.GetLetter().ToUpper()[0]))
            {
                vowelCount --;
            }
            //Checks if the game should end or a new letter should be placed on the board
            if(!GameModeHelper.ReplaceOrVictory(symbol, lettercubes, activeLettercubes, false, ActivateCube, IsGameComplete))
            {
                //Calculates the multiplier for the xp reward. All values are temporary
                int multiplier = 1;
                switch(board.difficultyManager.diffculty){
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
                board.Won("Du vandt. Du fik lavet 5 nye ord", 1 * multiplier, 1 * multiplier);
            }
        }

        /// <summary>
        /// Sets the game rules of the game. Currently supports FindIncorrectWords
        /// </summary>
        /// <param name="gameRules">the gamerules to be used</param>
        public void SetGameRules(IGameRules gameRules)
        {
            this.gamerules = gameRules;
        }

        /// <summary>
        /// Gets the list of lettercubes and the boardController from the boardcontroller
        /// </summary>
        /// <param name="letterCubes">List of lettercubes</param>
        /// <param name="board">the board connected to the lettercubes</param>
        public void SetLetterCubesAndBoard(List<LetterCube> letterCubes, BoardController board)
        {
            this.lettercubes = letterCubes;
            this.board = board;
        }

        /// <summary>
        /// Not used
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
    }
}