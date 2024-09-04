using UnityEngine;

public class CarMainWorldMovement : MonoBehaviour
{
    private bool carActive; //the car state
    public bool CarActive
    {
        get { return carActive; }
        set { carActive = value; }
    }
    private float fuelAmount;

    // Constants for input axes names, used for reading player input.
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";


    private float horizontalInput;
    private float forwardInput;


    private float currentSteerAngle;
    private float currentBreakForce;

    public float motorForce; // Power of the car's engine.
    public float maxSteeringAngle; // Maximum angle for steering.
    public float maxSpeed; // Maximum speed for the car.
    public float reverseMaxSpeed;

    /// <summary>
    /// Brake torque expressed in Newton metres.
    /// </summary>
    private float brakingTorque = 1000000f;
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
    // public GameObject leftHeadlight;
    // public GameObject rightHeadlight;

    //Steering correction when parking
    [SerializeField] private float steeringCorrectionRate;
    private Rigidbody rb;

    // Start is called before the first frame update.
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fuelAmount = GetComponent<CarFuel>().fuelAmount;
        if (carActive)
        {
            //leftHeadlight.SetActive(carActive == true);
            //rightHeadlight.SetActive(carActive == true);
        }
        else
        {
            //leftHeadlight.SetActive(carActive == false);
            //rightHeadlight.SetActive(carActive == false);
        }

    }

    private void Update()
    {
        // Toggle CarActive state when 'E' key is pressed.
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Toggle the headlights based on the CarActive state.
            // leftHeadlight.SetActive(carActive);
            //  rightHeadlight.SetActive(carActive);
        }
    }

    // FixedUpdate is called regularly at fixed intervals for physics updates.
    private void FixedUpdate()
    {
        HandleSteering();  // Manages the car's steering based on input.
        HandleMotor(); // Handle motor logic irrespective of CarActive state.
        UpdateRotationWheels();    // Updates the visual representation of the wheels.

        if (carActive && GetComponent<CarFuel>().fuelAmount > 0)
        {
            GetInput(); // Reads the player's input.
            print(GetComponent<CarFuel>().fuelAmount);
        }

    }

    /// <summary>
    /// Reads player's input from keyboard
    /// </summary>

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        forwardInput = Input.GetAxis(VERTICAL);

        // Check if the car is currently moving forward or is stopped
        bool movingForward = IsCarMovingForward();


        if (forwardInput < 0 && movingForward == true)
        {
            // Player is attempting to reverse while the car is moving forward
            ApplyBrakingToStop();

        }
        else if (forwardInput > 0)
        {

            // Player is moving forward
            ResetBraking();
        }
        if (forwardInput < 0 && movingForward == false)
        {
            ResetBraking();
            // Player is reversing
        }
    }

    private bool IsCarMovingForward()
    {
        // Calculate the dot product between the car's forward direction and its velocity vector
        float forwardVelocityDotProduct = Vector3.Dot(wheelColliderFrontL.attachedRigidbody.velocity.normalized, transform.forward);

        // Check if the dot product is positive, indicating movement in the forward direction
        return forwardVelocityDotProduct > 0; //True or false 
    }

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
        carSpeed = rb.velocity.magnitude * 3.6f; // Convert to km/h

        if (carSpeed > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * (maxSpeed / 3.6f); // Set velocity to max speed
        }

        if (!carActive)
        {
            if (carSpeed > 0)
            {
                ApplyBrakingToStop();
            }
            return; // Skip the rest of the method if the car isn't active.
        }

        // Apply motor force if under max speed.
        if (IsCarMovingBackwards() == false)
        {
            //Going forward
            if (carSpeed < maxSpeed)
            {
                wheelColliderFrontL.motorTorque = forwardInput * motorForce;
                wheelColliderFrontR.motorTorque = forwardInput * motorForce;
                wheelColliderRearL.motorTorque = forwardInput * motorForce;
                wheelColliderRearR.motorTorque = forwardInput * motorForce;
                RemoveFuel();
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
            if (carSpeed < reverseMaxSpeed)
            {
                wheelColliderFrontL.motorTorque = forwardInput * motorForce / 2;
                wheelColliderFrontR.motorTorque = forwardInput * motorForce / 2;
                wheelColliderRearL.motorTorque = forwardInput * motorForce / 2;
                wheelColliderRearR.motorTorque = forwardInput * motorForce / 2;
                RemoveFuel();
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
    float fuelTimer;
    private void RemoveFuel()
    {
        fuelTimer += Time.deltaTime;
        if (fuelTimer >0.5f)
        {
            GetComponent<CarFuel>().fuelAmount -= 0.001f;
            fuelTimer = 0;
        }
    }

    public void ApplyBrakingToStop()
    {
        //carSpeed = 0;
        wheelColliderFrontL.brakeTorque = brakingTorque;
        wheelColliderFrontR.brakeTorque = brakingTorque;
        wheelColliderRearL.brakeTorque = brakingTorque;
        wheelColliderRearR.brakeTorque = brakingTorque;

    }

    private void ResetBraking()
    {
        wheelColliderFrontL.brakeTorque = 0;
        wheelColliderFrontR.brakeTorque = 0;
        wheelColliderRearL.brakeTorque = 0;
        wheelColliderRearR.brakeTorque = 0;
    }




    /// <summary>
    /// Manages the car's steering
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


    /// <summary>
    /// Updates the position and rotation of the wheel models to match the physics
    /// </summary>
    private void UpdateRotationWheels()
    {
        UpdateRotationOfSingleWheel(wheelColliderFrontL, wheelTransformFrontL);
        UpdateRotationOfSingleWheel(wheelColliderFrontR, wheelTransformFrontR);
        UpdateRotationOfSingleWheel(wheelColliderRearL, wheelTransformRearL);
        UpdateRotationOfSingleWheel(wheelColliderRearR, wheelTransformRearR);
    }

    /// <summary>
    /// Updates a single wheel's visual representation to match its collider.
    /// </summary>
    private void UpdateRotationOfSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot * Quaternion.Euler(0, 0, 90);
    }
}
