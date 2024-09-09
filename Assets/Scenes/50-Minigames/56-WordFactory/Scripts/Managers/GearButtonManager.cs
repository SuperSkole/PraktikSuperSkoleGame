using UnityEngine;
using UnityEngine.UI;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.Managers
{
    public class GearButtonManager : MonoBehaviour
    {
        [SerializeField] private GameObject gearLeverPrefab; 
        [SerializeField] private Transform gearLeverParent;

        private void Start()
        {
            WordFactoryGameManager.Instance.OnGearAdded += HandleGearAdded;
        }

        private void OnDestroy()
        {
            WordFactoryGameManager.Instance.OnGearAdded -= HandleGearAdded;
        }

        private void HandleGearAdded(GameObject gear)
        {
            CreateButtonsForGear(gear);
        }

        public void CreateButtonsForGear(GameObject gear)
        {
            // Create buttons as children of the gear
            GameObject buttonGroup = Instantiate(gearLeverPrefab, gear.transform);

            // Find the buttons in the instantiated prefab
            Button[] buttons = buttonGroup.GetComponentsInChildren<Button>();

            // Find buttons by name
            Button rotateClockwiseButton = null;
            Button rotateCounterClockwiseButton = null;
            Button speakButton = null;

            foreach (Button button in buttons)
            {
                if (button.name == "RotateClockwise")
                {
                    rotateClockwiseButton = button;
                }
                else if (button.name == "RotateCounterClockwise")
                {
                    rotateCounterClockwiseButton = button;
                }
                else if (button.name == "Speak")
                {
                    speakButton = button;
                }
            }

            // Ensure all buttons are found
            if (rotateClockwiseButton == null || rotateCounterClockwiseButton == null || speakButton == null)
            {
                Debug.LogError("One or more buttons not found in GearButtonPrefab.");
                return;
            }

            // Assign buttons to the gear's rotation controller
            GearRotationController rotationController = gear.GetComponent<GearRotationController>();

            rotateClockwiseButton.onClick.AddListener(rotationController.RotateClockwise);
            rotateCounterClockwiseButton.onClick.AddListener(rotationController.RotateCounterClockwise);
            speakButton.onClick.AddListener(() => Debug.Log("Speak button clicked")); // Placeholder for speak functionality

            // Position the buttons relative to the gear
            PositionButtons(buttonGroup);
        }

        private void PositionButtons(GameObject buttonGroup)
        {
            // Position the buttons slightly below the gear
            buttonGroup.transform.localPosition = new Vector3(0, 1, 0.9f);
            // Adjust the rotation of the button group
            buttonGroup.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            // Ensure button group is not parented to gear to avoid rotation
            buttonGroup.transform.SetParent(gearLeverParent); 
        }
    }
}
