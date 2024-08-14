using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellWordFromSound : IGameMode
{
    /// <summary>
    /// The correct word
    /// </summary>
    string word;

    /// <summary>
    /// Current Word Sound clip
    /// </summary>
    GrovÆderSoundController currentWordsoundClip;

    int correctWords = 0;

    int currentIndex;

    List<string> words = new List<string>(){
        "Bil", "Båd", "Fly"
    };

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


    int minWrongLetters = 6;

    int maxWrongLetters = 10;

    /// <summary>
    /// Gets the letters for the current game
    /// </summary>
    public void GetLetters()
    {
        currentIndex = 0;
        word = words[Random.Range(0, words.Count)].ToLower();
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
        int count = Random.Range(6, 11);
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



        CurrentWordSound();
    }


    /// <summary>
    /// Checks if the letter is of the correct type
    /// </summary>
    /// <param name="letter">The letter which should be checked</param>
    /// <returns>Whether the letter is the correct one</returns>
    public bool IsCorrectLetter(string letter)
    {
        return word[currentIndex] == letter.ToLower()[0];
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

    public void ReplaceLetter(LetterCube letter)
    {
        if (IsCorrectLetter(letter.GetLetter()))
        {
            currentIndex++;
            string foundLetters = "";
            for (int i = 0; i < currentIndex; i++)
            {
                foundLetters += word[i];
            }
            boardController.SetAnswerText(foundLetters);
        }
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
            char nL = LetterManager.GetRandomLetters(1)[0];
            while (nL == word[currentIndex])
            {
                nL = LetterManager.GetRandomLetters(1)[0];
            }

            newLetter.Activate(nL.ToString());

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
                GetLetters();
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
    public void SetMinAndMaxCorrectLetters(int min, int max)
    {

    }

    /// <summary>
    /// Sets the minimum and maximum wrong letters which appears on the board
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SetMinAndMaxWrongLetters(int min, int max)
    {
        minWrongLetters = min;
        maxWrongLetters = max;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
