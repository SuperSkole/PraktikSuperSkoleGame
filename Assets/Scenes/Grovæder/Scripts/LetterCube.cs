using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Manager for letter cubes in the Grovæder game.
/// </summary>
public class LetterCube : MonoBehaviour
{
    /// <summary>
    /// Text field of the LetterCube
    /// </summary>
    [SerializeField]private TextMeshPro text;
    /// <summary>
    /// The gameboard the letter cube is connected to
    /// </summary>
    [SerializeField]private BoardController board;

    //Materials to change the color of the cube depending on the current status
    [SerializeField]private Material defaultMaterial;
    [SerializeField]private Material correctMaterial;
    [SerializeField]private Material incorrectMaterial;

    [SerializeField]private MeshRenderer meshRenderer;

    /// <summary>
    /// Whether the lettercube currently displays a letter
    /// </summary>
    private bool active;

    /// <summary>
    /// Which letter the letter cube displays
    /// </summary>
    private string letter;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        
    }

    /// <summary>
    /// Reacts to the players stepping on it and reacts according to if the letter is the correct one
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other){
        if(meshRenderer == null){
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            defaultMaterial = meshRenderer.material;
        }
        if(other.gameObject.tag == "Player" && active && !board.IsCorrectLetter(letter) && !board.GetPlayer().thrown){
            StartCoroutine(IncorrectGuess());
            board.GetPlayer().IncorrectGuess();
        }
        else if(active && other.gameObject.tag == "Player" && !board.GetPlayer().thrown){
            StartCoroutine(CorrectGuess());
        }
    }

    /// <summary>
    /// Overload on the activate method in case it is not important whether the letter is lower case. Takes the desired letter as input
    /// </summary>
    /// <param name="letter">The letter which should be displayed</param>
    public void Activate(string letter){
        Activate(letter, false);
    }

    public string GetLetter(){
        return letter;
    }

    /// <summary>
    /// Sets the letter of the letterbox and activates it by moving it upwards. If capitalization is not important it sets it to lower case half of the time randomly
    /// </summary>
    /// <param name="letter">The letter Which should be displayed</param>
    /// <param name="specific">Whether capitilzation should be preserved</param>
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

    /// <summary>
    /// Deactivates the letterbox by moving it back below the board and reseting the text and value of the letter variable
    /// </summary>
    public void Deactivate(){
        text.text = ".";
        letter = "";
        if(active){
            active = false;
            transform.Translate(0, -0.2f, 0);
        }
    }

    
    public void SetBoard(BoardController board){
        this.board = board;
    }

    private void SelfDeactivate(){
        board.ReplaceLetter(this);
    }

    /// <summary>
    /// Changes the color of the lettercube for some time if it does not contain the correct letter
    /// </summary>
    /// <returns></returns>
    IEnumerator IncorrectGuess(){
        meshRenderer.material = incorrectMaterial;
        yield return new WaitForSeconds(6);
        meshRenderer.material = defaultMaterial;
        SelfDeactivate();
    }

    /// <summary>
    /// Changes the color of the lettercube if it contains the correct letter
    /// </summary>
    /// <returns></returns>
    IEnumerator CorrectGuess(){
        meshRenderer.material = correctMaterial;
        yield return new WaitForSeconds(1);
        meshRenderer.material = defaultMaterial;
        SelfDeactivate();
    }
}
