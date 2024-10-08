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

        HashSet<string> colors;

        private void Awake()
        {
            playerColorChanging = this.GetComponent<ColorChanging>();

            clothChanging = this.GetComponent<ClothChanging>();

            wardrobeOptionList = new List<WardrobeOption>();
        }


        private void OnEnable()
        {
            if (playerColorChanging.colors == null)
            {
                Debug.LogError("playerColorChanging.colors is null");
                return;
            }
            colors = new HashSet<string>(playerColorChanging.colors, StringComparer.OrdinalIgnoreCase);

            foreach(var color in colors)
            {

            Debug.Log(color);
            }

            if (PlayerManager.Instance?.PlayerData == null)
            {
                Debug.LogError("PlayerData is null");
                return;
            }

            if (ClothingManager.Instance == null)
            {
                Debug.LogError("ClothingManager.Instance is null");
                return;
            }

            if (PlayerManager.Instance.PlayerData.BoughtClothes == null)
            {
                Debug.LogError("PlayerData.BoughtClothes is null");
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
            }

            //set variables
            if (!string.IsNullOrEmpty(PlayerManager.Instance.PlayerData.ClothMid))
            {
                wearingMid = PlayerManager.Instance.PlayerData.ClothMid;
                Debug.Log("wearingMid "+wearingMid);
            }
            if (!string.IsNullOrEmpty(PlayerManager.Instance.PlayerData.ClothTop))
            {
                wearingTop = PlayerManager.Instance.PlayerData.ClothTop;
                Debug.Log("wearingTop "+wearingTop);
            }
            if (!string.IsNullOrEmpty(PlayerManager.Instance.PlayerData.MonsterColor))
            {
                wearingColor = PlayerManager.Instance.PlayerData.MonsterColor;
                Debug.Log("wearingcolor "+wearingColor);
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
                Debug.Log(itemName);
                if (itemName.Contains("HEAD"))
                {
                    if (!string.IsNullOrEmpty(wearingTop))
                    {
                        skeletonGraphic.Skeleton.SetAttachment(wearingTop, null);

                        wearingTop = string.Empty;
                        wardOptionTop = null;



                        PlayerManager.Instance.PlayerData.ClothTop = string.Empty;
                    }
                }
                if (itemName.Contains("MID"))
                {
                    if (!string.IsNullOrEmpty(wearingMid))
                    {
                        skeletonGraphic.Skeleton.SetAttachment(wearingMid, null);

                        wearingMid = string.Empty;
                        wardOptionMid = null;

                        PlayerManager.Instance.PlayerData.ClothMid = string.Empty;
                    }
                }
                if (colors.Contains(itemName))
                {
                    Debug.Log("We in");
                    playerColorChanging.ColorChange(null);

                        wearingColor = null;
                        wardOptionColor = null;

                        PlayerManager.Instance.PlayerData.MonsterColor = null;                  
                }

                wardrobeShopOption.chosen = false;
                wardrobeShopOption.UnSelect();
            }
            //IF IT'S NOT CHOSEN
            else
            {
                if (itemName.Contains("HEAD"))
                {
                    if (!string.IsNullOrEmpty(wearingTop))
                    {
                        skeletonGraphic.Skeleton.SetAttachment(wearingTop, null);    
                    }

                    if(wardOptionTop != null)
                    {
                        wardOptionTop.UnSelect();
                    }
                    wardOptionTop = wardrobeShopOption;

                    skeletonGraphic.Skeleton.SetAttachment(itemName, itemName);
                    wearingTop = itemName;
                    PlayerManager.Instance.PlayerData.ClothTop = itemName;
                }
                if (itemName.Contains("MID"))
                {
                    if (!string.IsNullOrEmpty(wearingMid))
                    {
                        skeletonGraphic.Skeleton.SetAttachment(wearingMid, null);
                    }

                    if (wardOptionMid != null)
                    {
                        wardOptionMid.UnSelect();
                    }
                    wardOptionMid = wardrobeShopOption;

                    skeletonGraphic.Skeleton.SetAttachment(itemName, itemName);
                    wearingMid = itemName;
                    PlayerManager.Instance.PlayerData.ClothMid = itemName;
                }

                if (colors.Contains(itemName))
                {                
                        playerColorChanging.ColorChange(itemName);

                        if (wardOptionColor != null)
                        {
                            wardOptionColor.UnSelect();
                        }
                        wardOptionColor = wardrobeShopOption;

                        wearingColor = itemName;

                        PlayerManager.Instance.PlayerData.MonsterColor = itemName;                
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