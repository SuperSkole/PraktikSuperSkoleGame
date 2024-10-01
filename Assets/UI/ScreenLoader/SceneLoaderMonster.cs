using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Scenes._10_PlayerScene.Scripts;

public class SceneLoaderMonster : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;
    //changing color
    private ColorChanging playerColorChanging;
    //ClothChanging
    private ClothChanging clothChanging;

    

    private void Awake()
    {
        if(skeletonGraphic != null && skeletonGraphic.AnimationState != null)
        {
            skeletonGraphic.AnimationState.SetAnimation(0, "Walk", true);
        }

        if (playerColorChanging == null)
        {
            playerColorChanging = this.GetComponent<ColorChanging>();

             playerColorChanging.SetSkeleton(skeletonGraphic);
             playerColorChanging.ColorChange(PlayerManager.Instance.PlayerData.MonsterColor);
        }
        if (clothChanging == null)
        {
            clothChanging = this.GetComponent<ClothChanging>();
             clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothMid, skeletonGraphic);
             clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothTop, skeletonGraphic);
        }



    }
}
