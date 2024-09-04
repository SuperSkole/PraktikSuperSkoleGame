using Spine.Unity;
using UnityEngine;

namespace UI.Scripts
{
    public class ChanginSkeleton : MonoBehaviour
    {
        [SerializeField] SkeletonDataAsset Monster1;
        [SerializeField] SkeletonDataAsset Monster2;
        [SerializeField] SkeletonDataAsset Monster3;
        [SerializeField] SkeletonDataAsset Monster4;

        public void ChangingSkeletonData(int monsterID, ISkeletonComponent skeletonComponent)
        {
            SkeletonDataAsset selectedSkeletonDataAsset = null;

            //what monster is asked for
            switch (monsterID)
            {
                case 1:
                    selectedSkeletonDataAsset = Monster1;
                    break;
                case 2:
                    selectedSkeletonDataAsset = Monster2;
                    break;
                case 3:
                    selectedSkeletonDataAsset = Monster3;
                    break;
                case 4:
                    selectedSkeletonDataAsset = Monster4;
                    break;
                default:
                    selectedSkeletonDataAsset = Monster1;
                    return;
            }

            //what type is the ISkeletonComponent
            if (skeletonComponent is SkeletonGraphic skeletonGraphic)
            {
                skeletonGraphic.skeletonDataAsset = selectedSkeletonDataAsset;

            }
            else if (skeletonComponent is SkeletonAnimation skeletonAnimation)
            {
                skeletonAnimation.skeletonDataAsset = selectedSkeletonDataAsset;

            }

        }
   
    }
}
