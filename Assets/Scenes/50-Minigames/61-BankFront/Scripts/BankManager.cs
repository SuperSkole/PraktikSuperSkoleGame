using System;
using System.Collections;
using System.Collections.Generic;
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
public class BankManager : MonoBehaviour
{
    [SerializeField]private List<GameObject>validCoins;
    [SerializeField]private List<GameObject>fakeCoins;
    [SerializeField]private GameObject unsortedTray;
    [SerializeField]private Image unsortedTraybackground;
    [SerializeField]private GameObject sortedTray;
    [SerializeField]private Image sortedTrayBackground;
    [SerializeField]private TMP_InputField inputField;
    public List<Coin> currentCustomersCoins = new List<Coin>();
    [SerializeField]private GameObject coinPrefab;

    [SerializeField]private float realCoinPercentage = 80;

    [SerializeField]private int playerGuess = -1;

    private int completedGames = 0;
    private int mistakes = 0;

    /// <summary>
    /// Starts up the game if it is currently not going
    /// </summary>
    void Update()
    {
        if(currentCustomersCoins.Count == 0)
        {
            //finds out how many coins the customer have and then generates them
            int amount = Random.Range(1, 20);
            float chancePerCoin = realCoinPercentage / validCoins.Count;
            for(int i = 0; i < amount; i++)
            {
                int coinRoll = Random.Range(0, 100);
                bool realCoin = false;
                //Checks if the roll gets a real coin. If it does it finds out which and then generates the coin and setting up its various variables
                for(int j = 0; j < validCoins.Count; j++)
                {
                    if(coinRoll < (j + 1) * chancePerCoin)
                    {
                        GameObject coin = Instantiate(validCoins[j]);
                        coin.transform.SetParent(unsortedTray.transform);
                        Coin c = coin.GetComponent<Coin>();
                        c.SetTrays(unsortedTray, sortedTray);
                        currentCustomersCoins.Add(c);
                        realCoin = true;
                        break;
                    }
                }
                //Does the same but for fake coins
                if(!realCoin)
                {
                    coinRoll = Random.Range(0, fakeCoins.Count);
                    GameObject coin = Instantiate(fakeCoins[coinRoll]);
                    coin.transform.SetParent(unsortedTray.transform);
                    Coin c = coin.GetComponent<Coin>();
                    c.SetTrays(unsortedTray, sortedTray);
                    currentCustomersCoins.Add(c);
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
        bool correct = true;
        int currentSum = 0;
        foreach(Coin coin in currentCustomersCoins)
        {
            (bool, int) validateData = coin.placedCorrectly();
            currentSum += validateData.Item2;
            if(!validateData.Item1)
            {
                correct = false;
            }
        }
        //Ends the current game if the player sorted correctly and calculated the value of the correct conins correctly
        if(correct && playerGuess == currentSum)
        {
            sortedTrayBackground.color = Color.green;
            unsortedTraybackground.color = Color.green;
            PlayerEvents.RaiseGoldChanged(1);
            PlayerEvents.RaiseXPChanged(1);
            Instantiate(coinPrefab);
            StartCoroutine(Restart());
        }
        //Changes the background color of the trays to yellow if either the guess or the sorting is correct
        else if(correct || playerGuess == currentSum)
        {
            sortedTrayBackground.color = Color.yellow;
            unsortedTraybackground.color = Color.yellow;
        }
        //Colors the tray backgrounds red if neither the sorting or the players guess is correct
        else 
        {
            sortedTrayBackground.color = Color.red;
            unsortedTraybackground.color = Color.red;
        }
    }

    /// <summary>
    /// Waits a bit and then prepares for a restart of the game. If enough games have been completed the game ends
    /// </summary>
    /// <returns></returns>
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(5);
        completedGames++;
        for(int i = 0; i < currentCustomersCoins.Count; i++)
        {
            Destroy(currentCustomersCoins[i].gameObject);
        }
        currentCustomersCoins.Clear();
        sortedTrayBackground.color = Color.white;
        unsortedTraybackground.color = Color.white;
        if(completedGames >= 5)
        {
            SwitchScenes.SwitchToMainWorld();
        }
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
}
