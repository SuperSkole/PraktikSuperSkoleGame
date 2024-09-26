using System;
using System.Collections;
using System.Collections.Generic;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes;
using Scenes._10_PlayerScene.Scripts;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// Manager class for the bank main entrance minigame
/// </summary>
public class BankManager : MonoBehaviour, IMinigameSetup
{
    [SerializeField]private List<GameObject>validCoins;
    
    [SerializeField]private List<GameObject>fakeCoins;
    [SerializeField]private List<GameObject>animals;
    public List<string> multipleImagesAnimals = new List<string>()
    {
        "cow", "mouse", "cat"
    };
    public GameObject unsortedTray;
    [SerializeField]private Image unsortedTraybackground;
    public GameObject sortedTray;
    [SerializeField]private Image sortedTrayBackground;
    [SerializeField]private GameObject playerPrison;
    [SerializeField]private GameObject coinPrefab;
    [SerializeField]private TextMeshProUGUI lives;
    [SerializeField]private TextMeshProUGUI gameOverText;
    [SerializeField]private TextMeshProUGUI hintText;
    [SerializeField]private NumberDisplay numberDisplay;
    [SerializeField]private ErrorDisplay errorDisplay;
    [SerializeField]private List<GameObject>triangleCoins;
    [SerializeField]private List<GameObject>squareCoins;
    [SerializeField]private List<GameObject>roundCoins;
    [SerializeField]private List<GameObject>pentagonCoins;
    [SerializeField]private List<GameObject>hexagonCoins;
    public GameObject validCoinsField;
    [SerializeField]private GameObject validCoinsContainer;
    public GameObject unifiedField;
    [SerializeField]private Image unifiedFieldBackground;
    [SerializeField]private ErrorExplainer mistakeExplainer;

    private Customer currentCustomer;

    public IBankFrontGamemode gamemode;

    private int completedGames = 0;
    private float mistakes = 0;


    void Start()
    {
        gameOverText.text = "";
    }

    /// <summary>
    /// Starts up the game if it is currently not going
    /// </summary>
    void Update()
    {
        if(gamemode == null)
        {
            SetupGame(new SortAndCount(), null);
        }
    }

    /// <summary>
    /// Creates the money used for the current game
    /// </summary>
    /// <param name="customer">The current customer</param>
    public void HandOverMoney(Customer customer)
    {
        errorDisplay.Reset();
        currentCustomer = customer;
        lives.text = "3/3 liv";
        mistakes = 0;
        //finds out how many coins the customer have and then generates them
        int amount = Random.Range(1, 20);
        float chancePerCoin = gamemode.GetChance();
        for(int i = 0; i < amount; i++)
        {
            int coinRoll = Random.Range(0, 100);
            bool realCoin = false;
            //Checks if the roll gets a real coin. If it does it finds out which and then generates the coin and setting up its various variables
            for(int j = 0; j < gamemode.GetRealCoinCount(); j++)
            {
                if(coinRoll < (j + 1) * chancePerCoin)
                {
                    gamemode.CreateRealCoin(j);
                    realCoin = true;
                    break;
                }
            }
            //Does the same but for fake coins
            if(!realCoin)
            {
                gamemode.CreateFakeCoin();
            }
        }
    }
    /// <summary>
    /// Checks if the player has sorted the coins correctly and if their total for the correct ones is correct
    /// </summary>
    public void Validate()
    {
        
        //Checks if the coins have been sorted correctly and calculates the total value of the correct ones
        int result = gamemode.Validate(numberDisplay.GetNumber());
        //Ends the current game if the player sorted correctly and calculated the value of the correct conins correctly
        if(result < 2)
        {
            mistakeExplainer.gameObject.SetActive(true);
            mistakeExplainer.AddExplanation(gamemode.GetErrorExplainText());
            
        }
        if(result == 2)
        {
            errorDisplay.Correct();
            PlayerEvents.RaiseGoldChanged(1);
            PlayerEvents.RaiseXPChanged(1);
            Instantiate(coinPrefab);
            StartCoroutine(Restart());
        }
        //Changes the background color of the trays to yellow if either the guess or the sorting is correct
        else if(result == 1)
        {
            mistakes += 0.5f;
            UpdateLivesDisplay();
            errorDisplay.PartialCorrect();
        }
        //Colors the tray backgrounds red if neither the sorting or the players guess is correct
        else 
        {
            mistakes++;
            UpdateLivesDisplay();
            errorDisplay.Incorrect();
        }
        if(mistakes >= 3)
        {
            StartCoroutine(LostGame());
        }
    }

    /// <summary>
    /// Waits a bit and then prepares for a restart of the game. If enough games have been completed the game ends
    /// </summary>
    /// <returns></returns>
    IEnumerator Restart()
    {
        /*completedGames++;
        if(completedGames >= 5)
        {
            gameOverText.text = "Du vandt du sorterede mønterne korrekt og udregnede deres værdi 5 gange";
        }*/
        gamemode.ClearCurrentCustomersCoins();
        currentCustomer.PrepareToLeaveBank();
        currentCustomer = null;
        if(completedGames >= 5)
        {
            yield return new WaitForSeconds(5);
            SwitchScenes.SwitchToMainWorld();
        }
    }

    /// <summary>
    /// Sets the gameover text and then waits a bit before ending the game
    /// </summary>
    /// <returns></returns>
    IEnumerator LostGame()
    {
        gameOverText.text = "Du tabte. Du lavede for mange fejl";
        yield return new WaitForSeconds(5);
        SwitchScenes.SwitchToMainWorld();
    }

    /// <summary>
    /// Updates the display of the players remaining lives
    /// </summary>
    private void UpdateLivesDisplay()
    {
        if(mistakes <= 3)
        {
            lives.text = 3 - mistakes + "/3 liv";
        }
        else
        {
            lives.text = "0/3 liv";
        }
    }

    /// <summary>
    /// Returns the two coin lists
    /// </summary>
    /// <returns>Returns the validCoins list and the fakeCoins list</returns>
    public (List<GameObject>, List<GameObject>) GetCoins()
    {
        validCoins.Clear();
        fakeCoins.Clear();
        List<List<GameObject>> coinTypes = new List<List<GameObject>>()
        {
            triangleCoins,
            squareCoins,
            roundCoins,
            pentagonCoins,
            hexagonCoins
        };
        for (int i = 0; i < triangleCoins.Count; i++)
        {
            int coinList = Random.Range(0, coinTypes.Count);
            GameObject validCoin = coinTypes[coinList][i];
            validCoin.GetComponent<Coin>().validCoin = true;
            foreach(GameObject coin in coinTypes[coinList])
            {
                if(coin == validCoin)
                {
                    validCoins.Add(coin);
                }
                else
                {
                    fakeCoins.Add(coin);
                }
            }
            coinTypes.RemoveAt(coinList);
        }
        for(int i = 0; i < validCoins.Count; i++)
        {
            GameObject validCoin = Instantiate(validCoins[i]);
            validCoin.transform.SetParent(validCoinsContainer.transform);
            validCoin.transform.localScale = new Vector3(1, 1, 1);
        }
        return (validCoins, fakeCoins);
    }

    /// <summary>
    /// Moves the player out of the screen and sets up the given gamemode
    /// </summary>
    /// <param name="gameMode">The gamemode to be used</param>
    /// <param name="gameRules">currently not used</param>
    public void SetupGame(IGenericGameMode gameMode, IGameRules gameRules)
    {
        if(PlayerManager.Instance != null)
        {
            PlayerManager.Instance.PositionPlayerAt(playerPrison);
        }
        gamemode = (IBankFrontGamemode)gameMode;
        gamemode.RequestGameObjectsToBeUsed(this);
        gamemode.HandleUIElements();
        var tempColor = unsortedTraybackground.color;
        tempColor.a = 0;
        unsortedTraybackground.color = tempColor;
        sortedTrayBackground.color = tempColor;
        unifiedFieldBackground.color = tempColor;
        hintText.text = gamemode.GetHintText();
    }

    /// <summary>
    /// Instantiates the given object
    /// </summary>
    /// <param name="objectToBeInstantiated">the object which should be instantiated</param>
    /// <returns>the instantiated object</returns>
    public GameObject InstantiateObject(GameObject objectToBeInstantiated)
    {
        return Instantiate(objectToBeInstantiated);
    }

    /// <summary>
    /// Destroys the given object
    /// </summary>
    /// <param name="objectToBeDestroyed">The object which should be destroyed</param>
    public void DestroyObject(GameObject objectToBeDestroyed)
    {
        Destroy(objectToBeDestroyed);
    }
    public void DestroyComponent(MonoBehaviour component)
    {
        Destroy(component);
    }

    public List<GameObject> GetAnimalCoins()
    {
        return animals;
    }
}