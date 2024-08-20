using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private Camera camera;
    [SerializeField] private GameObject cameraChild;
    [SerializeField] private PlacementSystem placementSystem;

    private float maxFov = 70f;
    private float minFov = 40f;
    private float currentFov;

    private float minRotation = 20;
    private float maxRoation = 70;
    private Vector2 turn;

    private float sensitivity = 2.5f;   // Sensitivity of the mouse movement
    private float smoothTime = 0.1f;  // Smoothing time for the rotation
    private float minYAngle = 20f;   // Minimum angle the camera can rotate down
    private float maxYAngle = 80f;    // Maximum angle the camera can rotate up

    private Vector2 rotation = Vector2.zero;  // Current rotation
    private Vector2 currentRotation;          // Smoothed rotation

    private Vector2 currentRotationVelocity;  // Used by SmoothDamp for smoothin
    [SerializeField] private float moveSpeed = 5.0f; // Adjust the speed as needed

    

    private void Start()
    {
        camera.fieldOfView = 60f;
        currentFov = 60f;
    }

    // Update is called once per frame
    void Update()
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
            //float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Adjust the rotation based on the mouse movement
            //rotation.x += mouseX * sensitivity;
            rotation.y -= mouseY * sensitivity;

            // Clamp the vertical rotation
            rotation.y = Mathf.Clamp(rotation.y, minYAngle, maxYAngle);

            // Smooth the rotation
           // currentRotation.x = Mathf.SmoothDamp(currentRotation.x, rotation.x, ref currentRotationVelocity.x, smoothTime);
            currentRotation.y = Mathf.SmoothDamp(currentRotation.y, rotation.y, ref currentRotationVelocity.y, smoothTime);

            // Apply the rotation to the camera
            cameraChild.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0f);
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
        camera.fieldOfView = currentFov;
        currentFov -= Input.mouseScrollDelta.y;
        if (currentFov > maxFov) { currentFov = maxFov; }
        else if (currentFov < minFov) { currentFov = minFov; }
    }
}
