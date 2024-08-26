using Spine.Unity;
using UnityEngine;

namespace Scenes._01_StartScene.Scripts
{
    public class CharacterChoice : MonoBehaviour
    {
        [SerializeField] private UIStartSceneManager uiStartSceneManager;
        [SerializeField] private SkeletonGraphic previewImg;
        [SerializeField] private SkeletonDataAsset skeletonDataAsset;

        public int monsterId;

        public void OnClick(int monsterID)
        {
            this.monsterId = monsterID;
            previewImg.enabled = true;
            previewImg.skeletonDataAsset = skeletonDataAsset;
        }





    }
}
