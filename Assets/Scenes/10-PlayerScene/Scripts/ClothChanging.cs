using Spine.Unity;
using UnityEngine;

namespace Scenes._10_PlayerScene.Scripts
{
    public class ClothChanging : MonoBehaviour
    {
        private string clothMid = null;
        private string clothTop = null;

        public void ChangeClothes(string clothName, ISkeletonComponent givenSkeleton)
        {

            if (givenSkeleton != null && clothName != null && clothName != string.Empty)
            { 
                var skeleton = givenSkeleton.Skeleton;

                if (clothTop != null)
                {
                    skeleton.SetAttachment(clothTop, null);
                }

                if (clothMid != null)
                {
                    skeleton.SetAttachment(clothMid, null);
                }

                //set new clothing
                skeleton.SetAttachment(clothName, clothName);

                if (clothName.Contains("HEAD"))
                {
                    clothTop = clothName;
                }

                if (clothName.Contains("MID"))
                {
                    clothMid = clothName;
                }
            }
        }

    }
}
