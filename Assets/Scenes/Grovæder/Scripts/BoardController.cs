using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;

public class BoardController : MonoBehaviour
{
    /// <summary>
    /// List of all lettercubes on the board. Other types of gameobjects should not be added.
    /// </summary>
    [SerializeField]private List<GameObject>letterCubeObjects = new List<GameObject>();

    /// <summary>
    /// Game object containing the text field telling which letter the player should find
    /// </summary>
    [SerializeField]private GameObject answerTextObject;

    /// <summary>
    /// The text field containing the text telling the player which letter to find
    /// </summary>
    private TextMeshProUGUI answerText;

    [SerializeField]private GameObject playerObject;
    private Player player;

    private IGameMode gameMode = new FindCorrectLetter();

    // Start is called before the first frame update
    void Start()
    {
        player = playerObject.GetComponent<Player>();
        answerText = answerTextObject.GetComponent<TextMeshProUGUI>();
        List<LetterCube>letterCubes = new List<LetterCube>();
        //Retrieves the lettercube managers from the list of lettercubes and sets their board variable to this board mananager
        foreach (GameObject l in letterCubeObjects){
            LetterCube lC = l.GetComponent<LetterCube>();
            if (lC != null){
                letterCubes.Add(lC);
                lC.SetBoard(this);
            }
        }
        gameMode.SetLetterCubesAndBoard(letterCubes, this);
        gameMode.GetLetters();

    }

    public Player GetPlayerManager(){
        return player;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            gameMode.GetLetters();
        }
    }


    /// <summary>
    /// tells the gamemode to replace a specific letterbox
    /// </summary>
    /// <param name="letter">The letterbox which should be replaced</param>
    public void ReplaceLetter(LetterCube letter){
        gameMode.ReplaceLetter(letter);
    }

    /// <summary>
    /// Asks the gamemode whether a letter is the correct one.
    /// </summary>
    /// <param name="letter"></param>
    /// <returns>whether the letter is the same as the correct one</returns>
    public bool IsCorrectLetter(string letter){
        return gameMode.IsCorrectLetter(letter);
    }

    /// <summary>
    /// Sets the text of the answertext ui element
    /// </summary>
    /// <param name="text">the text which should be displayed</param>
    public void SetAnswerText(string text){
        answerText.text = text;
    }
}
