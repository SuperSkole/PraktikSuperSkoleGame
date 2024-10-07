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
                skeleton.SetAttachment(clothName, clothName);

                if (clothName.Contains("HEAD"))
                {
                    Debug.Log("Head " + clothName);
                    clothTop = clothName;
                }

                if (clothName.Contains("MID"))
                {
                    Debug.Log("Mid " + clothName);
                    clothMid = clothName;
                }
            }
        }

    }
}
