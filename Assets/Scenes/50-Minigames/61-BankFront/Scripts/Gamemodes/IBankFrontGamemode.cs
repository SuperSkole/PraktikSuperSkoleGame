using System.Collections;
using System.Collections.Generic;
using CORE.Scripts;
using UnityEngine;

/// <summary>
/// Base interface for gamemodes in the bank front entrance minigame
/// </summary>
public interface IBankFrontGamemode:IGenericGameMode
{
    /// <summary>
    /// Clears the board of coins
    /// </summary>
    void ClearCurrentCustomersCoins();
    /// <summary>
    /// Creates a fake coin
    /// </summary>
    void CreateFakeCoin();
    /// <summary>
    /// Creates a real coin
    /// </summary>
    /// <param name="prefabIndex">Which coin should be created</param>
    void CreateRealCoin(int prefabIndex);
    /// <summary>
    /// Returns the chance per valid coin
    /// </summary>
    /// <returns>the chance per coin as number between 0 and 100</returns>
    float GetChance();
    /// <summary>
    /// Returns the coins on the board
    /// </summary>
    /// <returns></returns>
    List<Coin> GetCurrentCustomersCoins();
    /// <summary>
    /// Returns how many active coins are active
    /// </summary>
    /// <returns></returns>
    int GetRealCoinCount();
    /// <summary>
    /// Toggles various ui elements on or off
    /// </summary>
    void HandleUIElements();
    /// <summary>
    /// tells the gamemode to set up the lists of objects it should use and sends a reference to the bank manager
    /// </summary>
    /// <param name="bankManager">The bankmanager of the game</param>
    void RequestGameObjectsToBeUsed(BankManager bankManager);
    /// <summary>
    /// Checks how correct the players guess is
    /// </summary>
    /// <param name="playerGuess">The latest data from the playerGuess field</param>
    /// <returns>How correct the player is on a scale from 0-2 where 0 = completely wrong 1 = partially correct and 2 = completely correct</returns>
    int Validate(int playerGuess);

    string GetHintText();
}
