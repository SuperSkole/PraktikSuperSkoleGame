using Spine.Unity;
using UnityEngine;

namespace Scenes._03_StartScene.Scripts
{
    public class CharacterChoice : MonoBehaviour
    {
        [SerializeField] private UIStartSceneManager uiStartSceneManager;
        [SerializeField] private SkeletonGraphic previewImg;
        [SerializeField] private SkeletonDataAsset skeletonDataAsset;

        private int monsterId;

        public void OnClick(int monsterID)
        {
            this.monsterId = monsterID;
            previewImg.enabled = true;
            previewImg.skeletonDataAsset = skeletonDataAsset;
        }
    }
}
