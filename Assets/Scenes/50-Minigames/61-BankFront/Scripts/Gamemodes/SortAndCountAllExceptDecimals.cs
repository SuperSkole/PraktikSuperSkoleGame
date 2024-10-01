using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bank front gamemode where the player should sort coins and count how much the correct ones are worth
/// </summary>
public class SortAndCountAllExceptDecimals : IBankFrontGamemode
{
    private SortAndCountAll sortAndCountAll = new SortAndCountAll();
    /// <summary>
    /// Clears the active coins
    /// </summary>
    public void ClearCurrentCustomersCoins()
    {
        sortAndCountAll.ClearCurrentCustomersCoins();
    }

    /// <summary>
    /// Create a fake coin based on the ones in the fakeCoins list
    /// </summary>
    public void CreateFakeCoin()
    {
        sortAndCountAll.CreateFakeCoin();
    }

    /// <summary>
    /// Creates a real coin based on the ones in the validCoins list
    /// </summary>
    /// <param name="prefabIndex">The index of the desired coin</param>
    public void CreateRealCoin(int prefabIndex)
    {
        sortAndCountAll.CreateRealCoin(prefabIndex);
    }

    /// <summary>
    /// Gets the chance per coin
    /// </summary>
    /// <returns>the chance per coin</returns>
    public float GetChance()
    {
        return sortAndCountAll.GetChance();
    }

    /// <summary>
    /// Returns the list of active coins
    /// </summary>
    /// <returns></returns>
    public List<Coin> GetCurrentCustomersCoins()
    {
        return sortAndCountAll.GetCurrentCustomersCoins();
    }

    public string GetErrorExplainText()
    {
        return sortAndCountAll.GetErrorExplainText();
    }

    public string GetHintText()
    {
        return sortAndCountAll.GetHintText();
    }

    /// <summary>
    /// Returns how many types of valid coins there are
    /// </summary>
    /// <returns></returns>
    public int GetRealCoinCount()
    {
        return sortAndCountAll.GetRealCoinCount();
    }

    /// <summary>
    /// deactivates the unified field
    /// </summary>
    public void HandleUIElements()
    {
        sortAndCountAll.HandleUIElements();
    }

    /// <summary>
    /// Sets the reference to the bankmanager and sets the validCoins and fakeCoins based on their respective lists in the bankmanager
    /// </summary>
    /// <param name="bankManager">the bankmanager of the game</param>
    public void RequestGameObjectsToBeUsed(BankManager bankManager)
    {
        sortAndCountAll.bankManager = bankManager;
        (List<GameObject>, List<GameObject>) coins = bankManager.GetCoins(false, false);
        sortAndCountAll.validCoins = coins.Item1;
        sortAndCountAll.fakeCoins = coins.Item2;
    }

    /// <summary>
    /// Checks if the players answer is correct
    /// </summary>
    /// <param name="playerGuess">the value of the playerGuess inputfield</param>
    /// <returns></returns>
    public int Validate(float playerGuess)
    {
        return sortAndCountAll.Validate(playerGuess);
    }
}
