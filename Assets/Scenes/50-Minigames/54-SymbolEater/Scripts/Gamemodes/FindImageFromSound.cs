using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._00_Bootstrapper;
using System.Collections.Generic;
using UnityEngine;
using Words;


namespace Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes
{
    public class FindImageFromSound : ISEGameMode
    {

        /// <summary>
        /// Current Word Sound clip
        /// </summary>
        SymbolEaterSoundController currentWordsoundClip;

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
                if (!texture.ContainsKey(gameRules.GetSecondaryAnswer()))
                {
                    texture.Add(gameRules.GetSecondaryAnswer(), ImageManager.GetImageFromWord(gameRules.GetSecondaryAnswer()));
                }
                else
                {
                    Texture2D texture2D = ImageManager.GetImageFromWord(gameRules.GetSecondaryAnswer());

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

                    string randoImage = WordRepository.GetAllWords()[Random.Range(0, WordRepository.GetAllWords().Count)].Identifier;
                    while(!WordsForImagesManager.imageWords.Contains(randoImage))
                    {
                        randoImage = WordRepository.GetAllWords()[Random.Range(0, WordRepository.GetAllWords().Count)].Identifier;
                    }
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
                boardController.SetAnswerText("Tryk [mellemrum] og find det rigtige billede. " + correctLetterCount + " ord tilbage.");


                //uses the CurrentWordSound 
                //CurrentWordSound();

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
            string audioFileName = gameRules.GetSecondaryAnswer().ToLower() + "_audio";

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
                boardController.SetAnswerText("Tryk [Mellemrum] for at h\u00f8re et ord, Find det billede der passer til det ord. Der er " + correctLetterCount + " tilbage.");
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
                string randoWords = WordRepository.GetAllWords()[Random.Range(0, WordRepository.GetAllWords().Count)].Identifier;
                while(!WordsForImagesManager.imageWords.Contains(randoWords) && randoWords == gameRules.GetSecondaryAnswer())
                {
                    randoWords = WordRepository.GetAllWords()[Random.Range(0, WordRepository.GetAllWords().Count)].Identifier;
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
                        letterCube.DeactivateImage();
                    }
                    int multiplier = 1;
                    //Calculates the multiplier for the xp reward. All values are temporary
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
                    boardController.Won("Du vandt. Du fandt det korrekte Billede fem gange", 1 * multiplier, 1 * multiplier);
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

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="letterCube"></param>
        /// <param name="correct"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ActivateCube(LetterCube letterCube, bool correct)
        {

        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <returns></returns>
        public bool IsGameComplete()
        {
            return false;
        }

        /// <summary>
        /// Not used
        /// </summary>
        public void UpdateLanguageUnitWeight()
        {
            
        }
    }

}
