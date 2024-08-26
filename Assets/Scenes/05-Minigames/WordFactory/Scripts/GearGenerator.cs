using Scenes._05_Minigames.WordFactory.Scripts.Managers;
using Scenes.Minigames.WordFactory.Scripts.Managers;
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

        private  int numberOfGears;
        private int numberOfTeeth;
        
        private void Awake()
        {
            numberOfGears = WordFactoryGameManager.Instance.GetNumberOfGears();
            numberOfTeeth = WordFactoryGameManager.Instance.GetNumberOfTeeth();
            
            // Set the semi-circle radius to accommodate gear spacing correctly
            semiCircleRadius = (cylinderScaleXZ / 2) + 0.6f + (numberOfGears - 2);  // half diameter + teeth outward position
            GenerateGears();
        }

        private void GenerateGears()
        {
            PlaceGearsSemiCircular(numberOfGears, numberOfTeeth);

            // Remaining setup
            letterHandler.DistributeLetters();
            
            ColorTooth.RequestBlinkAllTeethRandomly(WordFactoryGameManager.Instance.GetGears().ToArray());
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
                GameObject gear = InstantiateGear("Gear1", gearPosition, 0, numberOfTeeth);
                WordFactoryGameManager.Instance.AddGear(gear);
                gearButtonManager.CreateButtonsForGear(gear);
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
                        centralPoint.position.x + semiCircleRadius * Mathf.Cos(angleInRadians),  
                        centralPoint.position.y + semiCircleRadius * Mathf.Sin(angleInRadians), // Adjusted for clarity
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

        private void GenerateTeeth(Transform parent, float radius, int numberOfTeeth)
        {
            for (var i = 0; i < numberOfTeeth; i++)
            {
                float angle = i * Mathf.PI * 2 / numberOfTeeth;
                Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                Quaternion rotation = Quaternion.LookRotation(position, Vector3.up);
                var tooth = Instantiate(letterGearToothPrefab, parent);
                tooth.transform.localPosition = position;
                tooth.transform.localRotation = rotation;
                tooth.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                tooth.name = "Tooth " + (i + 1);
            }
        }
    }
}
