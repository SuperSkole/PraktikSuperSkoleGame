using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool validCoin;
    private int value;

    [SerializeField]GameObject sortedTray;
    [SerializeField]GameObject unsortedTray;

    GameObject currentTray;

    // Start is called before the first frame update
    void Start()
    {
        currentTray = unsortedTray;
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
}
