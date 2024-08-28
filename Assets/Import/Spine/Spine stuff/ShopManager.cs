using Scenes.PlayerScene.Scripts;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    //Display model
    public SkeletonGraphic skeletonGraphic;
    //changing color
    private ColorChanging colorChanging;
    //The chosen item
    private string currentItem;
    

    List<string> colors = new List<string> { "orange", "blue", "red", "green", "white" };

    //!!Fill this one out with the amount the player has avaliable!!
    private int avaliableMoney ;

    private int currentPrice;

    //GameObjects
    [SerializeField] Image offBuyButton; 
    private Shopoption currentShopOption;

    //Check if they can buy bool
    private bool ableToBuy = false;

    private void Awake()
    {
        colorChanging = this.GetComponent<ColorChanging>();

        if(PlayerManager.Instance == null )
        {
            Debug.Log("Didn't mind playermanager");
        }
        else
        {
            avaliableMoney = PlayerManager.Instance.PlayerData.CurrentGoldAmount;
        }
    }


    //Shop Option function
    public void Click(string itemName, int thisprice, Shopoption shopOption)
    {

        //turns off the outline on the previous item
        if (currentShopOption != null)
        {
            currentShopOption.UnSelect();
        }

        //set in the new option
        currentPrice = thisprice;
        currentShopOption = shopOption;

        if (itemName.Contains("HEAD")|| itemName.Contains("MID"))
        {
            //hvis curren item ikke er tom, 
            if(currentItem != null)
            { 
                skeletonGraphic.Skeleton.SetAttachment(currentItem, null);
            }
            skeletonGraphic.Skeleton.SetAttachment(itemName, itemName);
            currentItem = itemName;
        }

        foreach(var color in colors)
        {
            if(itemName.Contains(color, System.StringComparison.OrdinalIgnoreCase))
            {
                    colorChanging.ColorChange(itemName);
            }
        }
     

        //Check if the player can afford this

        if (avaliableMoney >= thisprice)
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
            PlayerManager.Instance.PlayerData.CurrentGoldAmount = avaliableMoney; 

            Destroy(currentShopOption.gameObject);

            offBuyButton.gameObject.SetActive(true);
            ableToBuy =false;

            currentPrice = 0; 

        }

        //you cannot afford it
    }

    public void CloseShop()
    {
        this.gameObject.SetActive(false);

    }

}
