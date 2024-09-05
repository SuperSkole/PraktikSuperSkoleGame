using Spine.Unity;
using System.Collections;
using UnityEngine;

public class SpinePlayerMovement : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset walk;
    public AnimationReferenceAsset throwing;
    public AnimationReferenceAsset idle;
    public string currentState;

    //Blend paramters
    public float walkThreshold = 0.1f;
    public float blendDuration = 0.2f;
    public float moveSpeed = 5.0f;
    public bool facingRight = true;

    public Camera sceneCamera;
    public LayerMask placementLayermask;
    private Vector3 targetPosition;
    private bool isMoving;
    private Coroutine moveCoroutine;

    public bool hoveringOverUI = false;
    [SerializeField] private GameObject interactionGO;
    [SerializeField] ParticleSystem pointAndClickEffect;
    [SerializeField] private Rigidbody rigidbody;

    [SerializeField] private float rideHeight = 2f;
    [SerializeField] private float rideSpringStrength = 1f;
    [SerializeField] private float rideSpringDamper = 1f;

    /// <summary>
    /// Initializes the player's animation state to idle.
    /// </summary>
    void Start()
    {
        SceneStart();

        //When player gets instanced we need to add the sceneCamera or movement is not going to work
        //skeletonAnimation.Skeleton.SetAttachment("Monster TOP1", "Monster TOP1");
    }
    public void SceneStart()
    {
        currentState = "idle";
        SetCharacterState("Idle");
    }
    /// <summary>
    /// Handles player input for both WASD movement and point-and-click movement.
    /// </summary>
    void Update()
    {
        //if (!isMoving || Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        //{
            //if (isMoving)
            //{
                //StopPointAndClickMovement();
            //}
            //PlayerWASDMovement();
        //}

        if (!hoveringOverUI && Input.GetMouseButtonDown(0))
        {
            Vector3 newMoveToPos = GetSelectedMapPosition();
            if (newMoveToPos != Vector3.zero)
            {
                StartMovement(newMoveToPos);
            }
        }

    }

    private void FixedUpdate()
    {
        if (!isMoving || Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            if (isMoving)
            {
                StopPointAndClickMovement();
            }
            PlayerWASDMovement();
        }
        if(isMoving && Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            MoveToTarget();
        }
        Floating();
    }

    private void Floating()
    {
        bool rayDidHit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f);
        if (rayDidHit)
        {
            Vector3 vel = rigidbody.velocity;
            Vector3 rayDir = Vector3.down;

            Vector3 otherVel = Vector3.zero;

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = hit.distance - rideHeight;

            float springForce = (x * rideSpringStrength) - (relVel * rideSpringDamper);

            Debug.DrawLine(transform.position, transform.position + (rayDir * springForce), Color.yellow);

            rigidbody.AddForce(rayDir * springForce);
        }
    }

    /// <summary>
    /// Handles movement of the player using WASD keys.
    /// </summary>
    private void PlayerWASDMovement()
    {
        //StopCoroutine(MoveToTarget());
        //Remove Raw to add inertia
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * moveSpeed;

        // Move the player
        rigidbody.velocity = new(movement.x, rigidbody.velocity.y, movement.z);

        //transform.Translate(movement * moveSpeed * Time.deltaTime);

        // Flip the player based on the horizontal input
        if (rigidbody.velocity.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            interactionGO.transform.localScale = new Vector3(Mathf.Abs(-interactionGO.transform.localScale.x), interactionGO.transform.localScale.y, interactionGO.transform.localScale.z);
            interactionGO.transform.localPosition = new Vector3(3.75f, 2.5f, -2.5f);
        }
        else if (rigidbody.velocity.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            interactionGO.transform.localScale = new Vector3(-Mathf.Abs(-interactionGO.transform.localScale.x), interactionGO.transform.localScale.y, interactionGO.transform.localScale.z);
            interactionGO.transform.localPosition = new Vector3(-3.75f, 2.5f, -2.5f);
        }

        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            SetCharacterState("Walk");
        }
        else
        {
            SetCharacterState("Idle");
        }
    }
    /// <summary>
    /// Starts the movement of the player towards a target position clicked on the map.
    /// </summary>
    /// <param name="newMoveToPos">The target position to move towards.</param>
    private void StartMovement(Vector3 newMoveToPos)
    {
        targetPosition = newMoveToPos;
        isMoving = true;
        // Stop any ongoing point-and-click movement
        if (isMoving)
        {
            SetCharacterState("Idle");
        }
        if (transform.position.x > targetPosition.x)
        {
            // Moving right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            interactionGO.transform.localScale = new Vector3(Mathf.Abs(-interactionGO.transform.localScale.x), interactionGO.transform.localScale.y, interactionGO.transform.localScale.z);
            interactionGO.transform.localPosition = new Vector3(3.75f, 2.5f, -2.5f);

        }
        else
        {
            // Moving left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            interactionGO.transform.localScale = new Vector3(-Mathf.Abs(-interactionGO.transform.localScale.x), interactionGO.transform.localScale.y, interactionGO.transform.localScale.z);
            interactionGO.transform.localPosition = new Vector3(-3.75f, 2.5f, -2.5f);
        }
        SetCharacterState("Walk");
        var effect = Instantiate(pointAndClickEffect, new Vector3(targetPosition.x, targetPosition.y - 1.892f, targetPosition.z), pointAndClickEffect.transform.rotation);
        Destroy(effect.gameObject, 0.5f);
    }

    /// <summary>
    /// Coroutine that smoothly moves the player towards the target position.
    /// </summary>
    /// <returns></returns>
    private void MoveToTarget()
    {
        if(Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Move towards the target position at the specified speed
            Vector3 moveVel = (targetPosition - transform.position).normalized * moveSpeed;
            rigidbody.velocity = new(moveVel.x,rigidbody.velocity.y,moveVel.z);
            return;
        }
        // Snap to the exact position when very close to avoid overshooting
        transform.position = targetPosition;
        isMoving = false;
    }

    /// <summary>
    /// Stops the point-and-click movement of the player.
    /// </summary>
    public void StopPointAndClickMovement()
    {
        isMoving = false;
    }
    /// <summary>
    /// Gets the position on the map that the player clicked on.
    /// </summary>
    /// <returns>The position on the map where the player clicked, or Vector3.zero if no valid position was found.</returns>
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, placementLayermask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// Sets the player's animation based on the specified parameters.
    /// </summary>
    /// <param name="animation">The animation to set.</param>
    /// <param name="loop">Whether the animation should loop.</param>
    /// <param name="timeScale">The speed at which the animation should play.</param>
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    /// <summary>
    /// Sets the player's animation state to either idle or walk, with blending between states.
    /// </summary>
    /// <param name="state">The desired animation state ("Idle" or "Walk").</param>
    public void SetCharacterState(string state)
    {
        if (state.Equals("Idle") && currentState != "Idle")
        {
            //Blending animations walk - idle
            skeletonAnimation.state.SetAnimation(0, idle, true).MixDuration = blendDuration;
            currentState = "Idle";
        }
        else if (state.Equals("Walk") && currentState != "Walk")
        {
            //Blending animations idle - walk
            skeletonAnimation.state.SetAnimation(0, walk, true).MixDuration = blendDuration;
            currentState = "Walk";
        }
        else if (state.Equals("Throw") && currentState != "Throw")
        {
            //Blending animations idle - walk
            skeletonAnimation.state.SetAnimation(0, throwing, false).MixDuration = blendDuration;
            currentState = "Throw";
        }
    }
}
