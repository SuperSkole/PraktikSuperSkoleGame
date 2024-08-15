using CORE.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class FindImageFromSound : IGameMode
{

    /// <summary>
    /// Current Word Sound clip
    /// </summary>
    GrovÆderSoundController currentWordsoundClip;

    /// <summary>
    /// The correct letter
    /// </summary>
    List<string> words = new List<string>(){
        "Bil", "Båd", "Fly"
    };

    string currentWord;

    string word;

    /// <summary>
    /// List of all lettercubes. Should be retrieved from Boardcontroller with method SetLetterCubesAndBoard
    /// </summary>
    List<LetterCube> letterCubes;

    /// <summary>
    /// The lettercubes displaying a letter
    /// </summary>
    List<LetterCube> activeLetterCubes = new List<LetterCube>();

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
    /// Gets the letters for the current game
    /// </summary>
    public void GetSymbols()
    {
        word = words[Random.Range(0, words.Count)].ToLower();
        //deactives all current active lettercubes
        foreach (LetterCube lC in activeLetterCubes)
        {
            lC.Deactivate();
        }
        int count = Random.Range(minWrongLetters, maxWrongLetters + 1);
        activeLetterCubes.Clear();
        //finds new letterboxes to be activated and assigns them a random letter. If it selects the correct letter the count for it is increased
        for (int i = 0; i < count; i++)
        {
            Texture2D image = ImageManager.GetRandomImage();
            while (IsCorrectSymbol(word))
            {
                image = ImageManager.GetRandomImage();
            }
            LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];

            //Check to ensure letters dont spawn below the player and that it is not an allready activated lettercube
            while (activeLetterCubes.Contains(potentialCube) && potentialCube.gameObject.transform.position != boardController.GetPlayer().gameObject.transform.position)
            {
                potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            }
            activeLetterCubes.Add(potentialCube);
            activeLetterCubes[i].ActivateImage(image);
        }
        //creates a random number of correct letters on the board
        int wrongCubeCount = activeLetterCubes.Count;
        count = Random.Range(minCorrectLetters, maxCorrectLetters + 1);
        for (int i = 0; i < count; i++)
        {
            string image = currentWord.ToLower();
            string imageFileName = currentWord.ToUpper() + "_image";
            Texture2D currentImage = Resources.Load<Texture2D>($"Pictures/{imageFileName}");
            LetterCube potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            //Check to ensure letters dont spawn below the player and that it is not an already activated lettercube
            while (activeLetterCubes.Contains(potentialCube))
            {
                potentialCube = letterCubes[Random.Range(0, letterCubes.Count)];
            }
            activeLetterCubes.Add(potentialCube);
            activeLetterCubes[i + wrongCubeCount].ActivateImage(currentImage, image);
            correctLetterCount++;
        }
        boardController.SetAnswerText("Tryk [Mellemrum] for at høre et ord, Find det billede der passer til det ord. Der er " + correctLetterCount + " tilbage.");

        CurrentWordSound();
    }

    public bool IsCorrectSymbol(string image)
    {
        return image.ToLower() == currentWord.ToLower();
    }

    public void CurrentWordSound()
    {
        //bruger correctLetter to find the right sound in tempgrovædersound in resource foulder
        string audioFileName = word.ToLower() + "_audio";

        AudioClip clip = Resources.Load<AudioClip>($"AudioWords/{audioFileName}");

        //checks whether or not its null.
        if (clip != null)
        {
            if (currentWordsoundClip == null)
                currentWordsoundClip = GameObject.FindObjectOfType<GrovÆderSoundController>();

            currentWordsoundClip.SetGrovæderSound(clip); // sends sound to AudioController
        }
        else
        {
            Debug.LogError("Lydklippet blev ikke fundet!");
        }
    }

    public void ReplaceSymbol(LetterCube image)
    {
        if (IsCorrectSymbol(image.GetLetter()))
        {
            correctLetterCount--;
            boardController.SetAnswerText("Tryk [Mellemrum] for at høre et ord, Find det billede der passer til det ord. Der er " + correctLetterCount + " tilbage.");
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
            newImage.ActivateImage(ImageManager.GetRandomImage());
            while (newImage.GetLetter() == currentWord)
            {
                newImage.ActivateImage(ImageManager.GetRandomImage());
            }
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
                boardController.Won("Du vandt. Du fandt det korrekte bogstav fem gange");
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

    // Start is called before the first frame update

}
