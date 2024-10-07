using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using UnityEngine;

namespace UI.ScreenLoader
{
    public class SceneLoaderMonster : MonoBehaviour
    {
        public SkeletonGraphic SkeletonGraphic;
    
        //changing color
        private ColorChanging playerColorChanging;
    
        //ClothChanging
        private ClothChanging clothChanging;

        private void Awake()
        {
            if(SkeletonGraphic != null && SkeletonGraphic.AnimationState != null)
            {
                SkeletonGraphic.AnimationState.SetAnimation(0, "Walk", true);
            }

            if (playerColorChanging == null && PlayerManager.Instance.PlayerData != null)
            {
                playerColorChanging = this.GetComponent<ColorChanging>();

                try
                {
                    playerColorChanging.SetSkeleton(SkeletonGraphic);
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
                    clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothMid, SkeletonGraphic);
                    clothChanging.ChangeClothes(PlayerManager.Instance.PlayerData.ClothTop, SkeletonGraphic);
                }
                catch
                {
                    print("Cant Find skeletonGraphic, But because i'm nice i wont error you");
                }
            }
        }
    }
}
