using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.StartScene.Scripts
{
    public class CharacterChoice : MonoBehaviour
    {
        [SerializeField] private UIStartSceneManager uiStartSceneManager;
        public int monsterId;
        [SerializeField] private SkeletonGraphic previewImg;

        [SerializeField] private SkeletonDataAsset switchedskel;



        public void OnClick(int monsterID)
        {
            this.monsterId = monsterID;
            previewImg.enabled = true;
            previewImg.skeletonDataAsset = switchedskel;
        }





    }
}
