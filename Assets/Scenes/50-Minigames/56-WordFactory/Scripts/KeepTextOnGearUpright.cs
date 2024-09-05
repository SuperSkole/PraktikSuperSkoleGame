using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts
{
    public class KeepTextOnGearUpright : MonoBehaviour
    {
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            // Make the text face the camera directly
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;
            directionToCamera.y = 0; // Ignore Y component to keep the text upright

            // Calculate the rotation to face the camera
            Quaternion lookRotation = Quaternion.LookRotation(-directionToCamera);
            transform.rotation = lookRotation;

            // Adjust rotation to avoid the text being upside down or sideways
            transform.Rotate(0, 0, 0);
        }
    }
}