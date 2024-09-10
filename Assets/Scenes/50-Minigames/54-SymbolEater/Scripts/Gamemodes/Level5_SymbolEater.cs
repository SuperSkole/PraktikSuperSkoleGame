using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._00_Bootstrapper;
using Scenes._10_PlayerScene.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


namespace Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes
{
    public class Level5_SymbolEater : ISEGameMode
    {

        /// <summary>
        /// Current Word Sound clip
        /// </summary>
        SymbolEaterSoundController currentWordsoundClip;

        IGameRules gameRules;
        private bool wordsLoaded = false;
        /// <summary>
        /// Should be retrieved from Boardcontroller with method SetLetterCubesAndBoard
        /// </summary>
        List<LetterCube> letterCubes;
     

        List<LetterCube> activeLetterCubes = new List<LetterCube>();
        /// <summary>
        /// a dictionary of textures that been on the map.
        /// </summary>
        Dictionary<string, Texture2D> texture = new Dictionary<string, Texture2D>();

        bool foundLetter;

            BoardController boardController;

            int correctLetters = 0;

            int maxWrongLetters = 10;

            int minWrongLetters = 1;

            int maxCorrectLetters = 3;

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

        public void GetSymbols()
        {



            //Checks if data has been loaded and if it has it begins preparing the board. Otherwise it waits on data being loaded before restarting
            if (DataLoader.IsDataLoaded)
            {

                wordsLoaded = true;
            }
            else
            {
                boardController.StartImageWait(GetSymbols);
            }

            // gets a current word to find images and sound with.

            if (wordsLoaded)
            {
                //deactives all current active lettercubes
                foreach (LetterCube lC in activeLetterCubes)
                {
                    lC.DeactivateImage();
                }
                int count = Random.Range(minWrongLetters, maxWrongLetters + 1);
                activeLetterCubes.Clear();
                //finds new letterboxes to be activated and assigns them a random image. If it selects the correct Image the count for it is increased
                for (int i = 0; i < count; i++)
                {
                    // creates random words from the word list, then creates images to fit those random words.

                    string randoImage = gameRules.GetWrongAnswer();

                    if (!texture.ContainsKey(randoImage))
                    {
                        texture.Add(randoImage, ImageManager.GetImageFromLetter(randoImage));
                    }

                    Texture2D image = texture[randoImage];

                    LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];



                    //Check to ensure Images dont spawn below the player and that it is not an allready activated lettercube
                    while (activeLetterCubes.Contains(potentialCube) && potentialCube.gameObject.transform.position != boardController.GetPlayer().gameObject.transform.position)
                    {
                        potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                    }
                    activeLetterCubes.Add(potentialCube);
                    activeLetterCubes[i].ActivateImage(image, randoImage);
                }
                //creates a random number of correct Images on the board
                int wrongCubeCount = activeLetterCubes.Count;
                count = Random.Range(minCorrectLetters, maxCorrectLetters + 1);
                gameRules.SetCorrectAnswer();
                for (int i = 0; i < count; i++)
                {
                    if (!texture.ContainsKey(gameRules.GetCorrectAnswer()))
                    {
                        texture.Add(gameRules.GetCorrectAnswer(), ImageManager.GetImageFromLetter(gameRules.GetCorrectAnswer()));

                    }
                    // makes a image string from the current word variable, so that we can find it in the files.
                    string image = gameRules.GetCorrectAnswer().ToLower();


                    Texture2D currentImage = texture[gameRules.GetCorrectAnswer()];
                    LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                    //Check to ensure images dont spawn below the player and that it is not an already activated lettercube
                    while (activeLetterCubes.Contains(potentialCube))
                    {
                        potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                    }
                    activeLetterCubes.Add(potentialCube);
                    activeLetterCubes[i + wrongCubeCount].ActivateImage(currentImage, image);

                    
                }
                boardController.SetAnswerText("Tryk [Mellemrum] for at hoere et bogstav, Find det billede der passer til det bogstav");


                //uses the CurrentWordSound 
                CurrentWordSound();

            }
        }




        /// <summary>
        /// dicttates the current sound
        /// </summary>
        public void CurrentWordSound()
        {
            //Uses currentWord to find the right sound in the Letteraudio Manager
            //Using +1 to the name because letter+1 is the way the audio clips for the letter and letter+2 is for the pronounciation of the letter. 

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
                }
                //Checks if the current game is over or if it should continue the current game
                if (!GameModeHelper.ReplaceOrVictory(letter, letterCubes, activeLetterCubes, false, ActivateCube, IsGameComplete))
                {
                   
                    
                    //Checks if the player has won. If not a new game is started
                    correctLetters++;
                    boardController.monsterHivemind.IncreaseMonsterSpeed();
                    if (correctLetters < 10)
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
                        boardController.Won("Du vandt. Du fandt det næste bogstav 10 gange", multiplier * 1, multiplier * 1);
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

        
    }
}