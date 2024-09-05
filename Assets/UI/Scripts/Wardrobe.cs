using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Scripts
{
    public class Wardrop : MonoBehaviour
    {
        public SkeletonGraphic skeletonGraphic;

        //The item names
        private string currentTopItem;
        private string currentMidItem;

        //changing color
        private ColorChanging playerColorChanging;
        //ClothChanging
        private ClothChanging clothChanging;

        [SerializeField] GameObject WardrobePrefab;
        [SerializeField] Transform WardrobeParent;

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

            List<ClothInfo> theWardrobeOptions = ClothingManager.Instance.WardrobeContent(PlayerManager.Instance.PlayerData.BoughtClothes);
            if (theWardrobeOptions.Count != 0)
            {
                InitializeWardrobeOption(theWardrobeOptions);
            }

        }

        private void OnDisable()
        {
            var amountOfChild = WardrobeParent.childCount;

            for (int i = amountOfChild - 1; i >= 0; i--)
            {
                Destroy(WardrobeParent.GetChild(i).gameObject);
            }
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
            if (itemName.Contains("TOP"))
            {
                if (currentTopItem != null)
                {
                    skeletonGraphic.Skeleton.SetAttachment(currentTopItem, null);
                }
                skeletonGraphic.Skeleton.SetAttachment(itemName, itemName);
                currentTopItem = itemName;

                PlayerManager.Instance.PlayerData.ClothMid = currentTopItem;
            }

            if (itemName.Contains("MID"))
            {
                if (currentMidItem != null)
                {
                    skeletonGraphic.Skeleton.SetAttachment(currentMidItem, null);
                }
                skeletonGraphic.Skeleton.SetAttachment(itemName, itemName);
                currentMidItem = itemName;

                PlayerManager.Instance.PlayerData.ClothMid = currentMidItem;
            }

            foreach (var color in colors)
            {
                if (itemName.Contains(color, System.StringComparison.OrdinalIgnoreCase))
                {
                    playerColorChanging.ColorChange(itemName);
                }
            }
        }

        public void CloseShop()
        {
            this.gameObject.SetActive(false);
        }
    }

}