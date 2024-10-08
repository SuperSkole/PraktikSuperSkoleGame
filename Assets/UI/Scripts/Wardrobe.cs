using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        private WardrobeOption wardOptionTop;
        private WardrobeOption wardOptionMid;
        private WardrobeOption wardOptionColor;

        private List<WardrobeOption> wardrobeOptionList;
        
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
            if (PlayerManager.Instance?.PlayerData == null)
            {
                Debug.LogError("PlayerData is null");
                return;
            }

            skeletonGraphic.Skeleton.SetSlotsToSetupPose();

            //change color
            playerColorChanging.SetSkeleton(skeletonGraphic);
            playerColorChanging.ColorChange(PlayerManager.Instance.PlayerData.MonsterColor);

            //change clothes
            clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothMid, skeletonGraphic);
            clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothTop, skeletonGraphic);

            List<ClothInfo> theWardrobeOptions = ClothingManager.Instance.WardrobeContent(PlayerManager.Instance.PlayerData.BoughtClothes);

            if (theWardrobeOptions.Count != 0)
            {
                    InitializeWardrobeOption(theWardrobeOptions);
                    Debug.Log(theWardrobeOptions.Count);
            }



            //set variables
            if (!string.IsNullOrEmpty(PlayerManager.Instance.PlayerData.ClothMid))
            {
                wearingMid = PlayerManager.Instance.PlayerData.ClothMid;
            }
            if (!string.IsNullOrEmpty(PlayerManager.Instance.PlayerData.ClothTop))
            {
                wearingTop = PlayerManager.Instance.PlayerData.ClothTop;
            }
            if (!string.IsNullOrEmpty(PlayerManager.Instance.PlayerData.MonsterColor))
            {
                wearingColor = PlayerManager.Instance.PlayerData.MonsterColor;
            }

            //Set WardrobeOptions
            if (wardrobeOptionList != null)
            {
                foreach (var item in wardrobeOptionList)
                {
                    if (item.SpineName == wearingTop)
                    {
                        wardOptionTop = item;
                        item.chosen = true;
                        item.LightUp();
                    }
                    if (item.SpineName == wearingMid)
                    {
                        wardOptionMid = item;
                        item.chosen = true;
                        item.LightUp();
                    }
                    if (item.SpineName == wearingColor)
                    {
                        wardOptionColor = item;
                        item.chosen = true;
                        item.LightUp();
                    }
                }
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
                wardrobeOptionList.Add(wardrobeOption);
            }
        }

        public void Click(string itemName, WardrobeOption wardrobeShopOption)
        {
            //turn off the last one
            if (currentOption != null && currentOption.chosen == false)
            {
                currentOption.UnSelect();
            }

            //IF IT'S ALREADY CHOSEN
            if (wardrobeShopOption.chosen)
            {
                if (itemName.Contains("HEAD"))
                {
                    if (wearingTop != string.Empty)
                    {
                        skeletonGraphic.Skeleton.SetAttachment(wearingTop, null);
                        wearingTop = string.Empty;
                        PlayerManager.Instance.PlayerData.ClothTop = string.Empty;
                    }
                }
                if (itemName.Contains("MID"))
                {
                    if (wearingMid != string.Empty)
                    {
                        skeletonGraphic.Skeleton.SetAttachment(wearingMid, null);
                        wearingMid = string.Empty;
                        PlayerManager.Instance.PlayerData.ClothMid = string.Empty;
                    }
                }
                foreach (var color in colors)
                {
                    if (itemName.Contains(color, System.StringComparison.OrdinalIgnoreCase))
                    {
                        playerColorChanging.ColorChange("white");

                        wearingColor = "white";

                        PlayerManager.Instance.PlayerData.MonsterColor = "white";
                    }
                }

                wardrobeShopOption.chosen = false;
                wardrobeShopOption.UnSelect();

            }

            //IF NOT CHOSEN
            if (!wardrobeShopOption.chosen)
            {

                if (itemName.Contains("HEAD"))
                {
                    if (wearingTop != string.Empty)
                    {
                        skeletonGraphic.Skeleton.SetAttachment(wearingTop, null);
                    }
                    skeletonGraphic.Skeleton.SetAttachment(itemName, itemName);
                    wearingTop = itemName;
                    PlayerManager.Instance.PlayerData.ClothTop = itemName;
                }
                if (itemName.Contains("MID"))
                {
                    if (wearingMid != string.Empty)
                    {
                        skeletonGraphic.Skeleton.SetAttachment(wearingMid, null);
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

                        wearingColor = itemName;

                        PlayerManager.Instance.PlayerData.MonsterColor = itemName;
                    }
                }

                wardrobeShopOption.chosen = true;
                wardrobeShopOption.LightUp();
            }

            currentOption = wardrobeShopOption;

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