using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    //List of all lettercubes on the board. Other types of gameobjects should not be added.
    [SerializeField]private List<GameObject>letterCubes = new List<GameObject>();

    //List of all letterCube managers on the board
    private List<LetterCubeManager>letterCubeManagers = new List<LetterCubeManager>();

    //List of all lettercubemanagers which currently displays a letter
    private List<LetterCubeManager>activeLetterCubeManagers = new List<LetterCubeManager>();

    //The letter which the player is currently looking for 
    private string correctLetter = "A";

    //Number of correct letters on the board
    [SerializeField]private int correctLetterCount = 0;

    //Game object containing the text field telling which letter the player should find
    [SerializeField]private GameObject answerTextObject;

    //The text field containing the text telling the player which letter to find
    [SerializeField]private TextMeshProUGUI answerText;

    [SerializeField]private GameObject player;
    private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = player.GetComponent<PlayerManager>();
        answerText = answerTextObject.GetComponent<TextMeshProUGUI>();
        //Retrieves the lettercube managers from the list of lettercubes and sets their board variable to this board mananager
        foreach (GameObject l in letterCubes){
            LetterCubeManager lCM = l.GetComponent<LetterCubeManager>();
            if (lCM != null){
                letterCubeManagers.Add(lCM);
                lCM.SetBoard(this);
            }
        }
        NewLetters();
        

    }

    public PlayerManager GetPlayerManager(){
        return playerManager;
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
        foreach (LetterCubeManager lCM in activeLetterCubeManagers){
            lCM.Deactivate();
        }
        activeLetterCubeManagers.Clear();
        //finds new letterboxes to be activated and assigns them a random letter. If it selects the correct letter the count for it is increased
        for (int i = 0; i < count; i++){
            string letter = LetterAndWordCollections.GetRandomLetters(1)[0].ToString();
            if(IsCorrectLetter(letter)){
                correctLetterCount++;
            }
            activeLetterCubeManagers.Add(letterCubeManagers[UnityEngine.Random.Range(0, letterCubeManagers.Count)]);
            activeLetterCubeManagers[i].Activate(letter);
        }
        //finds a random letterbox for the correct letter which has not already been activated
        LetterCubeManager correctLetterBox;
        while(true){
            correctLetterBox = letterCubeManagers[UnityEngine.Random.Range(0, letterCubeManagers.Count)];
            if(!activeLetterCubeManagers.Contains(correctLetterBox)){
                break;
            }
        }
        correctLetterBox.Activate(correctLetter.ToLower(), true);
        correctLetterCount++;
        activeLetterCubeManagers.Add(correctLetterBox);
        answerText.text = "Led efter " + correctLetter + ". Der er " + correctLetterCount + " tilbage.";
    }

    //replaces a random active letterbox
    private void ReplaceRandomLetter(){
        ReplaceLetter(activeLetterCubeManagers[UnityEngine.Random.Range(0, activeLetterCubeManagers.Count)]);
    }

    //replaces a specific letterbox
    public void ReplaceLetter(LetterCubeManager letter){
        if(IsCorrectLetter(letter.GetLetter())){
            correctLetterCount--;
            answerText.text = "Led efter " + correctLetter + ". Der er " + correctLetterCount + " tilbage.";
        }
        letter.Deactivate();
        activeLetterCubeManagers.Remove(letter);
        
        LetterCubeManager newLetter;
        //finds a new random letterbox which is not active and is not the one which should be replaced
        while(true){
            newLetter = letterCubeManagers[UnityEngine.Random.Range(0, letterCubeManagers.Count)];
            if(newLetter != letter && !activeLetterCubeManagers.Contains(newLetter)){
                break;
            }
        }
        activeLetterCubeManagers.Add(newLetter);
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
