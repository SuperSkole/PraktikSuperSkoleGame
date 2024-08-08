using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Manager for letter cubes in the Grovæder game.
public class LetterCubeManager : MonoBehaviour
{
    //Text field of the LetterCube
    [SerializeField]private TextMeshPro text;
    //The gameboard the letter cube is connected to
    [SerializeField]private BoardManager board;

    //Whether the lettercube currently displays a letter
    private bool active;

    //Which letter the letter cube displays
    private string letter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Reacts to the players stepping on it and reacts according to if the letter is the correct one
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player" && active && !board.IsCorrectLetter(letter)){
            SelfDeactivate();
        }
    }

    //Overload on the activate method in case it is not important whether the letter is lower case. Takes the desired letter as input
    public void Activate(string letter){
        Activate(letter, false);
    }

    //Sets the letter of the letterbox and activates it by moving it upwards. If capitalization is not important it sets it to lower case half of the time randomly
    public void Activate(string letter, bool specific){
        int lower = UnityEngine.Random.Range(0, 2);
        if(lower == 0 && !specific){
            letter = letter.ToLower();
        }
        text.text = letter;
        this.letter = letter;
        if(!active){
           active = true;
           transform.Translate(0, 0.2f, 0);
        }
    }

    //Deactivates the letterbox by moving it back below the board and reseting the text and value of the letter variable
    public void Deactivate(){
        text.text = ".";
        letter = "";
        if(active){
            active = false;
            transform.Translate(0, -0.2f, 0);
        }
    }

    public void SetBoard(BoardManager board){
        this.board = board;
    }

    private void SelfDeactivate(){
        board.ReplaceLetter(this);
    }

}
