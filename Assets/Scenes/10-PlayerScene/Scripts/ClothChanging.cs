using Spine.Unity;
using UnityEngine;

namespace Scenes._10_PlayerScene.Scripts
{
    public class ClothChanging : MonoBehaviour
    {

        public void ChangeClothes(string clothName, ISkeletonComponent givenSkeleton)
        {
            if (givenSkeleton != null && clothName != null && clothName != string.Empty)
            { 
                var skeleton = givenSkeleton.Skeleton;

                skeleton.SetAttachment(clothName, clothName);
            }
        }

    }
}
