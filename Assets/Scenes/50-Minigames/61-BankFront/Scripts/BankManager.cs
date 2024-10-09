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
    [SerializeField]private List<GameObject>octagonCoins;
    [SerializeField]private List<GameObject>trapezoidCoins;
    public GameObject validCoinsField;
    [SerializeField]private GameObject validCoinsContainer;
    public GameObject unifiedField;
    [SerializeField]private Image unifiedFieldBackground;
    [SerializeField]private ErrorExplainer mistakeExplainer;
    [SerializeField]private HealthDisplay healthDisplay;
    [SerializeField]private AudioClip correctSound;
    [SerializeField]private AudioClip incorrectSound;
    public GameObject dragArea;

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
            SetupGame(new SortAndCountOnesAndTwos(), null);
        }
    }

    /// <summary>
    /// Creates the money used for the current game
    /// </summary>
    /// <param name="customer">The current customer</param>
    public void HandOverMoney(Customer customer)
    {
        if(currentCustomer == null)
        {
            errorDisplay.Reset();
            numberDisplay.ClearNumber();
            currentCustomer = customer;
            healthDisplay.SetHearts(3);
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
        else
        {
            Debug.LogError("Attempted to add coins to ongoing game");
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
            AudioManager.Instance.PlaySound(incorrectSound, SoundType.SFX);
        }
        if(result == 2)
        {
            errorDisplay.Correct();
            PlayerEvents.RaiseGoldChanged(1);
            PlayerEvents.RaiseXPChanged(1);
            Instantiate(coinPrefab);
            AudioManager.Instance.PlaySound(correctSound, SoundType.SFX);
            StartCoroutine(Restart());
        }
        //Changes the background color of the trays to yellow if either the guess or the sorting is correct
        else if(result == 1)
        {
            mistakes += 0.5f;
            healthDisplay.ChangeHearts(0.5f);
            errorDisplay.PartialCorrect();
        }
        //Colors the tray backgrounds red if neither the sorting or the players guess is correct
        else 
        {
            mistakes++;
            healthDisplay.ChangeHearts(1);
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
    /// Returns the two coin lists
    /// </summary>
    /// <param name="useDecimals">Whether coin values between 1 and 0 should be used</param>
    /// <param name="onlyUseOneAndTwo">Whether the only coin values should be 1 and 2</param>
    /// <returns></returns>
    public (List<GameObject>, List<GameObject>) GetCoins(bool useDecimals, bool onlyUseOneAndTwo)
    {
        validCoins.Clear();
        fakeCoins.Clear();
        List<List<GameObject>> coinTypes = new List<List<GameObject>>()
        {
            triangleCoins,
            squareCoins,
            roundCoins,
            pentagonCoins,
            hexagonCoins,
            octagonCoins,
            trapezoidCoins
        };
        int coinAmount = triangleCoins.Count;
        if(!useDecimals)
        {
            coinAmount -= 2;
        }
        if(onlyUseOneAndTwo)
        {
            coinAmount = 2;
        }
        //Finds a random coin shape for all used coins and adds the rest of the coins of that coin type as fake coins
        for (int i = 0; i < coinAmount; i++)
        {
            int coinList = Random.Range(0, coinTypes.Count);
            GameObject validCoin = coinTypes[coinList][i];
            validCoin.GetComponent<Coin>().validCoin = true;
            for(int j = 0; j < coinAmount; j++)
            {
                GameObject coin = coinTypes[coinList][j];
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
        //Adds any remaining coin shapes as fake coins
        while(coinTypes.Count > 0)
        {
            for(int j = 0; j < coinAmount; j++)
            {
                GameObject coin = coinTypes[0][j];
                fakeCoins.Add(coin);
                
            }
            coinTypes.RemoveAt(0);
        }
        //Adds a clone of all valid coins to the valid coins display
        for(int i = 0; i < validCoins.Count; i++)
        {
            GameObject validCoin = Instantiate(validCoins[i]);
            validCoin.transform.SetParent(validCoinsContainer.transform);
            validCoin.GetComponent<Button>().transition = Selectable.Transition.None;
            validCoin.GetComponent<Button>().interactable = false;
            validCoin.transform.localScale = new Vector3(1, 1, 1);
        }
        validCoinsContainer.transform.parent.gameObject.GetComponent<CoinInfoToggle>().ToggleAlwaysDisplay();
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

    /// <summary>
    /// adds half a mistake if the player attempts to move a fake coin to the sorted tray
    /// </summary>
    public void FakeCoinMoved()
    {
        mistakes += 0.5f;
        healthDisplay.ChangeHearts(0.5f);
    }
}