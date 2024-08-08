using System;
using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
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
        //deactives all current active lettercubes
        foreach (LetterCubeManager lCM in activeLetterCubeManagers){
            lCM.Deactivate();
        }
        activeLetterCubeManagers.Clear();
        //finds new letterboxes to be activated and assigns them a random letter
        for (int i = 0; i < count; i++){
            string letter = LetterAndWordCollections.GetRandomLetters(1)[0].ToString();
            activeLetterCubeManagers.Add(letterCubeManagers[UnityEngine.Random.Range(0, letterCubeManagers.Count)]);
            activeLetterCubeManagers[i].Activate(letter);
        }
        //finds a random letterbox for the correct letter which has not allready been activated
        LetterCubeManager correctLetterBox;
        while(true){
            correctLetterBox = letterCubeManagers[UnityEngine.Random.Range(0, letterCubeManagers.Count)];
            if(!activeLetterCubeManagers.Contains(correctLetterBox)){
                break;
            }
        }
        correctLetterBox.Activate(correctLetter, true);
        activeLetterCubeManagers.Add(correctLetterBox);
    }

    //replaces a random active letterbox
    private void ReplaceRandomLetter(){
        ReplaceLetter(activeLetterCubeManagers[UnityEngine.Random.Range(0, activeLetterCubeManagers.Count)]);
    }

    public void PlayerReplaceLetter(LetterCubeManager letter){
        ReplaceLetter(letter);
    }

    //replaces a specific letterbox
    public void ReplaceLetter(LetterCubeManager letter){
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
        newLetter.Activate(LetterAndWordCollections.GetRandomLetters(1)[0].ToString());
    }

    public bool IsCorrectLetter(string letter){
        return letter == correctLetter;
    }
}
