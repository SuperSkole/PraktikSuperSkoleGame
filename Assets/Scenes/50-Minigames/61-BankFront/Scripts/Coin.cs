using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]private bool validCoin;
    [SerializeField]private int value;

    public GameObject sortedTray;
    public GameObject unsortedTray;

    private GameObject currentTray;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public void SetTrays(GameObject unsortedTray, GameObject sortedTray) {
        this.sortedTray = sortedTray;
        this.unsortedTray = unsortedTray;
        currentTray = unsortedTray;
    }

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