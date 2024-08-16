using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    //Display model
    public SkeletonGraphic skeletonGraphic;

    //The item names
    private string currentTopItem;
    private string currentMidItem;

    //!!Fill this one out with the amount the player has avaliable!!
    private int avaliableMoney = 200;

    private int currentPrice;

    //GameObjects
    [SerializeField] Image offBuyButton;
    private Shopoption currentShopOption;

    //Check if they can buy bool
    private bool ableToBuy = false;

    //Shop Option function
    public void Click(string itemName, int thisprice, Shopoption shopOption)
    {
        currentPrice = thisprice;
        currentShopOption = shopOption;

        if (itemName.Contains("TOP"))
        {
            if(currentTopItem != null)
            { 
                skeletonGraphic.Skeleton.SetAttachment(currentTopItem, null);
             }
            skeletonGraphic.Skeleton.SetAttachment(itemName, itemName);
            currentTopItem = itemName;
        }
        if (itemName.Contains("MID"))
        {
            if (currentMidItem != null)
            {
                skeletonGraphic.Skeleton.SetAttachment(currentMidItem, null);
            }
            skeletonGraphic.Skeleton.SetAttachment(itemName, itemName);
            currentMidItem = itemName;
        }

        //Check if the player can afford this

        if(avaliableMoney >= thisprice)
        {
            offBuyButton.gameObject.SetActive(false);
            ableToBuy = true;
        }
        if (avaliableMoney < thisprice)
        {
            offBuyButton.gameObject.SetActive(true);
            ableToBuy = false;
        }

    }

    //Buy Button Function
    public void Buying()
    {
        if (ableToBuy)
        {
            //add item to dictionary here, save their current money
            avaliableMoney -= currentPrice;
            Destroy(currentShopOption.gameObject);

            offBuyButton.gameObject.SetActive(true);
            ableToBuy =false;

            currentPrice = 0;

            Debug.Log(avaliableMoney+"");

        }

        //you cannot afford it
    }

    public void CloseShop()
    {
        this.gameObject.SetActive(false);

    }
}
