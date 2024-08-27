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

    [SerializeField] Image offEquipButton;
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
