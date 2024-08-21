using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CORE.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scenes.Minigames.SymbolEater.Scripts.Gamemodes;


namespace Scenes.Minigames.SymbolEater.Scripts
{

    /// <summary>
    /// Board controller for the Symbol Eater mini game
    /// </summary>
    public class BoardController : MonoBehaviour
    {
        /// <summary>
        /// Only objects containing lettercubes should be added.
        /// </summary>
        [SerializeField] private List<GameObject> letterCubeObjects = new List<GameObject>();

        [SerializeField] private GameObject answerTextObject;

        [SerializeField] private GameObject answerImageObject;

        [SerializeField] private GameObject gameOverObject;

        private TextMeshProUGUI answerText;

        private TextMeshProUGUI gameOverText;

        private Image answerImage;

    [SerializeField]private GameObject playerObject;
    private SymbolEaterPlayer player;
    private SymbolEaterPlayer player;

        private IGameMode gameMode;

        [SerializeField] GameObject monsterPrefab;

        private DifficultyManager difficultyManager = new DifficultyManager();

        public MonsterHivemind monsterHivemind = new MonsterHivemind();


        public delegate void LoadFinished();


    /// <summary>
    /// Sets up the gameboard and the gamemode
    /// </summary>
    /// <param name="targetMode">The game mode which should be used</param>
    public void GameModeSet(IGameMode targetMode, IGameRules targetRules)
    public void GameModeSet(IGameMode targetMode, IGameRules targetRules)
    {
        //Sets various fieldvariables and their field variables
        gameMode = targetMode;
        player = playerObject.GetComponent<SymbolEaterPlayer>();
        player = playerObject.GetComponent<SymbolEaterPlayer>();
        player.board = this;
        answerText = answerTextObject.GetComponent<TextMeshProUGUI>();
        answerImage = answerImageObject.GetComponent<Image>();
        answerImage.enabled = false;
        List<LetterCube>letterCubes = new List<LetterCube>();
        gameOverText = gameOverObject.GetComponent<TextMeshProUGUI>();
        gameOverText.text = "";
        difficultyManager.SetBoardControllerAndMonsterPrefab(this, monsterPrefab);
        difficultyManager.SetDifficulty(DiffcultyPreset.EASY);

        //Retrieves the lettercubes from the list of lettercube objects and sets their board variable to this board controller
        foreach (GameObject l in letterCubeObjects)
        {
            LetterCube lC = l.GetComponent<LetterCube>();
            if (lC != null)
            {
                letterCubes.Add(lC);
                lC.SetBoard(this);
            }
        }
        gameMode.SetLetterCubesAndBoard(letterCubes, this);
        gameMode.SetGameRules(targetRules);
        gameMode.SetGameRules(targetRules);
        gameMode.GetSymbols();
    }

    /// <summary>
    /// uncomment if the scene is run directly from unity
    /// </summary>
    private void Start()
    {
        //GameModeSet(new SpellWordFromImage(), new CORE.Scripts.GameRules.SpellWord());
    }

    public SymbolEaterPlayer GetPlayer(){
    public SymbolEaterPlayer GetPlayer(){
        return player;
    }

        /// <summary>
        /// tells the gamemode to replace a specific letterbox
        /// </summary>
        /// <param name="letter">The letterbox which should be replaced</param>
        public void ReplaceLetter(LetterCube letter)
        {
            gameMode.ReplaceSymbol(letter);
        }

        /// <summary>
        /// Asks the gamemode whether a letter is the correct one.
        /// </summary>
        /// <param name="letter"></param>
        /// <returns>whether the letter is the same as the correct one</returns>
        public bool IsCorrectSymbol(string letter)
        {
            return gameMode.IsCorrectSymbol(letter);
        }

        /// <summary>
        /// Sets the text of the answertext ui element
        /// </summary>
        /// <param name="text">the text which should be displayed</param>
        public void SetAnswerText(string text)
        {
            answerText.text = text;
        }


        /// <summary>
        /// Sets the answerimage and activates it if it is not allready
        /// </summary>
        /// <param name="sprite">the image which should be displayed</param>
        public void SetImage(Sprite sprite)
        {
            if (!answerImage.enabled)
            {
                answerImage.enabled = true;
            }
            answerImage.sprite = sprite;
        }

        /// <summary>
        /// Called when the player is thrown of the board and loses
        /// </summary>
        public void Lost()
        {
            gameOverText.text = "Du tabte. Monsteret smed dig ud af brættet";
            monsterHivemind.OnGameOver();
        }

        /// <summary>
        /// Called when the player wins a gamemode
        /// </summary>
        /// <param name="winText">The text to display</param>
        public void Won(string winText)
        {
            gameOverText.text = winText;
            monsterHivemind.OnGameOver();
        }


        /// <summary>
        /// Instantiates a monster at the given coordinates
        /// </summary>
        /// <param name="monster">The monster which should be instantiated</param>
        /// <param name="pos">The position at which it should be instantiated</param>
        public void InstantitateMonster(GameObject monster, Vector3 pos)
        {
            GameObject monsterObject = Instantiate(monster, pos, Quaternion.identity);
            monsterHivemind.monsters.Add(monsterObject.GetComponent<Monster>());
        }

        /// <summary>
        /// Changes the minimum and maximum wrong letters which appear on the board
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void ChangeMinAndMaxWrongSymbols(int min, int max)
        {
            gameMode.SetMinAndMaxWrongSymbols(min, max);
        }

        /// <summary>
        /// Changes the minimum and maximum correct letters which appears on the board
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void ChangeMinAndMaxCorrectSymbols(int min, int max)
        {
            gameMode.SetMinAndMaxCorrectSymbols(min, max);
        }


        /// <summary>
        /// Run in case a game needs the image manager and it hasnt finished loading the images.
        /// </summary>
        /// <param name="OnFinish">The method to run when the imageManager has finished. Should have the format void MethodName()</param>
        public void StartImageWait(LoadFinished OnFinish)
        {

            StartCoroutine(WaitOnImageLoad(OnFinish));
        }

        /// <summary>
        /// Couroutine for waiting on image load
        /// </summary>
        /// <param name="OnFinish">The same as above</param>
        /// <returns></returns>
        IEnumerator WaitOnImageLoad(LoadFinished OnFinish)
        {
            player.StopMovement();
            monsterHivemind.PauseMovement();
            gameOverText.text = "Indlæser billeder. Vent venligst";
            yield return new WaitUntil(() => DataLoader.IsDataLoaded);
            OnFinish();
            player.StartMovement();
            monsterHivemind.StartMovement();
            gameOverText.text = "";
        }
    }
}