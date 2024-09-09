using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankManager : MonoBehaviour
{
    private List<int> validCoinValues = new List<int>()
    {
        1, 2, 5, 10, 20
    };
    private List<int> fakeCoinValues = new List<int>()
    {
        2, 3, 0
    };
    [SerializeField]private List<Texture2D>validCoints;
    [SerializeField]private List<Texture2D>fakeCoins;

    [SerializeField]private List<(int, bool)> currentCustomersCoins = new List<(int, bool)>();

    [SerializeField]private float realCoinPercentage = 80;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentCustomersCoins.Count == 0)
        {
            int amount = Random.Range(1, 10);
            float chancePerCoin = realCoinPercentage / validCoinValues.Count;
            for(int i = 0; i < amount; i++)
            {
                int coinRoll = Random.Range(0, 100);
                bool realCoin = false;
                for(int j = 0; j < validCoinValues.Count; j++)
                {
                    if(coinRoll < (j + 1) * chancePerCoin)
                    {
                        currentCustomersCoins.Add((validCoinValues[j], true));
                        realCoin = true;
                        break;
                    }
                }
                if(!realCoin)
                {
                    coinRoll = Random.Range(0, fakeCoinValues.Count);
                    currentCustomersCoins.Add((fakeCoinValues[coinRoll], false));
                }
            }
        }
    }
}
