using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bank front gamemode where the player should sort coins and count how much the correct ones are worth
/// </summary>
public class SortAnimals : IBankFrontGamemode
{
    private float realCoinChance = 50;
    private List<GameObject> validCoins;
    private List<GameObject> fakeCoins;
    private BankManager bankManager;
    private string correctAnimal;

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
        c.SetTrays(bankManager.unsortedTray, bankManager.sortedTray, bankManager.dragArea);
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
        c.validCoin = true;
        c.SetTrays(bankManager.unsortedTray, bankManager.sortedTray, bankManager.dragArea);
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
        return "Du sorterede dyrene forkert";
    }

    public string GetHintText()
    {
        return "Find alle " + correctAnimal;
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
        bankManager.unifiedField.SetActive(false);
        bankManager.validCoinsField.SetActive(false);
    }

    /// <summary>
    /// Sets the reference to the bankmanager and sets the validCoins and fakeCoins based on their respective lists in the bankmanager
    /// </summary>
    /// <param name="bankManager">the bankmanager of the game</param>
    public void RequestGameObjectsToBeUsed(BankManager bankManager)
    {
        this.bankManager = bankManager;
        List<GameObject> coins = bankManager.GetAnimalCoins();
        correctAnimal = bankManager.multipleImagesAnimals[Random.Range(0, bankManager.multipleImagesAnimals.Count)];
        validCoins = new List<GameObject>();
        fakeCoins = new List<GameObject>();
        foreach(GameObject coin in coins)
        {
            if(coin.name.Contains(correctAnimal))
            {
                validCoins.Add(coin);
            }
            else
            {
                fakeCoins.Add(coin);
            }
        }
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
