using CORE;
using Scenes._10_PlayerScene.Scripts;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Scripts
{
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

        private string wearingMid = null;
        private string wearingTop = null;

        List<string> colors = new List<string>();

        //!!Fill this one out with the amount the player has avaliable!!
        private int avaliableMoney;

        private int currentPrice;

        //GameObjects
        [SerializeField] Image offBuyButton;

        [SerializeField] GameObject shopOptionPrefab;
        [SerializeField] Transform shopOptionsParent;

        //Money bar
        [SerializeField] BarMeter meter;

        private Shopoption currentShopOption;

        //Check if they can buy bool
        private bool ableToBuy = false;

        private void Start()
        {
            if(colors.Count == 0)
            {
            colors.AddRange(playerColorChanging.colors);
            }
        }

        private void OnEnable()
        {
            if (playerColorChanging == null)
            {
                playerColorChanging = this.GetComponent<ColorChanging>();
            }
            if (clothChanging == null)
            {
                clothChanging = this.GetComponent<ClothChanging>();
            }

            playerColorChanging.SetSkeleton(skeletonGraphic);
            playerColorChanging.ColorChange(PlayerManager.Instance.PlayerData.MonsterColor);


            clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothMid, skeletonGraphic);
            clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothTop, skeletonGraphic);

            //Monster clothes they already wear
            wearingMid = PlayerManager.Instance.PlayerData.ClothMid;
            wearingTop = PlayerManager.Instance.PlayerData.ClothTop;

            if (wearingMid != null && wearingMid != string.Empty)
            {
                skeletonGraphic.Skeleton.SetAttachment(wearingMid, wearingMid);
            }

            if (wearingTop != null && wearingTop != string.Empty)
            {
                skeletonGraphic.Skeleton.SetAttachment(wearingTop, wearingTop);
            }

            //Moneyy
            if (PlayerManager.Instance == null)
            {
                Debug.Log("Didn't find playermanager");
            }
            else
            {
                avaliableMoney = PlayerManager.Instance.PlayerData.CurrentGoldAmount;

                Debug.Log(PlayerManager.Instance.PlayerData.CurrentGoldAmount);
            }

            //Build shop

            List<ClothInfo> theShopOptions = ClothingManager.Instance.CipherList(PlayerManager.Instance.PlayerData.BoughtClothes);
            InitializeShopOptions(theShopOptions);
        }

        private void OnDisable()
        {
            var amountOfChild = shopOptionsParent.childCount;

            for (int i = amountOfChild - 1; i >= 0; i--)
            {
                Destroy(shopOptionsParent.GetChild(i).gameObject);
            }

            if(currentItem != null)
            {
                skeletonGraphic.Skeleton.SetAttachment(currentItem, null);
            }

            PlayerManager.Instance.UpdatePlayerClothOnSceneChange(SceneManager.GetActiveScene());
            PlayerManager.Instance.UpdatePlayerColorOnSceneChange(SceneManager.GetActiveScene());
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
            if (currentItem != null)
            { 
            Debug.Log(ableToBuy);

            if (ableToBuy)
            {
                //Get player


                //add item to list here
                PlayerManager.Instance.PlayerData.BoughtClothes.Add(currentShopOption.ID);

                     if (currentItem.Contains("HEAD"))
                    {
                        PlayerManager.Instance.PlayerData.ClothTop = currentItem;
                       
                    }
                    if (currentItem.Contains("MID"))
                    {
                        PlayerManager.Instance.PlayerData.ClothMid = currentItem;
                        
                    }
                    if (colors.Contains(currentItem.ToString()))
                    {
                        PlayerManager.Instance.PlayerData.MonsterColor = currentItem;

                    }



                    //Take away money
                    avaliableMoney -= currentPrice;
                PlayerManager.Instance.PlayerData.CurrentGoldAmount = avaliableMoney;

                meter.ChangeValue(-currentPrice);




                //remove bought option
                Destroy(currentShopOption.gameObject);

                offBuyButton.gameObject.SetActive(true);
                ableToBuy = false;

                currentPrice = 0;

                currentItem = null;

            }
             }
            //you cannot afford it
        }

        public void CloseShop()
        {
            currentShopOption = null;
            this.gameObject.SetActive(false);
            PlayerManager.Instance.UpdatePlayerClothOnSceneChange(SceneManager.GetActiveScene());
            PlayerManager.Instance.UpdatePlayerColorOnSceneChange(SceneManager.GetActiveScene());
            Debug.Log("lukket");
        }

    }
}
