using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CORE.Scripts;
using TMPro;
using UnityEngine;
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
    
    /// <summary>
    /// Object containing the answer image
    /// </summary>
    [SerializeField]private GameObject answerImageObject;

    [SerializeField]private GameObject gameOverObject;
    /// <summary>
    /// The text field containing the text telling the player which letter to find
    /// </summary>
    private TextMeshProUGUI answerText;

    /// <summary>
    /// The text field to display text then the game is over both if the player wins or loses
    /// </summary>
    private TextMeshProUGUI gameOverText;

    private Image answerImage;

    [SerializeField]private GameObject playerObject;
    private Player player;

    private IGameMode gameMode;

    [SerializeField]GameObject monsterPrefab;

    private DifficultyManager difficultyManager = new DifficultyManager();

    public MonsterHivemind monsterHivemind = new MonsterHivemind();


    public delegate void ImageLoadFinished();


    // Start is called before the first frame update
    public void GameModeSet(IGameMode targetMode)
    {
        gameMode = targetMode;
        player = playerObject.GetComponent<Player>();
        player.board = this;
        answerText = answerTextObject.GetComponent<TextMeshProUGUI>();
        answerImage = answerImageObject.GetComponent<Image>();
        answerImage.enabled = false;
        List<LetterCube>letterCubes = new List<LetterCube>();
        gameOverText = gameOverObject.GetComponent<TextMeshProUGUI>();
        gameOverText.text = "";
        difficultyManager.SetBoardControllerAndMonsterPrefab(this, monsterPrefab);
        difficultyManager.SetDifficulty(DiffcultyPreset.EASY);

        //Retrieves the lettercube managers from the list of lettercubes and sets their board variable to this board mananager
        foreach (GameObject l in letterCubeObjects){
            LetterCube lC = l.GetComponent<LetterCube>();
            if (lC != null){
                letterCubes.Add(lC);
                lC.SetBoard(this);
            }
        }
        gameMode.SetLetterCubesAndBoard(letterCubes, this);
        gameMode.GetSymbols();
    }

    private void Start()
    {
        GameModeSet(new SpellWord());
    }

    public Player GetPlayer(){
        return player;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            gameMode.GetSymbols();
        }
    }


    /// <summary>
    /// tells the gamemode to replace a specific letterbox
    /// </summary>
    /// <param name="letter">The letterbox which should be replaced</param>
    public void ReplaceLetter(LetterCube letter){
        gameMode.ReplaceSymbol(letter);
    }

    /// <summary>
    /// Asks the gamemode whether a letter is the correct one.
    /// </summary>
    /// <param name="letter"></param>
    /// <returns>whether the letter is the same as the correct one</returns>
    public bool IsCorrectSymbol(string letter){
        return gameMode.IsCorrectSymbol(letter);
    }

    /// <summary>
    /// Sets the text of the answertext ui element
    /// </summary>
    /// <param name="text">the text which should be displayed</param>
    public void SetAnswerText(string text){
        answerText.text = text;
    }


    /// <summary>
    /// Sets the answerimage and activates it if it is not allready
    /// </summary>
    /// <param name="sprite">the image which should be displayed</param>
    public void SetImage(Sprite sprite){
        if(!answerImage.enabled){
            answerImage.enabled = true;
        }
        answerImage.sprite = sprite;
    }

    /// <summary>
    /// Called when the player is thrown of the board and loses
    /// </summary>
    public void Lost(){
        gameOverText.text = "Du tabte. Monsteret smed dig ud af brættet";
        monsterHivemind.OnGameOver();
    }

    /// <summary>
    /// Called when the player wins a gamemode
    /// </summary>
    /// <param name="winText">The text to display</param>
    public void Won(string winText){
        gameOverText.text = winText;
        monsterHivemind.OnGameOver();
    }


    /// <summary>
    /// Instantiates a monster at the given coordinates
    /// </summary>
    /// <param name="monster">The monster which should be instantiated</param>
    /// <param name="pos">The position at which it should be instantiated</param>
    public void InstantitateMonster(GameObject monster, Vector3 pos){
        GameObject monsterObject = Instantiate(monster, pos, Quaternion.identity);
        monsterHivemind.monsters.Add(monsterObject.GetComponent<Monster>());
    }

    /// <summary>
    /// Changes the minimum and maximum wrong letters which appear on the board
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void ChangeMinAndMaxWrongSymbols(int min, int max){
        gameMode.SetMinAndMaxWrongSymbols(min, max);
    }

    /// <summary>
    /// Changes the minimum and maximum correct letters which appears on the board
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void ChangeMinAndMaxCorrectSymbols(int min, int max){
        gameMode.SetMinAndMaxCorrectSymbols(min, max);
    }

    public void StartImageWait(ImageLoadFinished OnFinish){
        
        StartCoroutine(waitOnImageLoad(OnFinish));
    }

    IEnumerator waitOnImageLoad(ImageLoadFinished OnFinish){
        player.StopMovement();
        monsterHivemind.PauseMovement();
        gameOverText.text = "Indlæser billeder. Vent venligst";
        yield return new WaitUntil(() => ImageManager.IsDataLoaded);
        OnFinish();
        player.StartMovement();
        monsterHivemind.StartMovement();
        gameOverText.text = "";
    }
}