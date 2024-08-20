using System.Collections.Generic;
using UnityEngine;


namespace Scenes.Minigames.SymbolEater.Scripts.Gamemodes.FindImageFromSound
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
        List<string> words = new List<string>()
        {
        "Bil", "B�d", "Fly"
        };

        string currentWord;



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
        Dictionary<string, Sprite> texture = new Dictionary<string, Sprite>();

        /// <summary>
        /// number of correct letters currntly displayed
        /// </summary>
        int correctLetterCount;

        /// <summary>
        /// The boardController of the current game
        /// </summary>
        BoardController boardController;

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


            // gets a current word to find images and sound with.

            currentWord = words[Random.Range(0, words.Count)].ToLower();
            //deactives all current active lettercubes
            foreach (LetterCube lC in activeLetterCubes)
            {
                lC.Deactivate();
            }
            int count = Random.Range(minWrongLetters, maxWrongLetters + 1);
            activeLetterCubes.Clear();
            //finds new letterboxes to be activated and assigns them a random image. If it selects the correct Image the count for it is increased
            for (int i = 0; i < count; i++)
            {


                // creates random words from the word list, then creates images to fit those random words.

                string randoImage = words[Random.Range(0, words.Count)];
                if (!texture.ContainsKey(randoImage))
                {
                    texture.Add(randoImage, Resources.Load<Sprite>("Pictures/" + randoImage + "_image"));
                }

                Sprite image = texture[randoImage];

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
            for (int i = 0; i < count; i++)
            {
                if (!texture.ContainsKey(currentWord))
                {
                    texture.Add(currentWord, Resources.Load<Sprite>("Pictures/" + currentWord + "_image"));

                }
                // makes a image string from the current word variable, so that we can find it in the files.
                string image = currentWord.ToLower();
                string imageFileName = currentWord + "_image";

                Sprite currentImage = texture[currentWord];
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
            boardController.SetAnswerText("Tryk [Mellemrum] for at h�re et ord, Find det billede der passer til det ord. Der er " + correctLetterCount + " tilbage.");


            //uses the CurrentWordSound 
            CurrentWordSound();
        }


        /// <summary>
        /// Checks if the Word is the same as the correct one
        /// </summary>
        /// <param name="letter">The Word which should be checked</param>
        /// <returns>Whether the Word is the correct one</returns>
        public bool IsCorrectSymbol(string image)
        {
            return image.ToLower() == currentWord.ToLower();
        }


        /// <summary>
        /// dictates what the currentLetterSound is from the currentWord.
        /// </summary>
        public void CurrentWordSound()
        {
            //Uses currentWord to find the right sound in tempSymbolEatersound in resource foulder
            string audioFileName = currentWord.ToLower() + "_audio";

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

            //decreases the correctlettercount incase theres more then 1 answer on the board.
            if (IsCorrectSymbol(image.GetLetter()))
            {
                correctLetterCount--;
                boardController.SetAnswerText("Tryk [Mellemrum] for at h�re et ord, Find det billede der passer til det ord. Der er " + correctLetterCount + " tilbage.");
            }
            image.Deactivate();
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
                string randoWords = words[Random.Range(0, words.Count)].ToLower();
                while (randoWords == currentWord)
                {
                    randoWords = words[Random.Range(0, words.Count)].ToLower();
                }

                //then adds the images to the texture dictionary if dosnt already exists.
                if (!texture.ContainsKey(randoWords))
                {
                    texture.Add(randoWords, Resources.Load<Sprite>("Pictures/" + randoWords + "_image"));

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
                        letterCube.Deactivate();
                    }
                    boardController.Won("Du vandt. Du fandt det korrekte billede fem gange");
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

        // Start is called before the first frame update


    }
}
