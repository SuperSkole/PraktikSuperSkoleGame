using System;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

namespace Scenes.PlayerScene.Scripts
{
    public class PlayerColorChanger : MonoBehaviour
    {
        // The spine thingy that handles color change for the monster
        [SerializeField] private SkeletonAnimation skeletonAnimation;
    
        private Color selectedColor;
        
        private Dictionary<string, string> colorMap = new Dictionary<string, string>
        {
            { "orange", "ead25f" },
            { "blue", "19daf9" },
            { "red", "cf5b5d" },
            { "green", "6aa85c" }
        };
        
        public void ColorChange(string colorName)
        {
            if (colorMap.TryGetValue(colorName.ToLower(), out string hexValue))
            {
                Color selectedColor = HexToColor(hexValue);
                ApplyColorToSkeleton(selectedColor);
            }
            else
            {
                ApplyColorToSkeleton(Color.white);
            }
            

            // // register color
            // switch (colorName.ToLower())
            // {
            //     case "orange":
            //         selectedColor = HexToColor("ead25f");
            //         break;
            //     case "blue":
            //         selectedColor = HexToColor("19daf9");
            //         break;
            //     case "red":
            //         selectedColor = HexToColor("cf5b5d");
            //         break;
            //     case "green":
            //         selectedColor = HexToColor("6aa85c");
            //         break;
            //     default:
            //         selectedColor = Color.white;
            //         break;
            // }
            //
            // // Change the specific bones to chosen color
            // switch (skeletonAnimation.skeletonDataAsset.name)
            // {
            //     case "PraktikMonster_SkeletonData":
            //         string[] slotsToColor = 
            //         {
            //             "Monster L lowerleg color",
            //             "Monster R lowerleg color",
            //             "Monster L upperleg color",
            //             "Monster R upperleg color",
            //             "Monster head",
            //             "Monster body",
            //             "Monster R upperarm color",
            //             "Monster L upperarm color",
            //             "Monster R lowerarm color",
            //             "Monster L lowerarm color"
            //         };
            //
            //         foreach (string slotName in slotsToColor)
            //         {
            //             ChangeSlotColor(slotName, selectedColor);
            //         }
            //     
            //         break;
            //
            //     default:
            //         break;
            // }
        }
        
        private void ApplyColorToSkeleton(Color color)
        {
            string[] slotsToColor = GetSlotsToColor(skeletonAnimation.skeletonDataAsset.name);
            foreach (string slotName in slotsToColor)
            {
                ChangeSlotColor(slotName, color);
            }
        }
        
        private string[] GetSlotsToColor(string skeletonDataName)
        {
            switch (skeletonDataName)
            {
                case "PraktikMonster_SkeletonData":
                    return new string[]
                    {
                        "Monster L lowerleg color",
                        "Monster R lowerleg color",
                        "Monster L upperleg color",
                        "Monster R upperleg color",
                        "Monster head",
                        "Monster body",
                        "Monster R upperarm color",
                        "Monster L upperarm color",
                        "Monster R lowerarm color",
                        "Monster L lowerarm color"
                    };
                default:
                    return Array.Empty<string>();
            }
        }

        private void ChangeSlotColor(string slotName, Color color)
        {
            var slot = skeletonAnimation.Skeleton.FindSlot(slotName);
            if (slot != null)
            {
                slot.SetColor(color);
            }
        }

        private Color HexToColor(string hex)
        {
            // Unity's colorUtility class, try to convert string to hex
            return ColorUtility.TryParseHtmlString("#" + hex, out Color color)
                // return chosen color if match
                ? color
                // else return white
                : Color.white;
        }
    }
}
