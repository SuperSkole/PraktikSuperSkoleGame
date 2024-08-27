using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    //Display model
    public SkeletonGraphic skeletonGraphic;

    //The chosen item
    private string currentItem;

    List<string> colors = new List<string> { "orange", "blue", "red", "green", "white" };

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
        if (currentShopOption != null)
        {
            currentShopOption.UnSelect();
        }

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
            Color selectedColor;

            //register farven
            switch (itemName.ToLower())
            {
                case "orange":
                    selectedColor = HexToColor("ead25f");
                    break;
                case "blue":
                    selectedColor = HexToColor("19daf9");
                    break;
                case "red":
                    selectedColor = HexToColor("cf5b5d");
                    break;
                case "green":
                    selectedColor = HexToColor("6aa85c");
                    break;
                default:
                    selectedColor = Color.white;
                    break;
            }
            switch (skeletonGraphic.skeletonDataAsset.name)
            {
                case "PraktikMonster_SkeletonData":
                    string[] slotsToColor =
                    {
                    "Monster L lowerleg color",
                    "Monster R lowerleg color",
                    "Monster L upperleg color",
                    "Monster R upperleg color",
                    "Monster head",
                    "Monster body",
                    "Monster R upperarm color",
                    "Monster L upperarm color",
                    "Monster R lowerarm color",
                    "Monster L lowerarm color"
                };

                    foreach (string slotName in slotsToColor)
                    {
                        ChangeSlotColor(slotName, selectedColor);
                    }

                    break;

                default:
                    break;
            }
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

    private Color HexToColor(string hex)
    {
        //Unity's colorUtility klasse, der forsøger at konverter en string i HTML style hex format
        if (ColorUtility.TryParseHtmlString("#" + hex, out Color color))
        {
            //hvis farvekoden matcher
            return color;
        }
        else
        {
            //hvis farvekoden ikke matcher
            return Color.white;
        }
    }
    private void ChangeSlotColor(string slotName, Color color)
    {
        var slot = skeletonGraphic.Skeleton.FindSlot(slotName);
        if (slot != null)
        {
            slot.SetColor(color);
        }
    }
}
