using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// class to handle coins in the bank main entrance minigame
/// </summary>
public class Coin : MonoBehaviour
{
    public bool validCoin;
    [SerializeField]private int value;

    public Button button;

    public GameObject sortedTray;
    public GameObject unsortedTray;

    private GameObject currentTray;

    private GameObject dragArea;

    private bool drag = false;
    private bool beginToDrag = false;
    /// <summary>
    /// Changes which tray the coin is currently in
    /// </summary>
    public void ChangeTray()
    {
        if(drag)
        {
            float sortedTrayDistance = Vector3.Distance(transform.position, sortedTray.transform.position);
            float unsortedTrayDistance = Vector3.Distance(transform.position, unsortedTray.transform.position);
            if (unsortedTrayDistance <= sortedTrayDistance)
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
        else 
        {
            drag = true;
            beginToDrag = true;
            transform.SetParent(dragArea.transform);
        }
    }
    void Update()
    {
        if((drag && Input.GetKey(KeyCode.Mouse0)) || beginToDrag)
        {
            if(beginToDrag && Input.GetKey(KeyCode.Mouse0))
            {
                beginToDrag = false;
            }
            transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
        }
        else if(drag)
        {
            ChangeTray();
            drag = false;
        }
    }
    /// <summary>
    /// Sets up the various tray variables
    /// </summary>
    /// <param name="unsortedTray"></param>
    /// <param name="sortedTray"></param>
    public void SetTrays(GameObject unsortedTray, GameObject sortedTray, GameObject dragArea) {
        this.sortedTray = sortedTray;
        this.unsortedTray = unsortedTray;
        this.dragArea = dragArea;
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