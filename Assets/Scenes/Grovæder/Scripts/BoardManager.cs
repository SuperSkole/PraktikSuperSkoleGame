using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    /// <summary>
    /// List of all lettercube gameobjects on the board. Other types of gameobjects should not be added.
    /// </summary>
    [SerializeField]private List<GameObject>letterCubeObjects = new List<GameObject>();

    /// <summary>
    /// List of all letterCubes on the board
    /// </summary>
    private List<LetterCube>letterCubes = new List<LetterCube>();

    /// <summary>
    /// List of all lettercubes which currently displays a letter
    /// </summary>
    private List<LetterCube>activeLetterCubes = new List<LetterCube>();

    /// <summary>
    /// List of all lettercubes which currently displays a letter
    /// </summary>
    private string correctLetter = "A";

    /// <summary>
    /// List of all lettercubes which currently displays a letter
    /// </summary>
    [SerializeField]private int correctLetterCount = 0;

    /// <summary>
    /// List of all lettercubes which currently displays a letter
    /// </summary>
    [SerializeField]private GameObject answerTextObject;

    /// <summary>
    /// The text field containing the text telling the player which letter to find
    /// </summary>
    [SerializeField]private TextMeshProUGUI answerText;

    [SerializeField]private GameObject playerObject;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = playerObject.GetComponent<Player>();
        answerText = answerTextObject.GetComponent<TextMeshProUGUI>();
        //Retrieves the lettercube managers from the list of lettercubes and sets their board variable to this board mananager
        foreach (GameObject l in letterCubeObjects){
            LetterCube lC = l.GetComponent<LetterCube>();
            if (lC != null){
                letterCubes.Add(lC);
                lC.SetBoard(this);
            }
        }
        NewLetters();
        

    }

    public Player GetPlayerManager(){
        return player;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            NewLetters();
        }
        else if(Input.GetKeyDown(KeyCode.Y)){
            ReplaceRandomLetter();
        }
    }

    //Overload of newletters which activates a random number of letterboxes
    private void NewLetters(){
        NewLetters(UnityEngine.Random.Range(1, 11));
    }

    //activates a given number of letterboxes
    private void NewLetters(int count){
        correctLetter = LetterAndWordCollections.GetRandomLetters(1)[0].ToString();
        //deactives all current active lettercubes
        foreach (LetterCube lC in activeLetterCubes){
            lC.Deactivate();
        }
        activeLetterCubes.Clear();
        //finds new letterboxes to be activated and assigns them a random letter. If it selects the correct letter the count for it is increased
        for (int i = 0; i < count; i++){
            string letter = LetterAndWordCollections.GetRandomLetters(1)[0].ToString();
            if(IsCorrectLetter(letter)){
                correctLetterCount++;
            }
            activeLetterCubes.Add(letterCubes[UnityEngine.Random.Range(0, letterCubes.Count)]);
            activeLetterCubes[i].Activate(letter);
        }
        //finds a random letterbox for the correct letter which has not already been activated
        LetterCube correctLetterBox;
        while(true){
            correctLetterBox = letterCubes[UnityEngine.Random.Range(0, letterCubes.Count)];
            if(!activeLetterCubes.Contains(correctLetterBox)){
                break;
            }
        }
        correctLetterBox.Activate(correctLetter.ToLower(), true);
        correctLetterCount++;
        activeLetterCubes.Add(correctLetterBox);
        answerText.text = "Led efter " + correctLetter + ". Der er " + correctLetterCount + " tilbage.";
    }

    //replaces a random active letterbox
    private void ReplaceRandomLetter(){
        ReplaceLetter(activeLetterCubes[UnityEngine.Random.Range(0, activeLetterCubes.Count)]);
    }

    //replaces a specific letterbox
    public void ReplaceLetter(LetterCube letter){
        if(IsCorrectLetter(letter.GetLetter())){
            correctLetterCount--;
            answerText.text = "Led efter " + correctLetter + ". Der er " + correctLetterCount + " tilbage.";
        }
        letter.Deactivate();
        activeLetterCubes.Remove(letter);
        
        LetterCube newLetter;
        //finds a new random letterbox which is not active and is not the one which should be replaced
        while(true){
            newLetter = letterCubes[UnityEngine.Random.Range(0, letterCubes.Count)];
            if(newLetter != letter && !activeLetterCubes.Contains(newLetter)){
                break;
            }
        }
        activeLetterCubes.Add(newLetter);
        if(correctLetterCount > 0){
            newLetter.Activate(LetterAndWordCollections.GetRandomLetters(1)[0].ToString());
        }
        else{
            NewLetters();
        }
    }

    //Checks whether a letter is the correct one. As it is not case sensistive both are lowered for the comparison.
    public bool IsCorrectLetter(string letter){
        return letter.ToLower() == correctLetter.ToLower();
    }
}
