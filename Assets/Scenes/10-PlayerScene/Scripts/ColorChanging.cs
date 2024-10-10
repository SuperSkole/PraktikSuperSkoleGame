using System.Collections.Generic;
using CORE;
using Spine.Unity;
using UnityEngine;

namespace Scenes._10_PlayerScene.Scripts
{
    public class ColorChanging : MonoBehaviour
    {
        public SkeletonGraphic graphic;

        private ISkeletonComponent chosenSkeletonComponent;

        private Dictionary<string, string> colorMap = new Dictionary<string, string>
        {
            { "orange", "ead25f" },
            { "blue", "19daf9" },
            { "red", "cf5b5d" },
            { "green", "6aa85c" },
            {"white", "FFFFFF" },
            {"pink","f888eb" }
        };

        public List<string> colors = new List<string>();

        private void Awake()
        {
            if(graphic != null)
            {
                chosenSkeletonComponent = graphic;

                if (graphic == null)
                {
                    Debug.Log("No graphic");
                }
            }

            colors.AddRange(colorMap.Keys);
        
        }

        public void SetSkeleton(ISkeletonComponent givenSkeleton)
        {
            chosenSkeletonComponent = givenSkeleton;
        }
            
        public void ColorChange(string colorName)
        {
            var skeleton = chosenSkeletonComponent.Skeleton;

            Color selectedColor;
            if (colorName == null)
            {
                colorName = "White";
                Debug.Log("WHite");
            }

            if (colorMap.TryGetValue(colorName.ToLower(), out string hexValue))
            {
                selectedColor = HexToColor(hexValue);
            }
            else
            {
                selectedColor = (Color.white);
            }

            if (chosenSkeletonComponent is SkeletonRenderer skeletonRenderer)
            {
                ApplyColorToSlots(skeletonRenderer, selectedColor, skeletonRenderer.skeletonDataAsset.name);
            }

            else if (chosenSkeletonComponent is SkeletonGraphic skeletonGraphic)
            {
                ApplyColorToSlots(skeletonGraphic, selectedColor, skeletonGraphic.skeletonDataAsset.name);
            }

            if (GameManager.Instance.CurrentMonsterColor != "" && colorName != GameManager.Instance.CurrentMonsterColor)
            {
                GameManager.Instance.CurrentMonsterColor = colorName;
            }
            else if(string.IsNullOrEmpty(GameManager.Instance.CurrentMonsterColor) )
            {
                GameManager.Instance.CurrentMonsterColor = "White";
            }
        }


        private void ApplyColorToSlots(ISkeletonComponent skeletonComponent, Color color, string skeletonDataAssetName)
        {
            string[] slotsToColor = null;

            switch (skeletonDataAssetName)
            {
                case "PraktikMonster_SkeletonData":
                    slotsToColor = new string[]
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
                    break;

                default:
                    Debug.LogWarning($"No matching skeleton found");
                    return;
            }

            if (slotsToColor != null)
            {
                foreach (string slotName in slotsToColor)
                {
                    ChangeSlotColor(skeletonComponent, slotName, color);
                }
            }
        }


        private void ChangeSlotColor(ISkeletonComponent skeletonComponent, string slotName, Color color)
        {
            var slot = skeletonComponent.Skeleton.FindSlot(slotName);
            if (slot != null)
            {
                slot.SetColor(color);
            }
            else
            {
                Debug.LogWarning($"Slot {slotName} not found in skeleton.");
            }
        }


        private Color HexToColor(string hex)
        {
            //Unity's colorUtility klasse, der fors�ger at konverter en string i HTML style hex format
            if (ColorUtility.TryParseHtmlString("#" + hex, out Color color))
            {
                //hvis farvekoden matcher
                return color;
            }
            else
            {
                //hvis farvekoden ikke matcher
                return Color.white;
            }
        }
    }
}
