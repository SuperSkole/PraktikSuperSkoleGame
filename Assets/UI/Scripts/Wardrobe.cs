using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] Image offEquipButton;

    [SerializeField] GameObject WardrobePrefab;
    [SerializeField] Transform WardrobeParent;

    private void Awake()
    {
        playerColorChanging = this.GetComponent<ColorChanging>();

        clothChanging = this.GetComponent<ClothChanging>();
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

    }

    private void OnDisable()
    {
        var amountOfChild = WardrobeParent.childCount;

        for (int i = 0; i < amountOfChild; i++)
        {
            Destroy(WardrobeParent.GetChild(i));
        }
    }


    public void Click(string itemName, int thisprice, Shopoption shopOption)
    {
        if (itemName.Contains("TOP"))
        {
            if (currentTopItem != null)
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
    }

    public void CloseShop()
    {
        this.gameObject.SetActive(false);

    }
}
