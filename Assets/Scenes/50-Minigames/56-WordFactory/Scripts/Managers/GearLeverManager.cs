using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.Managers
{
    public class GearLeverManager : MonoBehaviour
    {
        [SerializeField] private GameObject gearLeverPrefab; // Prefab for the lever
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
            CreateLeversForGear(gear);
        }

        public void CreateLeversForGear(GameObject gear)
        {
            // Instantiate the lever group as a child of the gear
            GameObject leverGroup = Instantiate(gearLeverPrefab, gear.transform);

            // Find the levers in the instantiated prefab
            LeverInteraction[] levers = leverGroup.GetComponentsInChildren<LeverInteraction>();

            // Find levers by their designated direction
            LeverInteraction clockwiseLever = null;
            LeverInteraction counterClockwiseLever = null;

            foreach (LeverInteraction lever in levers)
            {
                if (lever.ChosenRotationDirection == LeverInteraction.RotationDirection.Clockwise)
                {
                    clockwiseLever = lever;
                }
                else if (lever.ChosenRotationDirection == LeverInteraction.RotationDirection.CounterClockwise)
                {
                    counterClockwiseLever = lever;
                }
            }

            // Ensure both levers are found
            if (clockwiseLever == null || counterClockwiseLever == null)
            {
                Debug.LogError("One or more levers not found in GearLeverPrefab.");
                return;
            }

            // Assign levers to the gear's rotation controller
            GearRotationController rotationController = gear.GetComponent<GearRotationController>();

            clockwiseLever.OnLeverPulled += rotationController.RotateClockwise;
            counterClockwiseLever.OnLeverPulled += rotationController.RotateCounterClockwise;

            // Position the levers relative to the gear
            PositionLevers(leverGroup);
        }

        private void PositionLevers(GameObject leverGroup)
        {
            // Position the levers slightly below the gear
            leverGroup.transform.localPosition = new Vector3(0, -1, 0);

            // Adjust the rotation of the lever group if necessary
            leverGroup.transform.localRotation = Quaternion.identity;

            // Ensure lever group is not parented to the gear to avoid rotating with it
            leverGroup.transform.SetParent(gearLeverParent);
        }
    }
}
