using CORE.Scripts;
using System.Collections.Generic;
using UnityEngine;


namespace Scenes.Minigames.SymbolEater.Scripts.Gamemodes.SpellWordFromSound
{
    public class SpellWordFromSound : IGameMode
    {


        /// <summary>
        /// Current Word Sound clip
        /// </summary>
        SymbolEaterSoundController currentWordsoundClip;

        /// <summary>
        /// The correct word
        /// </summary>
        string word;


        int correctWords = 0;

        int currentIndex;

        char currentLetter;


        /// <summary>
        /// letters which the player has already found
        /// </summary>
        Queue<char> foundLetters = new Queue<char>();

        List<string> words = new List<string>(){
        "Bil", "B�d", "Fly"
        };

        int minWrongLetters = 6;

        int maxWrongLetters = 10;

        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        /// <summary>
        /// List of all lettercubes. Should be retrieved from Boardcontroller with method SetLetterCubesAndBoard
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

        /// <summary>
        /// Gets the letters for the current game
        /// </summary>
        public void GetSymbols()
        {
            currentIndex = 0;
            word = words[Random.Range(0, words.Count)].ToLower();
            currentLetter = word[currentIndex];
            if (sprites.ContainsKey(word))
            {
                boardController.SetImage(sprites[word]);
            }
            else
            {
                sprites.Add(word, Resources.Load<Sprite>("Pictures/" + word + "_image"));
                boardController.SetImage(sprites[word]);
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
                char letter = LetterManager.GetRandomLetters(1)[0];
                while (word.Contains(char.ToLower(letter)))
                {
                    letter = LetterManager.GetRandomLetters(1)[0];
                }
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
            for (int i = 0; i < word.Length; i++)
            {
                char letter = word[i];
                LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

                //Check to ensure letters arent spawned on an allready activated letter cube.
                while (activeLetterCubes.Contains(potentialCube))
                {
                    potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
                }
                activeLetterCubes.Add(potentialCube);
                activeLetterCubes[i].Activate(letter.ToString());
            }
            boardController.SetAnswerText("");
            boardController.SetAnswerText("Tryk [Mellemrum]s tasten for at lytte til Lyden af bogstavet og v�lg det rigtige.");


            /// <summary>
            /// Uses the Word.
            /// </summary>
            CurrentWordSound();
        }


        /// <summary>
        /// Checks if the letter is of the correct type
        /// </summary>
        /// <param name="letter">The letter which should be checked</param>
        /// <returns>Whether the letter is the correct one</returns>
        public bool IsCorrectSymbol(string letter)
        {
            if (currentLetter.ToString() == letter.ToLower() && currentIndex < word.Length - 1)
            {
                currentIndex++;
                foundLetters.Enqueue(currentLetter);
                currentLetter = word[currentIndex];
                return true;
            }
            else if (currentLetter.ToString() == letter.ToLower() && currentIndex == word.Length - 1)
            {
                foundLetters.Enqueue(currentLetter);
                currentIndex++;
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// dictates what the currentLetterSound is from the word.
        /// </summary>
        public void CurrentWordSound()
        {
            //Uses word to find the right sound in tempSymbolEatersound in resource foulder
            string audioFileName = word.ToLower() + "_audio";

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
        /// Replaces LetterCubes on the map when x number of correct ones are found.
        /// </summary>
        public void ReplaceSymbol(LetterCube letter)
        {
            //Updates the display of letters which the player has already found
            if (foundLetters.Count > 0 && letter.GetLetter() == foundLetters.Peek().ToString())
            {
                string foundWordPart = "";
                int j = word.IndexOf(foundLetters.Dequeue());
                for (int i = 0; i < j + 1; i++)
                {
                    if (word.Length > i)
                    {
                        foundWordPart += word[i];
                    }
                }
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
            if (currentIndex < word.Length)
            {
                //currentLetter = word[currentIndex];
                char newLetterCubeValue = LetterManager.GetRandomLetters(1)[0];
                if (word.Contains(oldLetter))
                {
                    newLetterCubeValue = oldLetter[0];
                }

                newLetter.Activate(newLetterCubeValue.ToString());

            }
            else
            {
                correctWords++;
                if (correctWords == 3)
                {
                    boardController.Won("Du vandt. Du stavede rigtigt 3 gange");
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
        /// Temporarily unused until relevant game rules have been implemented
        /// </summary>
        /// <param name="gameRules">game rules to be used by the game mode</param>
        public void SetGameRules(IGameRules gameRules)
        {
            
        }
    }
}
