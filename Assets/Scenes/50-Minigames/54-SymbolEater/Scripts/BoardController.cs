using System;
using System.Collections;
using System.Collections.Generic;
using CORE.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scenes._10_PlayerScene.Scripts;
using Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes;
using CORE.Scripts.Game_Rules;
using Scenes._00_Bootstrapper;
using CORE;

namespace Scenes._50_Minigames._54_SymbolEater.Scripts
{

    /// <summary>
    /// Board controller for the Symbol Eater mini game
    /// </summary>
    public class BoardController : MonoBehaviour, IMinigameSetup
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

        [SerializeField] private GameObject coinAnimationPrefab;

        private SymbolEaterPlayer player;

        private ISEGameMode gameMode;

        [SerializeField] GameObject monsterPrefab;
        [SerializeField] AudioClip backgroundMusic;

        public DifficultyManager difficultyManager = new DifficultyManager();

        public MonsterHivemind monsterHivemind = new MonsterHivemind();
        public bool isTutorialOver = false;
        private IGameRules gameRules;


        public delegate void LoadFinished();


        #region initialization

        /// <summary>
        /// Sets up the gameboard and the gamemode
        /// </summary>
        /// <param name="targetMode">The game mode which should be used</param>
        public void SetupGame(IGenericGameMode targetMode, IGameRules targetRules)
        {
            AudioManager.Instance.PlaySound(backgroundMusic, SoundType.Music, true);
            //Sets various fieldvariables and their field variables
            gameMode = (ISEGameMode)targetMode;
            gameRules = targetRules;
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
                    lC.SetCoin(coinAnimationPrefab);
                }
            }
            gameMode.SetLetterCubesAndBoard(letterCubes, this);
            gameMode.SetGameRules(targetRules);
            gameMode.GetSymbols();
        }

        /// <summary>
        /// uncomment if the scene is run directly from unity
        /// </summary>
        private void Start()
        {
            //SetupGame(new FindNumber(), new FindNumberSeries());
        }

        #endregion

        #region Gamemode communication
        public SymbolEaterPlayer GetPlayer()
        {
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
            bool correct = gameMode.IsCorrectSymbol(letter);
            if(!isTutorialOver)
                isTutorialOver = correct;
            return correct;
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
        /// Instantiates a monster at the given coordinates
        /// </summary>
        /// <param name="monster">The monster which should be instantiated</param>
        /// <param name="pos">The position at which it should be instantiated</param>
        public void InstantitateMonster(GameObject monster, Vector3 pos)
        {
            GameObject monsterObject = Instantiate(monster, pos, Quaternion.identity);
            monsterObject.GetComponent<Monster>().SetBord(this);
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
        #endregion

        /// <summary>
        /// used to check if a pos is an active symobl
        /// </summary>
        /// <param name="pos">the pos you want to cheak</param>
        /// <returns>fasle if ther is an active symbol, and true if its a free space</returns>
        public bool IsPosFree(Vector3 pos)
        {
            GameObject closestCube = null;
            float Dist = Mathf.Infinity;
            for (int i = 0; i < letterCubeObjects.Count; i++)
            {
                if(Dist > Vector3.Distance(pos, letterCubeObjects[i].transform.position))
                {
                    Dist = Vector3.Distance(pos, letterCubeObjects[i].transform.position);
                    closestCube = letterCubeObjects[i];
                }
            }
            return !closestCube.GetComponent<LetterCube>().active;
        }


        #region Game Over

        /// <summary>
        /// Called when the player is thrown of the board and loses
        /// </summary>
        public void Lost()
        {
            gameOverText.text = "Du tabte. Monsteret smed dig ud af brættet";
            GameManager.Instance.DynamicDifficultyAdjustmentManager.UpdateLanguageUnitWeight(gameRules.GetCorrectAnswer(), false);
            monsterHivemind.OnGameOver();
            player.GameOver();
            gameMode.UpdateLanguageUnitWeight();
            StartCoroutine(ReturnToMainWorld());
        }

        /// <summary>
        /// Called when the player wins a gamemode
        /// </summary>
        /// <param name="winText">The text to display</param>
        public void Won(string winText, int xpReward, int goldReward)
        {
            GameManager.Instance.DynamicDifficultyAdjustmentManager.UpdateLanguageUnitWeight(gameRules.GetCorrectAnswer(), true);
            gameOverText.text = winText;
            monsterHivemind.OnGameOver();
            //Calls to update the players xp and gold. Temporary values
            player.GameOver();
            PlayerEvents.RaiseGoldChanged(goldReward);
            PlayerEvents.RaiseXPChanged(xpReward);
            string answer = gameRules.GetCorrectAnswer();
            gameMode.UpdateLanguageUnitWeight();
            if(answer.Length > 1)
            {
                PlayerEvents.RaiseAddWord(answer);
            }
            else
            {
                PlayerEvents.RaiseAddLetter(answer[0]);
            }
            StartCoroutine(ReturnToMainWorld());
        }


        /// <summary>
        /// Called when the game is over in order to wait a bit before returning to main world
        /// </summary>
        /// <returns></returns>
        IEnumerator ReturnToMainWorld()
        {
            if(SymbolEaterSoundController.playSound)
            {
                SymbolEaterSoundController.playSound = false;
            }
            player.GameOver();
            yield return new WaitForSeconds(5);
            SwitchScenes.SwitchToMainWorld();
        }


        #endregion

        
    }
}