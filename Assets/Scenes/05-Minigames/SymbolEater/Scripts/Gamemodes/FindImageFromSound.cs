using CORE.Scripts;
using System.Collections.Generic;

using UnityEngine;


namespace Scenes.Minigames.SymbolEater.Scripts.Gamemodes
{
    public class FindImageFromSound : IGameMode
    {

        /// <summary>
        /// Current Word Sound clip
        /// </summary>
        SymbolEaterSoundController currentWordsoundClip;

        /// <summary>
        /// The correct word
        /// </summary>
        

        

        private bool wordsLoaded = false;


        /// <summary>
        /// List of all lettercubes. Should be retrieved from Boardcontroller with method SetLetterCubesAndBoard
        /// </summary>
        List<LetterCube> letterCubes;

        /// <summary>
        /// The lettercubes displaying a letter
        /// </summary>
        List<LetterCube> activeLetterCubes = new List<LetterCube>();


        /// <summary>
        /// a dictionary of textures that been on the map.
        /// </summary>
        Dictionary<string, Texture2D> texture = new Dictionary<string, Texture2D>();

        /// <summary>
        /// number of correct letters currntly displayed
        /// </summary>
        int correctLetterCount;

        /// <summary>
        /// The boardController of the current game and the lettercubes
        /// </summary>
        BoardController boardController;

        IGameRules gameRules;

        LetterCube letterCube;

        int correctLetters = 0;

        int maxWrongLetters = 10;

        int minWrongLetters = 1;

        int maxCorrectLetters = 5;

        int minCorrectLetters = 1;

        /// <summary>
        /// Gets the Word and images for the current game
        /// </summary>
        public void GetSymbols()
        {

            

            //Checks if data has been loaded and if it has it begins preparing the board. Otherwise it waits on data being loaded before restarting
            if (DataLoader.IsDataLoaded)
            {

                gameRules.SetCorrectAnswer();
                if (texture.ContainsKey(gameRules.GetCorrectAnswer()))
                {
                    texture.Add(gameRules.GetCorrectAnswer(), ImageManager.GetImageFromWord(gameRules.GetCorrectAnswer()));
                }
                else
                {
                    Texture2D texture2D = ImageManager.GetImageFromWord(gameRules.GetCorrectAnswer());

                    letterCube = letterCubes[Random.Range(0, letterCubes.Count)];
                    
                    letterCube.ActivateImage(texture2D);
                }

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
                        texture.Add(randoImage, ImageManager.GetImageFromWord(randoImage));
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
                        texture.Add(gameRules.GetCorrectAnswer(), ImageManager.GetImageFromWord(gameRules.GetCorrectAnswer()));

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
                    
                    correctLetterCount++;
                }
                boardController.SetAnswerText("Tryk [Mellemrum] for at høre et ord, Find det billede der passer til det ord. Der er " + correctLetterCount + " tilbage.");


                //uses the CurrentWordSound 
                CurrentWordSound();

            }
        }

        /// <summary>
        /// Simple checks if the given word is the same as the currentword to see if the given word is correct.
        /// </summary>
        /// <param name="theWord"></param>
        /// <returns></returns>
        public bool IsCorrectSymbol(string theWord)
        {
            return gameRules.IsCorrectSymbol(theWord);
        }


        /// <summary>
        /// dictitates the current sound, may be changed later
        /// </summary>
        public void CurrentWordSound()
        {
            //Uses currentWord to find the right sound in tempgrovædersound in resource foulder
            string audioFileName = gameRules.GetCorrectAnswer().ToLower() + "_audio";

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
        /// Replaces images on the map when the number of correct ones are found.
        /// </summary>
        /// <param name="image"></param>
        public void ReplaceSymbol(LetterCube image)
        {
            
            //decreases the correctlettercount incase the player moves over a correct tile, and theres more then 1 answer on the board.
            if (IsCorrectSymbol(image.GetLetter()))
            {
                correctLetterCount--;
                boardController.SetAnswerText("Tryk [Mellemrum] for at h�re et ord, Find det billede der passer til det ord. Der er " + correctLetterCount + " tilbage.");
            }
            image.DeactivateImage();
            activeLetterCubes.Remove(image);

            LetterCube newImage;


            //finds a new random letterbox which is not active and is not the one which should be replaced
            while (true)
            {
                newImage = letterCubes[Random.Range(0, letterCubes.Count)];
                if (newImage != image && !activeLetterCubes.Contains(newImage))
                {
                    break;
                }
            }
            activeLetterCubes.Add(newImage);
            if (correctLetterCount > 0)
            {

                // yet again creates random words from the Word list.
                string randoWords = gameRules.GetWrongAnswer();
                while (randoWords == gameRules.GetCorrectAnswer())
                {
                    randoWords = gameRules.GetWrongAnswer();
                }

                //then adds the images to the texture dictionary if dosnt already exists.
                if (!texture.ContainsKey(randoWords))
                {
                    texture.Add(randoWords, ImageManager.GetImageFromWord(randoWords));

                }

                newImage.ActivateImage(texture[randoWords], randoWords);
            }
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
                        letterCube.DeactivateImage();
                    }
                    boardController.Won("Du vandt. Du fandt det korrekte Billede fem gange");
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
            //foreach(LetterCube letter in this.letterCubes){
            //    letter.toggleImage();
            //}
            boardController = board;
        }

        /// <summary>
        /// Sets the minimum and maximum correct images which appears on the board
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void SetMinAndMaxCorrectSymbols(int min, int max)
        {
            minCorrectLetters = min;
            maxCorrectLetters = max;
        }

        /// <summary>
        /// Sets the minimum and maximum wrong images which appears on the board
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



    }

}
