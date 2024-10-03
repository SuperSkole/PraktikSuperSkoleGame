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

        if (playerColorChanging == null && PlayerManager.Instance.PlayerData != null)
        {
            playerColorChanging = this.GetComponent<ColorChanging>();

            try
            {
                playerColorChanging.SetSkeleton(skeletonGraphic);
                playerColorChanging.ColorChange(PlayerManager.Instance.PlayerData.MonsterColor);
            }
            catch
            {
                print("Cant Find MonsterColor, But because i'm nice i wont error you");
            }
        }
        if (clothChanging == null && PlayerManager.Instance.PlayerData != null)
        {
            clothChanging = this.GetComponent<ClothChanging>();

            try
            {
                clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothMid, skeletonGraphic);
                clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothTop, skeletonGraphic);
            }
            catch
            {

                print("Cant Find skeletonGraphic, But because i'm nice i wont error you");
            }
        }



    }
}
