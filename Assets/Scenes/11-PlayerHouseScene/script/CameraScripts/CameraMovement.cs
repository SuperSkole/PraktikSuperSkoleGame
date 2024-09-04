using Cinemachine;
using Scenes._11_PlayerHouseScene.script.HouseScripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scenes._11_PlayerHouseScene.script.CameraScripts
{
    public class CameraMovement : MonoBehaviour
    {

        [SerializeField] private PlacementSystem placementSystem;
        private float smoothTime = 0.1f;  // Smoothing time for the rotation
   

        private Vector2 rotation = Vector2.zero;  // Current rotation
        private Vector2 currentRotation;          // Smoothed rotation

        private Vector2 currentRotationVelocity;  // Used by SmoothDamp for smoothin
        [SerializeField] private float moveSpeed = 5.0f; // Adjust the speed as needed
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        // Update is called once per frame
        private void Update()
        {
            CameraZoom();
            Moveing();
            RotatingCamera();
        }

        private void RotatingCamera()
        {
            // Only rotate the camera if the right mouse button is held down
            if (Input.GetMouseButton(0) 
                && !placementSystem.GridVisualization.activeSelf
                && !EventSystem.current.IsPointerOverGameObject())
            {
                // Capture mouse input
                float mouseX = Input.GetAxis("Mouse X");
                // float mouseY = Input.GetAxis("Mouse Y");

                // Adjust the rotation based on the mouse movement
                rotation.x += mouseX;
                //   rotation.y -= mouseY;

                //// Clamp the vertical rotation
                //rotation.y = Mathf.Clamp(rotation.y, 0, 180);

                // Smooth the rotation
                currentRotation.x = Mathf.SmoothDamp(currentRotation.x, rotation.x, ref currentRotationVelocity.x, smoothTime);
                //  currentRotation.y = Mathf.SmoothDamp(currentRotation.y, rotation.y, ref currentRotationVelocity.y, smoothTime);

                // Apply the rotation to the camera
                gameObject.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0f);
            }
        }
    

        private void Moveing()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            //float tmpVal = horizontalInput + verticalInput;
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            // Move the player
            transform.Translate(movement * moveSpeed * Time.deltaTime);

        }

        private void CameraZoom()
        {

        }
    }
}
