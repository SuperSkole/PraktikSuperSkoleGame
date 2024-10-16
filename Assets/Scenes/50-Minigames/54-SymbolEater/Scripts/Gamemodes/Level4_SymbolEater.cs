using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._00_Bootstrapper;
using Scenes._10_PlayerScene.Scripts;
using System.Collections.Generic;
using UnityEngine;


namespace Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes
{
    public class Level4_SymbolEater : ISEGameMode
    {

        /// <summary>
        /// Current Word Sound clip
        /// </summary>
        SymbolEaterSoundController currentWordsoundClip;

        IGameRules gameRules;

            /// <summary>
            /// Should be retrieved from Boardcontroller with method SetLetterCubesAndBoard
            /// </summary>
            List<LetterCube> letterCubes;

            List<LetterCube> activeLetterCubes = new List<LetterCube>();

            bool foundLetter;

            BoardController boardController;

            int correctLetters = 0;

            int maxWrongLetters = 10;

            int minWrongLetters = 1;

            int maxCorrectLetters = 5;

            int minCorrectLetters = 1;

            List<string> incorrectAnswers = new List<string>();
            /// <summary>
            /// Activates the given cube
            /// </summary>
            /// <param name="letterCube">The lettercube to be activated</param>
            /// <param name="correct">Whether the symbol should be correct</param>
            public void ActivateCube(LetterCube letterCube, bool correct)
            {
                if (correct)
                {
                    letterCube.Activate(gameRules.GetCorrectAnswer().ToLower(), true);
                    foundLetter = false;
                }
                else
                {
                    string temp = gameRules.GetWrongAnswer();
                    while (incorrectAnswers.Contains(temp) || temp == gameRules.GetCorrectAnswer())
                    {
                        temp = gameRules.GetWrongAnswer();
                    }
                    incorrectAnswers.Add(temp);
                    letterCube.Activate(temp);
                }
            }

            /// <summary>
            /// Gets the shown letters for the current game and the correct one
            /// </summary>
            public void GetSymbols()
            {
                if(!SymbolEaterSoundController.playSound)
                {
                    SymbolEaterSoundController.playSound = true;
                }
                gameRules.SetCorrectAnswer();
                //deactives all current active lettercubes
                foreach (LetterCube lC in activeLetterCubes)
                {
                    lC.Deactivate();
                }
                int count = Random.Range(minWrongLetters, maxWrongLetters + 1);
                activeLetterCubes.Clear();
                incorrectAnswers.Clear();
                //finds new letterboxes to be activated and assigns them a random wrong letter.
                GameModeHelper.ActivateLetterCubes(count, letterCubes, activeLetterCubes, ActivateCube, false, gameRules,boardController.GetPlayer().transform.position);
                //Activates a random lettercube with the correct letter
                GameModeHelper.ActivateLetterCube(letterCubes, activeLetterCubes, ActivateCube, true);
                boardController.SetAnswerText("Tryk [Mellemrum] og find bogstavet.");

                CurrentWordSound();


            }


        /// <summary>
        /// dictitates the current sound, may be changed later
        /// </summary>
        public void CurrentWordSound()
        {
            //Uses currentWord to find the right sound in the Letteraudio Manager

            AudioClip clip = LetterAudioManager.GetAudioClipFromLetter(gameRules.GetCorrectAnswer().ToLower() + "1");

            //checks whether or not its null.
            if (clip != null)
            {
                if (currentWordsoundClip == null)
                    currentWordsoundClip = GameObject.FindObjectOfType<SymbolEaterSoundController>();

                currentWordsoundClip.SetSymbolEaterSound(clip); // sends sound to AudioController
            }
            else
            {
                Debug.LogError("Lydklippet blev ikke fundet!");
            }
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
            /// Checks whether there are still correct letters on the board in order to determine if the current game is over
            /// </summary>
            /// <returns>if there are correct letters on the board</returns>
            public bool IsGameComplete()
            {
                return foundLetter;
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
                    foundLetter = true;
                    PlayerEvents.RaiseAddLetter(letter.GetLetter()[0]);
                }
                //Checks if the current game is over or if it should continue the current game
                if (!GameModeHelper.ReplaceOrVictory(letter, letterCubes, activeLetterCubes, false, ActivateCube, IsGameComplete))
                {
                    

                    //Checks if the player has won. If not a new game is started
                    correctLetters++;
                    boardController.monsterHivemind.IncreaseMonsterSpeed();
                    if (correctLetters < 4)
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
                        boardController.Won("Du vandt. Du fandt det n\u00e6ste bogstav fire gange", multiplier * 1, multiplier * 1);
                    }
                }
            }

            /// <summary>
            /// Sets the game rules used by the game mode
            /// </summary>
            /// <param name="gameRules">The game rules to be used</param>
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
                /*foreach(LetterCube letter in this.letterCubes)
                {
                    letter.randomizeFont = true;
                }*/
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
        /// Not used
        /// </summary>
        public void UpdateLanguageUnitWeight()
        {
            
        }
    }
}