using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CORE;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Letters;
using Scenes._10_PlayerScene.Scripts;
using Scenes._50_Minigames._58_MiniRacingGame.Scripts;
using Scenes.Minigames.LetterGarden.Scripts.Gamemodes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;

namespace Scenes.Minigames.LetterGarden.Scripts
{

    public class ActiveLetterHandler : MonoBehaviour
    {
        [SerializeField] private SymbolManager symbolManager;
        [SerializeField] private Transform splineHolder;
        [SerializeField] private BeeMovement bee;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private GameObject helperBee;
        [SerializeField] private GameObject activeHelperBee;
        public AudioClip letterSound;
        float defaultBeeSpeed;
        bool helperBeeActive = true;
        private List<SplineSymbolDataHolder> splines = new();
        public SplineSymbolDataHolder currentSymbol;
        private int currentSymbolIndex = 0;
        [SerializeField] GameObject coinObject;

         public TextMeshProUGUI descriptionText;

        private LettergardenGameMode gamemode;
        public string oldLetter;

        /// <summary>
        /// used for testing only!!!!
        /// </summary>
        private void Start () //TODO remove and replace when we got a loading scene
        {
            
        }
        /// <summary>
        /// call to start the game
        /// </summary>
        public void StartGame(LettergardenGameMode gameMode, IGameRules gameRules)
        {
            this.gamemode = gameMode;

            gamemode.SetUpGameModeDescription(this);

            symbolManager.StartLoad();
            splines = gameMode.GetSymbols(5, gameRules);
            if (splines.Count <= 0) return;//end game

            currentSymbol = splines[0];
            splines.RemoveAt(0);
            bee.NextLetter(currentSymbol.splineContainer);
            defaultBeeSpeed = bee.speed;
            letterSound = LetterAudioManager.GetAudioClipFromLetter(currentSymbol.symbol.ToString() + 1);
            //Disables the bee's movement and if it is active adds an extra bee which rotates around it self at the endpoint of the spline
            if(!gameMode.UseBee())
            {
                bee.speed = 0;
                
                if(helperBeeActive)
                {
                    activeHelperBee = Instantiate(helperBee, (Vector3)currentSymbol.splineContainer.EvaluatePosition(0, 1), Quaternion.identity);
                    activeHelperBee.GetComponent<BeeMovement>().rotateInPlace = true;
                }
            }
        }

        public void Update()
        {
            if (letterSound != null && Input.GetKeyDown(KeyCode.Space) && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(letterSound);
            }
        }

        /// <summary>
        /// cheaks if the drawing given is good and handles sending the bee to the next letter/segment of the letter.
        /// </summary>
        /// <param name="dwaing">the line the player has drawn</param>
        public bool CheakDwaingQualaty(LineRenderer dwaing)
        {
            Vector3 offSet = new Vector3(0, 0, 0);
            if(SymbolManager.capitalLetters.ContainsValue(currentSymbol.splineContainer))
            {
                offSet = SymbolManager.capitalLettersObjects[currentSymbol.symbol].transform.position;
            }
            else if(SymbolManager.lowercaseLetters.ContainsValue(currentSymbol.splineContainer))
            {
                offSet = SymbolManager.lowercaseLettersObjects[currentSymbol.symbol].transform.position;
            }
            if (LineSegmentEvaluator.EvaluateSpline(currentSymbol.splineContainer[currentSymbolIndex],dwaing, offSet))
            {
                currentSymbolIndex++;
                if(!bee.NextSplineInLetter())
                {
                    PlayerEvents.RaiseGoldChanged(1);
                    PlayerEvents.RaiseXPChanged(1);
                    oldLetter = currentSymbol.symbol.ToString();
                    GameManager.Instance.PlayerData.CollectedLetters.Add(currentSymbol.symbol);
                    GameManager.Instance.DynamicDifficultyAdjustmentManager.UpdateLanguageUnitWeight(currentSymbol.symbol.ToString(), true);
                    Instantiate(coinObject);
                    //StartCoroutine(TakeScreenShot());
                    
                    //next letter
                    currentSymbolIndex = 0;
                    if(splines.Count <= 0) return true;//end game

                    currentSymbol = splines[0];
                    splines.RemoveAt(0);
                    bee.NextLetter(currentSymbol.splineContainer);
                    //Removes the currently active helper bee if it exists
                    if(activeHelperBee != null)
                    {
                        Destroy(activeHelperBee);
                        activeHelperBee = null;
                    }
                    letterSound = LetterAudioManager.GetAudioClipFromLetter(currentSymbol.symbol.ToString() + 1);
                    //Disables the bee's movement and if it is active adds an extra bee which rotates around it self at the endpoint of the spline
                    if(!gamemode.UseBee())
                    {
                        bee.speed = 0;
                        
                        if(helperBeeActive)
                        {
                            activeHelperBee = Instantiate(helperBee, (Vector3)currentSymbol.splineContainer.EvaluatePosition(0, 1), Quaternion.identity);
                            activeHelperBee.GetComponent<BeeMovement>().rotateInPlace = true;
                        }
                    }
                    else 
                    {
                        bee.speed = defaultBeeSpeed;
                        letterSound = null;
                    }
                    return true;
                }   
                //Removes the currently active helper bee if it exist and replaces it with a new one at the end point of the current spline
                else if(activeHelperBee != null && helperBeeActive)
                {
                    Destroy(activeHelperBee);
                    activeHelperBee = null;
                    activeHelperBee = Instantiate(helperBee, (Vector3)currentSymbol.splineContainer.EvaluatePosition(currentSymbolIndex, 1), Quaternion.identity);
                    activeHelperBee.GetComponent<BeeMovement>().rotateInPlace = true;
                }
                //next Spline in container
                return true;
            }
            
            dwaing.positionCount = 0;
            return false;
        }

        /// <summary>
        /// Checks whether there are splines remaining
        /// </summary>
        /// <returns>whether there are splines remaining</returns>
        public bool GameOver()
        {
            return splines.Count == 0;
        }
    }

    public class SplineSymbolDataHolder
    {
        public GameObject splineObject;
        public SplineContainer splineContainer;
        public char symbol;

        /// <summary>
        /// this is a class for holding information about a Symbol for LetterGarden
        /// </summary>
        /// <param name="splineObject">a refrence to the instanc of a symbol</param>
        /// <param name="splineContainer">the splineContainer for the symbol</param>
        /// <param name="Symbol">the spisific symbol(used to update player after finishing a dwaing)</param>
        public SplineSymbolDataHolder(GameObject splineObject, SplineContainer splineContainer, char Symbol)
        {
            this.splineObject = splineObject;
            this.splineContainer = splineContainer;
            this.symbol = Symbol;
        }
    
    }
}