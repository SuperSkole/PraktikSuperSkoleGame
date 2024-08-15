using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

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
    
    [SerializeField]private GameObject answerImageObject;


    /// <summary>
    /// The text field containing the text telling the player which letter to find
    /// </summary>
    private TextMeshProUGUI answerText;

    private Image answerImage;

    [SerializeField]private GameObject playerObject;
    private Player player;

    private IGameMode gameMode;

    // Start is called before the first frame update
    public void GameModeSet(IGameMode modeSet)
    {
        gameMode = modeSet;
        player = playerObject.GetComponent<Player>();
        answerText = answerTextObject.GetComponent<TextMeshProUGUI>();
        answerImage = answerImageObject.GetComponent<Image>();
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
    private void Start()
    {
        
    }

    public Player GetPlayer(){
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

    public void SetImage(Sprite sprite){

        answerImage.sprite = sprite;
    }
}
