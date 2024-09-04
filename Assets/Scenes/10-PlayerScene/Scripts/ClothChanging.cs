using Spine.Unity;
using UnityEngine;

public class ClothChanging : MonoBehaviour
{

    public void ChangeClothes(string clothName, ISkeletonComponent givenSkeleton)
    {
        if (givenSkeleton == null && clothName == null)
        { 
            var skeleton = givenSkeleton.Skeleton;

            skeleton.SetAttachment(clothName, clothName);
        }
    }

}
