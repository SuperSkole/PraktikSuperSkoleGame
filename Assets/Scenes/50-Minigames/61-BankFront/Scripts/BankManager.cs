using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BankManager : MonoBehaviour
{
    [SerializeField]private List<GameObject>validCoins;
    [SerializeField]private List<GameObject>fakeCoins;
    [SerializeField]private GameObject unsortedTray;
    [SerializeField]private Image unsortedTraybackground;
    [SerializeField]private GameObject sortedTray;
    [SerializeField]private Image sortedTrayBackground;
    [SerializeField]private TMP_InputField inputField;
    [SerializeField]private List<Coin> currentCustomersCoins = new List<Coin>();

    [SerializeField]private float realCoinPercentage = 80;

    [SerializeField]private int playerGuess = -1;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(currentCustomersCoins.Count == 0)
        {
            int amount = Random.Range(1, 20);
            float chancePerCoin = realCoinPercentage / validCoins.Count;
            for(int i = 0; i < amount; i++)
            {
                int coinRoll = Random.Range(0, 100);
                Debug.Log("coinroll: " + coinRoll);
                Debug.Log("chancePerCoin:" + chancePerCoin);
                bool realCoin = false;
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
    public void Validate()
    {
        bool correct = true;
        int currentSum = 0;
        foreach(Coin coin in currentCustomersCoins)
        {
            (bool, int) validateData = coin.placedCorrectly();
            currentSum += validateData.Item2;
            if(!validateData.Item1)
            {
                correct = false;
                break;
            }
        }
        Debug.Log("currentSum: " + currentSum);
        if(correct && playerGuess == currentSum)
        {
            sortedTrayBackground.color = Color.green;
            unsortedTraybackground.color = Color.green;
            StartCoroutine(Restart());
        }
        else 
        {
            sortedTrayBackground.color = Color.red;
            unsortedTraybackground.color = Color.red;
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(5);
        for(int i = 0; i < currentCustomersCoins.Count; i++)
        {
            Destroy(currentCustomersCoins[i].gameObject);
        }
        currentCustomersCoins.Clear();
        sortedTrayBackground.color = Color.white;
        unsortedTraybackground.color = Color.white;
    }

    public void UpdateGuess()
    {
        int number;
        bool res = Int32.TryParse(inputField.text, out number);
        if(res)
        {
            playerGuess = number;
        }
    }
}
