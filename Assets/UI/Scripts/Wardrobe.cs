using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Scripts
{
    public class Wardrop : MonoBehaviour
    {
        public SkeletonGraphic skeletonGraphic;

        //changing color
        private ColorChanging playerColorChanging;
        //ClothChanging
        private ClothChanging clothChanging;

        [SerializeField] GameObject WardrobePrefab;
        [SerializeField] Transform WardrobeParent;

        private WardrobeOption currentOption;

        private string wearingMid = null;
        private string wearingTop = null;
        private string wearingColor = null;
        
        //colors
        List<string> colors = new List<string>();

        private void Awake()
        {
            playerColorChanging = this.GetComponent<ColorChanging>();

            clothChanging = this.GetComponent<ClothChanging>();

            colors.AddRange(playerColorChanging.colors);
        }


        private void OnEnable()
        {
            //change color
            playerColorChanging.SetSkeleton(skeletonGraphic);
            playerColorChanging.ColorChange(PlayerManager.Instance.PlayerData.MonsterColor);

            //chnage clothes
            clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothMid, skeletonGraphic);
            clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothTop, skeletonGraphic);

            List<ClothInfo> theWardrobeOptions = new();

            try
            {
                theWardrobeOptions = ClothingManager.Instance.WardrobeContent(PlayerManager.Instance.PlayerData.BoughtClothes);
                if (theWardrobeOptions.Count != 0)
                {
                    InitializeWardrobeOption(theWardrobeOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Error fetching wardrobe options: {ex.Message}");
            }

            //set variables
           if(PlayerManager.Instance.PlayerData.ClothMid != null && PlayerManager.Instance.PlayerData.ClothMid != string.Empty)
           {
                  wearingMid = PlayerManager.Instance.PlayerData.ClothMid;
           }
           if (PlayerManager.Instance.PlayerData.ClothTop != null && PlayerManager.Instance.PlayerData.ClothTop != string.Empty)
           {
                 wearingTop = PlayerManager.Instance.PlayerData.ClothTop;
           }
           if (PlayerManager.Instance.PlayerData.MonsterColor != null && PlayerManager.Instance.PlayerData.MonsterColor != string.Empty)
           {
                 wearingColor = PlayerManager.Instance.PlayerData.MonsterColor;
           }
        }

        private void OnDisable()
        {
            var amountOfChild = WardrobeParent.childCount;

            for (int i = amountOfChild - 1; i >= 0; i--)
            {
                Destroy(WardrobeParent.GetChild(i).gameObject);
            }

            PlayerManager.Instance.UpdatePlayerClothOnSceneChange(SceneManager.GetActiveScene());
            PlayerManager.Instance.UpdatePlayerColorOnSceneChange(SceneManager.GetActiveScene());
        }


        private void InitializeWardrobeOption(List<ClothInfo> availableoptions)
        {
            foreach (ClothInfo cloth in availableoptions)
            {
                //instantiate a new WardrobeOption
                GameObject newWardrobeObj = Instantiate(WardrobePrefab, WardrobeParent);

                //initialize the wardrobeOption with the cloth data
                WardrobeOption wardrobeOption = newWardrobeObj.GetComponent<WardrobeOption>();
                wardrobeOption.Initialize(cloth.Name, cloth.image, cloth.SpineName);
            }
        }

        public void Click(string itemName, WardrobeOption wardrobeShopOption)
        {

            if (currentOption != null)
            {
                currentOption.UnSelect();
            }

            currentOption = wardrobeShopOption;

            if (itemName.Contains("HEAD"))
            {
                if (wearingTop != null)
                {
                    skeletonGraphic.Skeleton.SetAttachment(wearingTop, null);
                }
                if (wearingMid != null)
                {
                    skeletonGraphic.Skeleton.SetAttachment(wearingMid, wearingMid);
                }
                if (wearingColor != null)
                {
                    playerColorChanging.ColorChange(wearingColor);
                }

                skeletonGraphic.Skeleton.SetAttachment(itemName, itemName);
                wearingTop = itemName;

                PlayerManager.Instance.PlayerData.ClothTop = itemName;
            }

            if (itemName.Contains("MID"))
            {
                if (wearingMid != null)
                {
                    skeletonGraphic.Skeleton.SetAttachment(wearingMid, null);
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
                wearingMid = itemName;

                PlayerManager.Instance.PlayerData.ClothMid = itemName;
            }

            foreach (var color in colors)
            {
                if (itemName.Contains(color, System.StringComparison.OrdinalIgnoreCase))
                {
                    playerColorChanging.ColorChange(itemName);

                    if (wearingMid != null)
                    {
                        skeletonGraphic.Skeleton.SetAttachment(wearingMid, wearingMid);
                    }
                    if (wearingTop != null)
                    {
                        skeletonGraphic.Skeleton.SetAttachment(wearingTop, wearingTop);
                    }

                    wearingColor = itemName;

                    PlayerManager.Instance.PlayerData.MonsterColor = itemName;
                }
            }
        }

        public void CloseShop()
        {
            currentOption = null;
            this.gameObject.SetActive(false);
            PlayerManager.Instance.UpdatePlayerClothOnSceneChange(SceneManager.GetActiveScene());
            PlayerManager.Instance.UpdatePlayerColorOnSceneChange(SceneManager.GetActiveScene());
        }

       
    }

}