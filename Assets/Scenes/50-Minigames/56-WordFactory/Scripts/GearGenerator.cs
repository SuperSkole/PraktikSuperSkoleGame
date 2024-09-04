using System.Collections.Generic;
using Scenes._05_Minigames._56_WordFactory.Scripts;
using Scenes._05_Minigames._56_WordFactory.Scripts.Managers;
using Scenes._05_Minigames.WordFactory.Scripts;
using TMPro;
using UnityEngine;

namespace Scenes.Minigames.WordFactory.Scripts
{
    public class GearGenerator : MonoBehaviour
    {
        [SerializeField] private GearButtonManager gearButtonManager;
        [SerializeField] private LetterHandler letterHandler;
        [SerializeField] private Transform centralPoint;  
        
        [SerializeField] private float cylinderScaleXZ = 4f;  
        [SerializeField] private float semiCircleRadius;  
        [SerializeField] private GameObject letterGearCylinderPrefab;
        [SerializeField] private GameObject letterGearToothPrefab;
        [SerializeField] private GameObject wordBlockPrefabForSingleGearMode;

        private GameObject singleGearConsonantWordBlock;
        private int numberOfGears;
        private int numberOfTeeth;
        
        private void Awake()
        {
            numberOfGears = WordFactoryGameManager.Instance.GetNumberOfGears();
            numberOfTeeth = numberOfGears >= 2
                ? WordFactoryGameManager.Instance.GetNumberOfTeeth()
                : 9;
            
            // Set the semi-circle radius to accommodate gear spacing correctly
            semiCircleRadius = (cylinderScaleXZ / 2) + 0.6f + (numberOfGears - 2);  // half diameter + teeth outward position
            GenerateGears();
        }

        private void GenerateGears()
        {
            PlaceGearsSemiCircular(numberOfGears, numberOfTeeth);

            // Remaining setup
            letterHandler.DistributeLetters();

            if (numberOfGears == 1)
            {
                // If using SingleGearStrategy, ensure consonants are displayed
                if (WordFactoryGameManager.Instance.GetGearStrategy() is SingleGearStrategy singleGearStrategy)
                {
                    DisplayConsonants(singleGearStrategy.GetConsonants());
                }
            }

            ColorTooth.RequestBlinkAllTeethRandomly(WordFactoryGameManager.Instance.GetGears().ToArray());
        }

        private void DisplayConsonants(List<char> consonants)
        {
            if (singleGearConsonantWordBlock == null)
            {
                Debug.LogError("SingleGearConsonantWordBlock is not instantiated.");
                return;
            }

            // Find the TextMeshProUGUI components within the instantiated wordBlockPrefabStatic
            TextMeshProUGUI[] textBlocks = singleGearConsonantWordBlock.GetComponentsInChildren<TextMeshProUGUI>();

            if (textBlocks.Length < consonants.Count)
            {
                Debug.LogError("Not enough text blocks to display all consonants.");
                return;
            }

            // Assign each consonant to the respective block
            for (int i = 0; i < consonants.Count && i < textBlocks.Length; i++)
            {
                textBlocks[i].text = consonants[i].ToString();
            }
        }
        
        public void ClearConsonantBlock()
        {
            if (singleGearConsonantWordBlock != null)
            {
                TextMeshProUGUI[] textBlocks = singleGearConsonantWordBlock.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (var textBlock in textBlocks)
                {
                    textBlock.text = ""; // Clear the text
                }
            }
            else
            {
                Debug.LogError("No consonant block found to reset.");
            }
        }


        private void PlaceGearsSemiCircular(int numberOfGears, int numberOfTeeth)
        {
            if (numberOfGears == 1)
            {
                // Position single gear at 3 o'clock by
                // moving right from the central point, Maintain the same vertical position and Constant z pos
                Vector3 gearPosition = new Vector3(
                    centralPoint.position.x + semiCircleRadius, 
                    centralPoint.position.y,                    
                    5);                                        

                // Instantiate and configure the single gear
                GameObject gear = InstantiateGear("Gear1",
                    gearPosition,
                    0,
                    numberOfTeeth);
                WordFactoryGameManager.Instance.AddGear(gear);
                gearButtonManager.CreateButtonsForGear(gear);
                
                // Instantiate UI element just to the left of the central point
                // Slightly left from the central point, Same vertical level as the central point
                // Slightly in front of the gear for visibility
                Vector3 uiPosition = new Vector3(
                    centralPoint.position.x - 1.6f,  
                    centralPoint.position.y,           
                    4);                                
                singleGearConsonantWordBlock = Instantiate(
                    wordBlockPrefabForSingleGearMode,
                    uiPosition,
                    Quaternion.identity,
                    transform);
                singleGearConsonantWordBlock.name = "ConsonantWordBlock";
                WordFactoryGameManager.Instance.SetWordBlock(
                    singleGearConsonantWordBlock);
            }
            else
            {
                float angleStep = 180f / Mathf.Max(1, numberOfGears - 1);  // Calculate angle between gears

                for (int i = 0; i < numberOfGears; i++)
                {
                    float angle = angleStep * i - 90;  // Start from -90 degrees to spread gears in a semi-circle
                    float angleInRadians = angle * Mathf.Deg2Rad;

                    // Calculate x and y positions for gear placement along the semi-circle
                    Vector3 gearPosition = new Vector3(
                        centralPoint.position.x + semiCircleRadius * Mathf.Sin(angleInRadians),  
                        centralPoint.position.y + semiCircleRadius * Mathf.Cos(angleInRadians), // y adjusts for semi-circle curvature vertically
                        5);  // z is constant, moved back

                    GameObject gear = InstantiateGear($"Gear{i + 1}", gearPosition, i, numberOfTeeth);
                    WordFactoryGameManager.Instance.AddGear(gear);
                    gearButtonManager.CreateButtonsForGear(gear);
                }
            }
        }


        private GameObject InstantiateGear(string name, Vector3 position, int gearIndex, int numberOfTeeth)
        {
            GameObject gearInstance = Instantiate(letterGearCylinderPrefab, position, Quaternion.Euler(90, 0, 0), transform);
            gearInstance.name = name;
            gearInstance.transform.localScale = new Vector3(cylinderScaleXZ, 0.1f, cylinderScaleXZ);
        
            GearRotationController rotationController = gearInstance.GetComponent<GearRotationController>();
            if (rotationController != null)
            {
                rotationController.gearIndex = gearIndex;
            }

            var teethContainer = new GameObject("TeethContainer");
            teethContainer.transform.parent = gearInstance.transform;
            teethContainer.transform.localScale = Vector3.one;
            teethContainer.transform.localPosition = new Vector3(0, -1f, 0);
            teethContainer.transform.localRotation = Quaternion.identity;

            GenerateTeeth(teethContainer.transform, 0.55f, numberOfTeeth);

            return gearInstance;
        }

        /// <summary>
        /// Generates teeth around the parent transform in a circle, starting from the 9 o'clock position.
        /// </summary>
        /// <param name="parent">The parent transform to attach the teeth.</param>
        /// <param name="radius">The radius at which to place the teeth.</param>
        /// <param name="numberOfTeeth">The total number of teeth to generate.</param>
        private void GenerateTeeth(Transform parent, float radius, int toothnumber)
        {
            for (var i = 0; i < toothnumber; i++)
            {
                float angle = i * Mathf.PI * 2 / toothnumber + Mathf.PI;
                Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                Quaternion rotation = Quaternion.LookRotation(position, Vector3.up);
                var tooth = Instantiate(letterGearToothPrefab, parent);
                tooth.transform.localPosition = position;
                tooth.transform.localRotation = rotation;
                tooth.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                tooth.name = "Tooth " + (i + 1);
            }
        }
    }
}
