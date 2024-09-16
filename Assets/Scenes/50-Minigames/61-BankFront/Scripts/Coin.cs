using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

/// <summary>
/// class to handle coins in the bank main entrance minigame
/// </summary>
public class Coin : MonoBehaviour
{
    [SerializeField]private bool validCoin;
    [SerializeField]private int value;

    public GameObject sortedTray;
    public GameObject unsortedTray;

    private GameObject currentTray;

    /// <summary>
    /// Changes which tray the coin is currently in
    /// </summary>
    public void ChangeTray()
    {
        if (currentTray == sortedTray)
        {
            transform.SetParent(unsortedTray.transform);
            currentTray = unsortedTray;
        }
        else
        {
            transform.SetParent(sortedTray.transform);
            currentTray = sortedTray;
        }
    }

    /// <summary>
    /// Sets up the various tray variables
    /// </summary>
    /// <param name="unsortedTray"></param>
    /// <param name="sortedTray"></param>
    public void SetTrays(GameObject unsortedTray, GameObject sortedTray) {
        this.sortedTray = sortedTray;
        this.unsortedTray = unsortedTray;
        currentTray = unsortedTray;
    }

    /// <summary>
    /// Checks if the coin has been placed correctly
    /// </summary>
    /// <returns>whether the coin have been placed correctly and its value</returns>
    public (bool, int) placedCorrectly()
    {
        if(validCoin && currentTray == sortedTray)
        {
            return (true, value);
        }
        else if(!validCoin && currentTray == unsortedTray)
        {
            return (true, 0);
        }
        else 
        {
            return (false, 0);
        }
    }
}