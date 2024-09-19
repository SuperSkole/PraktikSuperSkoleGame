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
    public GameObject unsortedTray;
    [SerializeField]private Image unsortedTraybackground;
    public GameObject sortedTray;
    [SerializeField]private Image sortedTrayBackground;
    [SerializeField]private TMP_InputField inputField;
    [SerializeField]private GameObject playerPrison;
    [SerializeField]private GameObject coinPrefab;
    [SerializeField]private TextMeshProUGUI lives;
    [SerializeField]private TextMeshProUGUI gameOverText;
    public GameObject unifiedField;

    public IBankFrontGamemode gamemode;

    [SerializeField]private int playerGuess = -1;

    private int completedGames = 0;
    private float mistakes = 0;


    /// <summary>
    /// Starts up the game if it is currently not going
    /// </summary>
    void Update()
    {
        if(gamemode.GetCurrentCustomersCoins().Count == 0)
        {
            
            lives.text = "3/3 liv tilbage";
            gameOverText.text = "";
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
    }
    /// <summary>
    /// Checks if the player has sorted the coins correctly and if their total for the correct ones is correct
    /// </summary>
    public void Validate()
    {
        
        //Checks if the coins have been sorted correctly and calculates the total value of the correct ones
        int result = gamemode.Validate(playerGuess);
        //Ends the current game if the player sorted correctly and calculated the value of the correct conins correctly
        if(result == 2)
        {
            sortedTrayBackground.color = Color.green;
            unsortedTraybackground.color = Color.green;
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
            sortedTrayBackground.color = Color.yellow;
            unsortedTraybackground.color = Color.yellow;
        }
        //Colors the tray backgrounds red if neither the sorting or the players guess is correct
        else 
        {
            mistakes++;
            UpdateLivesDisplay();
            sortedTrayBackground.color = Color.red;
            unsortedTraybackground.color = Color.red;
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
        completedGames++;
        if(completedGames >= 5)
        {
            gameOverText.text = "Du vandt du sorterede mønterne korrekt og udregnede deres værdi 5 gange";
        }
        yield return new WaitForSeconds(5);
        gamemode.ClearCurrentCustomersCoins();
        sortedTrayBackground.color = Color.white;
        unsortedTraybackground.color = Color.white;
        if(completedGames >= 5)
        {
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
    /// Updates the playerguess then the player types an integer into the player guess field
    /// </summary>
    public void UpdateGuess()
    {
        bool res = Int32.TryParse(inputField.text, out int number);
        if (res)
        {
            playerGuess = number;
        }
    }

    /// <summary>
    /// Updates the display of the players remaining lives
    /// </summary>
    private void UpdateLivesDisplay()
    {
        if(mistakes <= 3)
        {
            lives.text = 3 - mistakes + "/3 liv tilbage";
        }
        else
        {
            lives.text = "0/3 liv tilbage";
        }
    }

    /// <summary>
    /// Returns the two coin lists
    /// </summary>
    /// <returns>Returns the validCoins list and the fakeCoins list</returns>
    public (List<GameObject>, List<GameObject>) GetCoins()
    {
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
}
