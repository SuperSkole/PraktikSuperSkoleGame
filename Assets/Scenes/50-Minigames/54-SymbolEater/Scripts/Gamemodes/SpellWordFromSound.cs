using System.Collections.Generic;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._00_Bootstrapper;
using UnityEngine;

namespace Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes
{
    public class SpellWordFromSound : ISEGameMode
    {
        /// <summary>
        /// Current Word Sound clip
        /// </summary>
        SymbolEaterSoundController currentWordsoundClip;

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

        /// <summary>
        /// Gets the letters for the current game
        /// </summary>
        public void GetSymbols()
        {
            foundWordPart = "";
            oldWord = "";
            //Checks if data has been loaded and if it has it begins preparing the board. Otherwise it waits on data being loaded before restarting
            if (DataLoader.IsDataLoaded)
            {
                gameRules.SetCorrectAnswer();
                oldWord = gameRules.GetDisplayAnswer();
                if (!sprites.ContainsKey(gameRules.GetDisplayAnswer()))
                {
                    boardController.SetImage(sprites[gameRules.GetDisplayAnswer()]);
                }
                else
                {
                    Texture2D texture = ImageManager.GetImageFromWord(gameRules.GetDisplayAnswer());
                    sprites.Add(gameRules.GetDisplayAnswer(), Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f));
                    
                }
                wordsLoaded = true;
            }
            else
            {
                boardController.StartImageWait(GetSymbols);
            }
            //If the words are loaded then it starts generating the board
            if (wordsLoaded)
            {
                //deactives all current active lettercubes
                foreach (LetterCube lC in activeLetterCubes)
                {
                    lC.Deactivate();
                }
                int count = Random.Range(minWrongLetters, maxWrongLetters + 1);
                Debug.Log(count);
                activeLetterCubes.Clear();
                //finds new letterboxes to be activated and assigns them a random incorrect letter.
                for (int i = 0; i < count; i++)
                {
                    string letter = gameRules.GetWrongAnswer();
                    LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

                    //Check to ensure the potiential cube has not already been activated
                    while (activeLetterCubes.Contains(potentialCube))
                    {
                        potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                    }
                    activeLetterCubes.Add(potentialCube);
                    potentialCube.Activate(letter);

                }
                //finds some new letterboxes and assigns them a correct letter
                for (int i = 0; i < gameRules.GetDisplayAnswer().Length; i++)
                {
                    string letter = gameRules.GetDisplayAnswer()[i].ToString();
                    LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

                    //Check to ensure the potiential cube has not already been activated
                    while (activeLetterCubes.Contains(potentialCube))
                    {
                        potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                    }
                    activeLetterCubes.Add(potentialCube);
                    activeLetterCubes[i].Activate(letter.ToString());
                }
                boardController.SetAnswerText("");
            }

            //uses the CurrentWordSound 
            //CurrentWordSound();

        }


        /// <summary>
        /// Checks if the letter is of the correct type and updates the letter the player should find. 
        /// </summary>
        /// <param name="letter">The letter which should be checked</param>
        /// <returns>Whether the letter is the correct one</returns>
        public bool IsCorrectSymbol(string letter)
        {

            if (gameRules.IsCorrectSymbol(letter) && gameRules.GetCorrectAnswer()[0] != gameRules.GetDisplayAnswer()[gameRules.GetDisplayAnswer().Length - 1])
            {
                foundLetters.Enqueue(letter[0]);
                gameRules.SetCorrectAnswer();
                return true;
            }
            else if (gameRules.IsCorrectSymbol(letter))
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
        /// dictitates the current sound, may be changed later
        /// </summary>
        public void CurrentWordSound()
        {
            //Uses currentWord to find the right sound in tempgrovædersound in resource foulder
            string audioFileName = gameRules.GetDisplayAnswer() + "_audio";

            AudioClip clip = Resources.Load<AudioClip>($"AudioWords/{audioFileName}");

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
        /// Replaces an active lettercube with another one
        /// </summary>
        /// <param name="letter">The letter which should be replaced</param>
        public void ReplaceSymbol(LetterCube letter)
        {
            //Updates the display of letters which the player has already found
            if (foundLetters.Count > 0 && letter.GetLetter() == foundLetters.Peek().ToString())
            {
                foundWordPart += foundLetters.Dequeue();
                boardController.SetAnswerText(foundWordPart);
            }
            string oldLetter = letter.GetLetter();
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
            //Checks if the word has been completed. If it hasnt a new random letter is placed on the board unless the old letter is in the currrent word, in which case the same letter is used
            if (!gameRules.SequenceComplete() || oldLetter[0] != oldWord[oldWord.Length - 1])
            {
                string newLettercubeValue = gameRules.GetWrongAnswer();
                if (gameRules.GetDisplayAnswer().Contains(oldLetter))
                {
                    newLettercubeValue = oldLetter;
                }

                newLetter.Activate(newLettercubeValue);

            }
            //Checks if the game is over. If it is it informs the boardcontroller that the game is over. Otherwise it just restarts with a new word.
            else
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
        /// Currently not implemented
        /// </summary>
        /// <param name="letterCube"></param>
        /// <param name="correct"></param>
        public void ActivateCube(LetterCube letterCube, bool correct)
        {
            
        }

        /// <summary>
        /// Currently not implemented
        /// </summary>
        /// <returns></returns>
        public bool IsGameComplete()
        {
            return false;
        }
    }

}