using UnityEngine;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class CarController : MonoBehaviour
    {
        #region Variables

        #region Car activity & axe constants
        public bool carActive = false; //the car state
        #endregion

        #region Player input
        // Constants for input axes names, used for reading player input.
        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";
        private float horizontalInput;
        private float verticalInput;
        #endregion

        #region Steering, speed & braking
        private float currentSteerAngle;

        public float motorForce; // Power of the car's engine.
        public float maxSteeringAngle; // Maximum angle for steering.
        public float maxSpeed; // Maximum speed for the car.
        public float reverseMaxSpeed;

        public float brakingForce = 10000000f;
        public float carSpeed;

        //Steering correction when parking
        public float steeringCorrectionRate = 2.0f;
        #endregion

        #region Wheels
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

        // Speed of each wheel
        private float speedFL;
        private float speedFR;
        private float speedRL;
        private float speedRR;
        #endregion

        #endregion

        #region Functions

        #region Setup and update
        /// <summary>
        /// Sets up the car once the map is ready.
        /// </summary>
        public void Setup()
        {
            carActive = true; // Start with the car being off.
        }

        /// <summary>
        /// Handles the car input.
        /// </summary>
        private void FixedUpdate()
        {
            HandleSteering();  // Manages the car's steering based on input.
            MoveTheCar();
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
        }
        #endregion

        #region Car movement
        /// <summary>
        /// Controls the cars movement and checks what direction to move it
        /// </summary>
        private void MoveTheCar()
        {
            if (!carActive)
            {
                if (IsCarMoving())
                {
                    ApplyBrakingToStop();
                }
                return; // Skip the rest of the method if the car isn't active.
            }

            Rigidbody carRigidbody = wheelColliderFrontL.attachedRigidbody; //Tilføj flere??
            carSpeed = carRigidbody.velocity.magnitude * 3.6f; // Convert to km/h

            if (carSpeed > maxSpeed)
            {
                carRigidbody.velocity = carRigidbody.velocity.normalized * (maxSpeed / 3.6f); // Set velocity to max speed
            }

            if (verticalInput > 0)
            {
                MoveCarForward();
            }
            else if (verticalInput < 0)
            {
                MoveCarBackward();
            }
            else if (verticalInput == 0 && (IsCarMovingBackwards() || IsCarMovingForward()))
            {
                ApplyBrakingToStop();
            }
        }

        /// <summary>
        /// Attempts to drive forward
        /// </summary>
        private void MoveCarForward()
        {
            if (IsCarMovingBackwards()) // If the car is going backwards, brake it
            {
                ApplyBrakingToStop();
            }
            else // If car is not going backwards, go forward
            {
                ResetBraking();
                if (WheelSpeedCheck(true)) // Apply speed if not at max speed
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
        }
        /// <summary>
        /// Attempts to drive backwards
        /// </summary>
        private void MoveCarBackward()
        {
            if (IsCarMovingForward()) // If the car is going forward, brake it
            {
                ApplyBrakingToStop();
            }
            else // If car is not going forward, go backwards
            {
                ResetBraking();
                if (WheelSpeedCheck(false)) // Apply speed if not at max speed
                {
                    wheelColliderFrontL.motorTorque = verticalInput * motorForce / 2;
                    wheelColliderFrontR.motorTorque = verticalInput * motorForce / 2;
                    wheelColliderRearL.motorTorque = verticalInput * motorForce / 2;
                    wheelColliderRearR.motorTorque = verticalInput * motorForce / 2;
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
        #endregion

        #region Car movement & speed check
        /// <summary>
        /// Checks if the car is moving at all.
        /// </summary>
        /// <returns>Returns true if the car is moving at all</returns>
        private bool IsCarMoving()
        {
            if (IsCarMovingForward() || IsCarMovingBackwards())
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if the car is moving forward
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
        /// Checks the speed of all the wheels
        /// </summary>
        /// <param name="forward">Should it check for going forward? If not, it checks for going backwards</param>
        /// <returns>Returns true if any wheel is below the max speed</returns>
        private bool WheelSpeedCheck(bool forward)
        {
            speedFL = wheelColliderFrontL.attachedRigidbody.velocity.magnitude;
            speedFR = wheelColliderFrontR.attachedRigidbody.velocity.magnitude;
            speedRL = wheelColliderRearL.attachedRigidbody.velocity.magnitude;
            speedRR = wheelColliderRearR.attachedRigidbody.velocity.magnitude;

            speedFL *= 3.6f; // Convert to km/h.
            speedFR *= 3.6f;
            speedRL *= 3.6f;
            speedRR *= 3.6f;
            if (forward)
            {
                if (speedFL < maxSpeed || speedFR < maxSpeed || speedRR < maxSpeed || speedRL < maxSpeed)
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (speedFL < reverseMaxSpeed || speedFR < reverseMaxSpeed || speedRR < reverseMaxSpeed || speedRL < reverseMaxSpeed)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region Braking & steering
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
        }
        #endregion

        #region Wheel updates
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
            wheelTransform.rotation = rot * Quaternion.Euler(0, 0, 0);
        }
        #endregion

        #endregion
    }
}
