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
                    skeleton.SetAttachment(clothTop, clothTop);
                }

                if (clothMid != null)
                {
                    skeleton.SetAttachment(clothMid, clothMid);
                }

                //set new clothing

                if (clothName.Contains("HEAD"))
                {

                    if (clothTop != null)
                    {
                        skeleton.SetAttachment(clothTop, null);
                    }

                    clothTop = clothName;
                }

                if (clothName.Contains("MID"))
                {
                    if (clothMid != null)
                    {
                        skeleton.SetAttachment(clothMid, null);
                    }
                    clothMid = clothName;
                }

                skeleton.SetAttachment(clothName, clothName);
            }
        }

    }
}
