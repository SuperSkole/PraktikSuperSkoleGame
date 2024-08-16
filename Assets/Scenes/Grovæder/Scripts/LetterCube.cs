using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    /// A gameobject to have the images sprite onto.
    /// </summary>
    [SerializeField]private GameObject imageObject;


    /// <summary>
    /// these 3 are used for the FindImageFromSound gamemode, with a SpriteRendere and a sprite, and a String for the current word.
    /// </summary>
    private Sprite texture;

    private SpriteRenderer spriteRenderer;

    private string isCurrentWord;

    

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
    /// Whether the font used for the displayed symbol should be randomized
    /// </summary>
    public bool randomizeFont = false;

    /// <summary>
    /// List of the fonts which the lettercube can select.
    /// </summary>
    [SerializeField] List<TMP_FontAsset> fonts;


    bool readyForDeactivation = false;
    /// <summary>
    /// Whether the lettercube currently displays a letter
    /// </summary>
    public bool active;

    /// <summary>
    /// Which letter the letter cube displays
    /// </summary>
    private string letter;
    

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        spriteRenderer = imageObject.GetComponent<SpriteRenderer>();
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
        if(!readyForDeactivation){
            if(other.gameObject.tag == "Player" && active && !board.IsCorrectSymbol(letter) && !board.GetPlayer().thrown && board.GetPlayer().hasMoved){
                StartCoroutine(IncorrectGuess());
                board.GetPlayer().IncorrectGuess();
            }
            else if(active && other.gameObject.tag == "Player" && !board.GetPlayer().thrown && board.GetPlayer().hasMoved){
                StartCoroutine(CorrectGuess());
            }
        }
    }

    /// <summary>
    /// Overload on the activate method in case it is not important whether the letter is lower case. Takes the desired letter as input
    /// </summary>
    /// <param name="letter">The letter which should be displayed</param>
    public void Activate(string letter){
        Activate(letter, false);
    }

    /// <summary>
    /// Overload on the activate method in case it is not important whether the Word is lower case. Takes the desired Word as input
    /// </summary>
    public void ActivateImage(Sprite sprite, string word )
    {
        
        spriteRenderer.sprite = sprite;
        letter = word;

        if (!active)
        {
            active = true;
            transform.Translate(0, 0.2f, 0);
        }
    }

    /// <summary>
    /// Same as Above but Meant for the find imageGamemode
    /// </summary>
    /// <param name="texture2D"></param>
    /// <param name="currentWord"></param>
    public void ActivateImage(Sprite sprite)
    {
        ActivateImage(sprite, isCurrentWord);
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
        int lower = Random.Range(0, 2);
        if(lower == 0 && !specific){
            letter = letter.ToLower();
        }
        if(randomizeFont){
            text.font = fonts[Random.Range(0, fonts.Count)];
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
            readyForDeactivation = false;
        }
    }


    /// <summary>
    /// Deactivates the letterbox by moving it back below the board and reseting the Image and value of the Word variable
    /// </summary>
    public void DeactivateImage()
    {
        texture = null;
        isCurrentWord = ".";

        if (active)
        {
            active = false;
            transform.Translate(0, -0.2f, 0);
            readyForDeactivation = false;
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
        readyForDeactivation = true;
        meshRenderer.material = incorrectMaterial;
        yield return new WaitForSeconds(board.GetPlayer().maxMoveDelay);
        meshRenderer.material = defaultMaterial;
        SelfDeactivate();
    }

    /// <summary>
    /// Changes the color of the lettercube if it contains the correct letter
    /// </summary>
    /// <returns></returns>
    IEnumerator CorrectGuess(){
        readyForDeactivation = true;
        meshRenderer.material = correctMaterial;
        yield return new WaitForSeconds(1);
        meshRenderer.material = defaultMaterial;
        SelfDeactivate();
    }
}
