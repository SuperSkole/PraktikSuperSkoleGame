using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;

    private string currentTopItem;
    private string currentMidItem;

    public void Click(string itemName)
    {

        if (itemName.Contains("TOP"))
        {
            if(currentTopItem != null)
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
}
