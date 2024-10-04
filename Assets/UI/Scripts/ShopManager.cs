using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using System.Collections.Generic;
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
        private string wearingColor = null;

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

        [SerializeField] private RectTransform MonsterRectContent;
        [SerializeField] private GameObject MonsterGOContent;
        [SerializeField] private RectTransform HouseRectContent;
        [SerializeField] private GameObject HouseGOContent;
        [SerializeField] private ScrollRect scrollRect;

        private HouseItemsBuying houseItem;

        private Shopoption currentShopOption;

        //Check if they can buy bool
        private bool ableToBuy = false;

        private void Start()
        {
            if (colors.Count == 0)
            {
                colors.AddRange(playerColorChanging.colors);
            }
        }

        private void OnEnable()
        {

            MonsterGOContent.SetActive(true);
            HouseGOContent.SetActive(false);

            scrollRect.content = MonsterRectContent;
            avaliableMoney = 0;
            meter.SettingValueAfterScene(0);

            if (playerColorChanging == null)
            {
                playerColorChanging = this.GetComponent<ColorChanging>();
            }
            if (clothChanging == null)
            {
                clothChanging = this.GetComponent<ClothChanging>();
            }

            //Change grahic skeleton to what player is wearing
            playerColorChanging.SetSkeleton(skeletonGraphic);
            playerColorChanging.ColorChange(PlayerManager.Instance.PlayerData.MonsterColor);


            clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothMid, skeletonGraphic);
            clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothTop, skeletonGraphic);

            //Monster clothes they already wear
            if(PlayerManager.Instance.PlayerData.ClothMid != null && PlayerManager.Instance.PlayerData.ClothMid != string.Empty)
            {
                wearingMid = PlayerManager.Instance.PlayerData.ClothMid;
                skeletonGraphic.Skeleton.SetAttachment(wearingMid, wearingMid);
            }

            if (PlayerManager.Instance.PlayerData.ClothTop != null && PlayerManager.Instance.PlayerData.ClothTop != string.Empty)
            {
                wearingTop = PlayerManager.Instance.PlayerData.ClothTop;
                skeletonGraphic.Skeleton.SetAttachment(wearingTop, wearingTop);
            }

            if (PlayerManager.Instance.PlayerData.MonsterColor != null && PlayerManager.Instance.PlayerData.MonsterColor != string.Empty)
            {
                wearingColor = PlayerManager.Instance.PlayerData.MonsterColor;
            }



            //Moneyy
            if (PlayerManager.Instance == null)
            {
                Debug.Log("Didn't find playermanager");
            }
            else
            {
                avaliableMoney = PlayerManager.Instance.PlayerData.CurrentGoldAmount;

                meter.SettingValueAfterScene(avaliableMoney);
            }

            //Build shop

            List<ClothInfo> theShopOptions = ClothingManager.Instance.CipherList(PlayerManager.Instance.PlayerData.BoughtClothes);
            InitializeShopOptions(theShopOptions);
        }

        private void OnDisable()
        {
            MonsterGOContent.SetActive(true);
            HouseGOContent.SetActive(false);
            var amountOfChild = shopOptionsParent.childCount;

            for (int i = amountOfChild - 1; i >= 0; i--)
            {
                Destroy(shopOptionsParent.GetChild(i).gameObject);
            }

            if (currentItem != null)
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

            if (itemName.Contains("HEAD"))
            {
                Debug.Log("Head" + itemName);
                //hvis curren item ikke er tom, 
                if (currentItem != null && (itemName.Contains("MID") || itemName.Contains("HEAD")))
                {
                    Debug.Log("current " + currentItem);
                    skeletonGraphic.Skeleton.SetAttachment(currentItem, null);
                }
                if (wearingMid != null)
                {
                    Debug.Log("mid" + wearingMid);
                    skeletonGraphic.Skeleton.SetAttachment(wearingMid, wearingMid);
                }
                if (wearingColor != null)
                {
                    Debug.Log("color" + wearingColor);
                    playerColorChanging.ColorChange(wearingColor);
                }

                Debug.Log("set item");
                skeletonGraphic.Skeleton.SetAttachment(itemName, itemName);
                currentItem = itemName;
            }

            if (itemName.Contains("MID"))
            {
                //hvis curren item ikke er tom, 
                if (currentItem != null && (itemName.Contains("MID") || itemName.Contains("HEAD")))
                {
                    skeletonGraphic.Skeleton.SetAttachment(currentItem, null);
                }
                if (wearingTop != null)
                {
                    skeletonGraphic.Skeleton.SetAttachment(wearingTop, wearingTop);
                }
                if (wearingColor != null)
                {
                    playerColorChanging.ColorChange(wearingColor);
                }
                skeletonGraphic.Skeleton.SetAttachment(itemName, itemName);
                currentItem = itemName;
            }

            foreach (var color in colors)
            {

                if (itemName.Contains(color, System.StringComparison.OrdinalIgnoreCase))
                {
                    playerColorChanging.ColorChange(itemName);

                    currentItem = itemName;

                    if (wearingMid != null)
                    {
                        skeletonGraphic.Skeleton.SetAttachment(wearingMid, wearingMid);
                    }
                    if (wearingTop != null)
                    {
                        skeletonGraphic.Skeleton.SetAttachment(wearingTop, wearingTop);
                    }
                }
            }


            //Check if the player can afford this
            CanAfford(thisprice);


        }
        
        /// <summary>
        /// Methode for checking if the player has enough money to buy an item
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        private bool CanAfford(int price)
        {
            if (avaliableMoney >= price)
            {
                offBuyButton.gameObject.SetActive(false);
                ableToBuy = true;
            }
            if (avaliableMoney < price)
            {
                offBuyButton.gameObject.SetActive(true);
                ableToBuy = false;
            }
            return ableToBuy;
        }

        //Buy Button Function
        public void Buying()
        {
            if (MonsterGOContent.activeSelf)
            {
                if (currentItem != null)
                {
                    if (ableToBuy)
                    {

                        //Get player
                        PlayerManager.Instance.PlayerData.BoughtClothes.Add(currentShopOption.ID);

                        if (currentItem.Contains("HEAD"))
                        {
                            PlayerManager.Instance.PlayerData.ClothTop = currentItem;
                            wearingTop = currentItem;

                        }
                        if (currentItem.Contains("MID"))
                        {
                            PlayerManager.Instance.PlayerData.ClothMid = currentItem;
                            wearingMid = currentItem;

                        }
                        if (colors.Contains(currentItem.ToString()))
                        {
                            PlayerManager.Instance.PlayerData.MonsterColor = currentItem;
                            wearingColor = currentItem;
                        }

                        ModifyMoneyValue(currentPrice);




                        //remove bought option
                        Destroy(currentShopOption.gameObject);

                        offBuyButton.gameObject.SetActive(true);
                        ableToBuy = false;

                        currentPrice = 0;

                    }
                }
            }
            else if (HouseGOContent.activeSelf)
            {
                if (CanAfford(houseItem.Price))
                {
                    PlayerManager.Instance.PlayerData.ListOfFurniture.Add(houseItem.ID);
                    ModifyMoneyValue(houseItem.Price);
                }
            }
        }

        /// <summary>
        /// Takes in a price and then removes it from the pool of money the player has
        /// Used when buying an item 
        /// </summary>
        /// <param name="amount"></param>
        private void ModifyMoneyValue(int amount)
        {
            avaliableMoney -= amount;

            PlayerManager.Instance.PlayerData.CurrentGoldAmount = avaliableMoney;

            meter.ChangeValue(-amount);
        }

        /// <summary>
        /// Used by buttons sets the HouseItemsBuying so that when clicking the buy button we can buy a furniture item.
        /// </summary>
        /// <param name="item"></param>
        public void SettingFurnitureItem(HouseItemsBuying item)
        {
            houseItem = item;
            CanAfford(houseItem.Price);
        }


        /// <summary>
        /// Switches the content out from the scrollRect
        /// </summary>
        /// <param name="indexer"></param>
        public void SwitchContentView(int indexer)
        {
            switch (indexer)
            {
                case 1:
                    scrollRect.content = MonsterRectContent;
                    MonsterGOContent.SetActive(true);
                    HouseGOContent.SetActive(false);
                    break;
                case 2:
                    scrollRect.content = HouseRectContent;
                    MonsterGOContent.SetActive(false);
                    HouseGOContent.SetActive(true);
                    break;
            }
        }

        public void CloseShop()
        {
            currentShopOption = null;
            this.gameObject.SetActive(false);
            PlayerManager.Instance.UpdatePlayerClothOnSceneChange(SceneManager.GetActiveScene());
            PlayerManager.Instance.UpdatePlayerColorOnSceneChange(SceneManager.GetActiveScene());
        }

    }
}
