using UnityEngine;

namespace Scenes.Minigames.MiniRacingGame
{
    public class CarController : MonoBehaviour
    {

        //public GameObject racingGameManager;
        int sceneID = 0;

        public bool carActive; //the car state
        public bool CarActive
        {
            get { return carActive; }
            set { carActive = value; }
        }

        // Constants for input axes names, used for reading player input.
        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";

        private float horizontalInput;
        private float verticalInput;

        private float currentSteerAngle;
        private float currentBreakForce;

        public float motorForce; // Power of the car's engine.
        public float maxSteeringAngle; // Maximum angle for steering.
        public float maxSpeed; // Maximum speed for the car.
        public float reverseMaxSpeed;

        public float brakingForce = 100000f;
        public float carSpeed;

        // Wheel colliders for simulating wheel physics.
        public WheelCollider wheelColliderFrontL;
        public WheelCollider wheelColliderFrontR;
        public WheelCollider wheelColliderRearR;
        public WheelCollider wheelColliderRearL;

        // Transforms for the visual representation of the wheels.
        public Transform wheelTransformFrontL;
        public Transform wheelTransformFrontR;
        public Transform wheelTransformRearR;
        public Transform wheelTransformRearL;

        //Headlight ref for toggle viseblity
        public GameObject leftHeadlight;
        public GameObject rightHeadlight;

        //Steering correction when parking
        public float steeringCorrectionRate = 2.0f;


        /// <summary>
        /// Check if this is in the racing scene before triggering. 
        /// TODO: Figure out a better way than scene ID.
        /// </summary>
        private void Start()
        {
            sceneID = SceneManagerScript.Instance.SceneID;
            if (sceneID == 13)
            {
                carActive = true; // Start with the car being off.
                leftHeadlight.SetActive(carActive == true);
                rightHeadlight.SetActive(carActive == true);
            }
            if (sceneID == 0)
            {
                carActive = false; // Start with the car being off.
                leftHeadlight.SetActive(carActive == false);
                rightHeadlight.SetActive(carActive == false);
            }
        }

        /// <summary>
        /// Checks if the player clicks E to activate headlights.
        /// </summary>
        private void Update()
        {
        
            // Toggle CarActive state when 'E' key is pressed.
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Toggle the headlights based on the CarActive state.
                leftHeadlight.SetActive(carActive);
                rightHeadlight.SetActive(carActive);
            }
        }

        /// <summary>
        /// Handles the car input.
        /// </summary>
        private void FixedUpdate()
        {
            HandleSteering();  // Manages the car's steering based on input.
            HandleMotor(); // Handle motor logic irrespective of CarActive state.
            UpdateWheels();    // Updates the visual representation of the wheels.

            if (carActive)
            {
                GetInput(); // Reads the player's input.

            }
        }

        /// <summary>
        /// Reads player's input from keyboard
        /// </summary>
        private void GetInput()
        {
            horizontalInput = Input.GetAxis(HORIZONTAL);
            verticalInput = Input.GetAxis(VERTICAL);

            // Check if the car is currently moving forward or is stopped
            bool movingForward = IsCarMovingForward();
        

            if (verticalInput < 0 && movingForward == true)
            {
                // Player is attempting to reverse while the car is moving forward
                ApplyBrakingToStop();
            }
            else if (verticalInput > 0)
            {
            
                // Player is moving forward
                ResetBraking();
            }
            if (verticalInput < 0 && movingForward == false)
            {
                ResetBraking();
                // Player is reversing

            }
        }

        /// <summary>
        /// Checks what way the car is moving.
        /// </summary>
        /// <returns>Returns true if movement is forward</returns>
        private bool IsCarMovingForward()
        {
            // Calculate the dot product between the car's forward direction and its velocity vector
            float forwardVelocityDotProduct = Vector3.Dot(wheelColliderFrontL.attachedRigidbody.velocity.normalized, transform.forward);

            // Check if the dot product is positive, indicating movement in the forward direction
            return forwardVelocityDotProduct > 0; //True or false 
        }

        /// <summary>
        /// Checks if the car is moving backwards
        /// TODO: This really should be in the forward check or something.
        /// </summary>
        /// <returns>Returns true if the car is moving backwards</returns>
        private bool IsCarMovingBackwards()
        {
            // Calculate the dot product between the car's forward direction and its velocity vector
            float forwardVelocityDotProduct = Vector3.Dot(wheelColliderFrontL.attachedRigidbody.velocity.normalized, transform.forward);

            // Check if the dot product is positive, indicating movement in the forward direction
            return forwardVelocityDotProduct < 0; //True or false 
        }


        /// <summary>
        /// Controls the car's motor, applying force to move the car.
        /// </summary>
        private void HandleMotor()
        {
            float speedFL = wheelColliderFrontL.attachedRigidbody.velocity.magnitude;
            float speedFR = wheelColliderFrontR.attachedRigidbody.velocity.magnitude;
            float speedRL = wheelColliderRearL.attachedRigidbody.velocity.magnitude;
            float speedRR = wheelColliderRearR.attachedRigidbody.velocity.magnitude;

            speedFL *= 3.6f; // Convert to km/h.
            speedFR *= 3.6f;
            speedRL *= 3.6f;
            speedRR *= 3.6f;


            Rigidbody carRigidbody = wheelColliderFrontL.attachedRigidbody; //Tilf�j flere??
            carSpeed = carRigidbody.velocity.magnitude * 3.6f; // Convert to km/h

            if (carSpeed > maxSpeed)
            {
                carRigidbody.velocity = carRigidbody.velocity.normalized * (maxSpeed / 3.6f); // Set velocity to max speed
            }

            if (!carActive)
            {

                float combinedSpeed = (speedFL + speedFR + speedRL + speedRR) / 4; // Average speed of all wheels.

                if (combinedSpeed > 0)
                {
                    ApplyBrakingToStop();
                }
                return; // Skip the rest of the method if the car isn't active.
            }

            // Reset brake torque when car is active.
            //ResetBraking();

            // Apply motor force if under max speed.
            if (IsCarMovingBackwards() == false)
            {
                //Going forward
                if (speedFL < maxSpeed|| speedFR < maxSpeed || speedRR < maxSpeed || speedRL < maxSpeed )
                {
                    wheelColliderFrontL.motorTorque = verticalInput * motorForce;
                    wheelColliderFrontR.motorTorque = verticalInput * motorForce;
                    wheelColliderRearL.motorTorque = verticalInput * motorForce;
                    wheelColliderRearR.motorTorque = verticalInput * motorForce;
                }
                else
                {

                    wheelColliderFrontL.motorTorque = 0;
                    wheelColliderFrontR.motorTorque = 0;
                    wheelColliderRearL.motorTorque = 0;
                    wheelColliderRearR.motorTorque = 0;
                }
            }
            if (IsCarMovingForward() == false)
            {
                //Going Backwards
                if (speedFL < reverseMaxSpeed || speedFR < reverseMaxSpeed || speedRR < reverseMaxSpeed || speedRL < reverseMaxSpeed)
                {
                    wheelColliderFrontL.motorTorque = verticalInput * motorForce/2;
                    wheelColliderFrontR.motorTorque = verticalInput * motorForce/2;
                    wheelColliderRearL.motorTorque = verticalInput * motorForce/2;
                    wheelColliderRearR.motorTorque = verticalInput * motorForce/2;
                }
                else
                {

                    wheelColliderFrontL.motorTorque = 0;
                    wheelColliderFrontR.motorTorque = 0;
                    wheelColliderRearL.motorTorque = 0;
                    wheelColliderRearR.motorTorque = 0;
                }
            }


        }
        
        /// <summary>
        /// Attempts to brake.
        /// </summary>
        private void ApplyBrakingToStop()
        {

            wheelColliderFrontL.brakeTorque = brakingForce;
            wheelColliderFrontR.brakeTorque = brakingForce;
            wheelColliderRearL.brakeTorque = brakingForce;
            wheelColliderRearR.brakeTorque = brakingForce;

        }

        /// <summary>
        /// Stops the braking.
        /// </summary>
        private void ResetBraking()
        {
            wheelColliderFrontL.brakeTorque = 0;
            wheelColliderFrontR.brakeTorque = 0;
            wheelColliderRearL.brakeTorque = 0;
            wheelColliderRearR.brakeTorque = 0;
        }

        /// <summary>
        /// Manages the car's steering mechanism.
        /// </summary>
        private void HandleSteering()
        {

            if (carActive)
            {
                // Calculate the steering angle based on player input when car is active.
                currentSteerAngle = maxSteeringAngle * horizontalInput;
            }
            else
            {
                // Gradually move the current steer angle back to 0 when car is inactive.
                currentSteerAngle = Mathf.Lerp(currentSteerAngle, 0, Time.deltaTime * steeringCorrectionRate);
            }

            // Apply the steer angle to the wheel colliders.
            wheelColliderFrontL.steerAngle = currentSteerAngle;
            wheelColliderFrontR.steerAngle = currentSteerAngle;


            //// Calculate the steering angle based on player's input.
            //currentSteerAngle = maxSteeringAngle * horizontalInput;
            //wheelColliderFrontL.steerAngle = currentSteerAngle;
            //wheelColliderFrontR.steerAngle = currentSteerAngle;
        }

        /// <summary>
        /// Updates the position and rotation of the wheel models to match the physics
        /// </summary>
        private void UpdateWheels()
        {
            UpdateSingleWheel(wheelColliderFrontL, wheelTransformFrontL);
            UpdateSingleWheel(wheelColliderFrontR, wheelTransformFrontR);
            UpdateSingleWheel(wheelColliderRearL, wheelTransformRearL);
            UpdateSingleWheel(wheelColliderRearR, wheelTransformRearR);
        }

        /// <summary>
        /// Updates a single wheel's visual representation to match its collider.
        /// </summary>
        private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
        {
            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);
            wheelTransform.position = pos;
            wheelTransform.rotation = rot * Quaternion.Euler(0, 0, 90);
        }
    }
}
