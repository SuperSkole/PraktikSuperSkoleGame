using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    //Display model
    public SkeletonGraphic skeletonGraphic;
    //changing color
    private ColorChanging playerColorChanging;
    //ClothChanging
    private ClothChanging clothChanging;
    //The chosen item
    private string currentItem;


    List<string> colors = new List<string>();

    //!!Fill this one out with the amount the player has avaliable!!
    private int avaliableMoney;

    private int currentPrice;

    //GameObjects
    [SerializeField] Image offBuyButton;

    [SerializeField] GameObject shopOptionPrefab;
    [SerializeField] Transform shopOptionsParent;

    private Shopoption currentShopOption;

    //Check if they can buy bool
    private bool ableToBuy = false;

    private void Awake()
    {

        playerColorChanging = this.GetComponent<ColorChanging>();

        clothChanging = this.GetComponent<ClothChanging>();

        if (PlayerManager.Instance == null)
        {
            Debug.Log("Didn't find playermanager");
        }
        else
        {
            avaliableMoney = PlayerManager.Instance.PlayerData.CurrentGoldAmount;
        }

        colors.AddRange(playerColorChanging.colors);

    }
    private void OnEnable()
    {
        playerColorChanging.SetSkeleton(skeletonGraphic);
        playerColorChanging.ColorChange(PlayerManager.Instance.PlayerData.MonsterColor);


        clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothMid,skeletonGraphic);
        clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothTop, skeletonGraphic);
        //Build shop

        List<ClothInfo> theShopOptions = ClothingManager.Instance.CipherList(PlayerManager.Instance.PlayerData.BoughtClothes);
        InitializeShopOptions(theShopOptions);
    }

    private void OnDisable()
    {
        while (shopOptionsParent.childCount > 0)
        {
            Destroy(shopOptionsParent.GetChild(0).gameObject);
        }
    }

    //Create the shop options
    private void InitializeShopOptions(List<ClothInfo> availableClothes)
    {
        foreach (ClothInfo cloth in availableClothes)
        {
            // Instantiate a new ShopOption as a child of shopOptionsParent
            GameObject newShopOptionObj = Instantiate(shopOptionPrefab, shopOptionsParent);

            // Initialize the ShopOption with the cloth data
            Shopoption shopOption = newShopOptionObj.GetComponent<Shopoption>();
            shopOption.Initialize(cloth.Name, cloth.Price, cloth.image, cloth.SpineName, cloth.ID);
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

        if (itemName.Contains("HEAD") || itemName.Contains("MID"))
        {
            Debug.Log(itemName);

            //hvis curren item ikke er tom, 
            if (currentItem != null)
            {
                skeletonGraphic.Skeleton.SetAttachment(currentItem, null);
            }
            skeletonGraphic.Skeleton.SetAttachment(itemName, itemName);
            currentItem = itemName;
        }

        foreach (var color in colors)
        {
            if (itemName.Contains(color, System.StringComparison.OrdinalIgnoreCase))
            {
                playerColorChanging.ColorChange(itemName);
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
            //add item to list here
            PlayerManager.Instance.PlayerData.BoughtClothes.Add(currentShopOption.ID);

            //change clothing
            if(currentItem.Contains("HEAD"))
            {
                PlayerManager.Instance.PlayerData.ClothMid = currentItem;
            }
            if(currentItem.Contains("MID"))
            {
                PlayerManager.Instance.PlayerData.ClothTop = currentItem;
            }
            

            //takes away money
            avaliableMoney -= currentPrice;
            PlayerManager.Instance.PlayerData.CurrentGoldAmount = avaliableMoney;

            Destroy(currentShopOption.gameObject);

            offBuyButton.gameObject.SetActive(true);
            ableToBuy = false;

            currentPrice = 0;

        }

        //you cannot afford it
    }

    public void CloseShop()
    {
        this.gameObject.SetActive(false);

    }

}
