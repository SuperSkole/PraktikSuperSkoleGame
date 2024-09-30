using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bank front gamemode where the player should sort coins and count how much the correct ones are worth
/// </summary>
public class SortAndCount : IBankFrontGamemode
{
    private float realCoinChance = 80;
    private List<GameObject> validCoins;
    private List<GameObject> fakeCoins;
    private BankManager bankManager;

    private List<Coin> currentCustomersCoins = new List<Coin>();

    /// <summary>
    /// Clears the active coins
    /// </summary>
    public void ClearCurrentCustomersCoins()
    {
        for(int i = 0; i < currentCustomersCoins.Count; i++)
        {
            bankManager.DestroyObject(currentCustomersCoins[i].gameObject);
        }
        currentCustomersCoins.Clear();
    }

    /// <summary>
    /// Create a fake coin based on the ones in the fakeCoins list
    /// </summary>
    public void CreateFakeCoin()
    {
        int coinRoll = Random.Range(0, fakeCoins.Count);
        GameObject coin = bankManager.InstantiateObject(fakeCoins[coinRoll]);
        coin.transform.SetParent(bankManager.unsortedTray.transform);
        coin.transform.localScale = new Vector3(1, 1, 1);
        Coin c = coin.GetComponent<Coin>();
        c.SetTrays(bankManager.unsortedTray, bankManager.sortedTray);
        currentCustomersCoins.Add(c);
    }

    /// <summary>
    /// Creates a real coin based on the ones in the validCoins list
    /// </summary>
    /// <param name="prefabIndex">The index of the desired coin</param>
    public void CreateRealCoin(int prefabIndex)
    {
        GameObject coin = bankManager.InstantiateObject(validCoins[prefabIndex]);
        coin.transform.SetParent(bankManager.unsortedTray.transform);
        coin.transform.localScale = new Vector3(1, 1, 1);
        Coin c = coin.GetComponent<Coin>();
        c.SetTrays(bankManager.unsortedTray, bankManager.sortedTray);
        currentCustomersCoins.Add(c);
    }

    /// <summary>
    /// Gets the chance per coin
    /// </summary>
    /// <returns>the chance per coin</returns>
    public float GetChance()
    {
        return realCoinChance / validCoins.Count;
    }

    /// <summary>
    /// Returns the list of active coins
    /// </summary>
    /// <returns></returns>
    public List<Coin> GetCurrentCustomersCoins()
    {
        return currentCustomersCoins;
    }

    public string GetErrorExplainText()
    {
        return "du sorterede eller talte m\u00F8nterne forkert";
    }

    public string GetHintText()
    {
        return "Udregn v\u00e6rdien af \u00e6gte m\u00F8nter";
    }

    /// <summary>
    /// Returns how many types of valid coins there are
    /// </summary>
    /// <returns></returns>
    public int GetRealCoinCount()
    {
        return validCoins.Count;
    }

    /// <summary>
    /// deactivates the unified field
    /// </summary>
    public void HandleUIElements()
    {
        bankManager.unifiedField.transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the reference to the bankmanager and sets the validCoins and fakeCoins based on their respective lists in the bankmanager
    /// </summary>
    /// <param name="bankManager">the bankmanager of the game</param>
    public void RequestGameObjectsToBeUsed(BankManager bankManager)
    {
        this.bankManager = bankManager;
        (List<GameObject>, List<GameObject>) coins = bankManager.GetCoins();
        validCoins = coins.Item1;
        fakeCoins = coins.Item2;
    }

    /// <summary>
    /// Checks if the players answer is correct
    /// </summary>
    /// <param name="playerGuess">the value of the playerGuess inputfield</param>
    /// <returns></returns>
    public int Validate(int playerGuess)
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
        int res = 0;
        if(correct)
        {
            res++;
        }
        if(currentSum == playerGuess)
        {
            res++;
        }
        return res;
    }
}
