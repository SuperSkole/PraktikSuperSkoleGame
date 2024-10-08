using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// class to handle coins in the bank main entrance minigame
/// </summary>
public class Coin : MonoBehaviour
{
    public bool validCoin;
    [SerializeField]private float value;
    [SerializeField]private AudioClip correctSound;
    [SerializeField]private AudioClip incorrectSound;
    private BankManager bankManager;

    public UnityEngine.UI.Button button;

    public GameObject sortedTray;
    public GameObject unsortedTray;

    private GameObject currentTray;

    private GameObject moveArea;

    private bool moveable = false;

    public bool showCoin = false;

    /// <summary>
    /// if moveable has not been activated it gets activated and the coin is transfered to the move zone. Otherwise it gets placed in the nearest tray if it is a valid coin. 
    /// Otherwise it gets moved back to the unsorted tray
    /// </summary>
    public void ChangeTray()
    {
        //determines which tray to place the coin in if it is currently being moved.
        if(!showCoin && moveable)
        {
            float sortedTrayDistance = Vector3.Distance(transform.position, sortedTray.transform.position);
            float unsortedTrayDistance = Vector3.Distance(transform.position, unsortedTray.transform.position);
            //Moves the coin to the unsorted tray if it is the closest
            if (unsortedTrayDistance <= sortedTrayDistance)
            {
                transform.SetParent(unsortedTray.transform);
                currentTray = unsortedTray;
                
            }
            //Moves it to the sorted tray if it is correct and a confirmation sound gets played
            else if (validCoin)
            {
                transform.SetParent(sortedTray.transform);
                currentTray = sortedTray;
                AudioManager.Instance.PlaySound(correctSound, SoundType.SFX);
            }
            //Moves the coin to the unsorted tray if it was closer to the sorted tray. The bankmanager is informed and an error sound is played
            else
            {
                bankManager.FakeCoinMoved();
                transform.SetParent(unsortedTray.transform);
                currentTray = unsortedTray;
                AudioManager.Instance.PlaySound(incorrectSound, SoundType.SFX);
            }
        }
        //Starts moving the coin
        else if(!showCoin)
        {
            moveable = true;
            transform.SetParent(moveArea.transform);
        }
    }
    /// <summary>
    /// if the coin is moveable its position gets updated to the current position of the mouse unless the left mousebutton is clicked again in which case it gets placed in the nearest tray. 
    /// </summary>
    void Update()
    {
        
        if(moveable && Input.GetKeyDown(KeyCode.Mouse0))
        {
            ChangeTray();
            moveable = false;
        }
        else if(moveable)
        {
            transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
        }
    }
    /// <summary>
    /// Sets up various reference variables
    /// </summary>
    /// <param name="bankManager">The bank manager of the game</param>
    public void SetTrays(BankManager bankManager) {
        sortedTray = bankManager.sortedTray;
        unsortedTray = bankManager.unsortedTray;
        moveArea = bankManager.dragArea;
        currentTray = unsortedTray;
        this.bankManager = bankManager;
    }

    /// <summary>
    /// Checks if the coin has been placed correctly
    /// </summary>
    /// <returns>whether the coin have been placed correctly and its value</returns>
    public (bool, float) placedCorrectly()
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