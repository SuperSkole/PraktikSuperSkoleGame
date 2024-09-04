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
    [SerializeField] ParticleSystem pointAndClickEffect;
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
        if (!isMoving || Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            if (isMoving)
            {
                StopPointAndClickMovement();
            }
            PlayerWASDMovement();
        }

        if (!hoveringOverUI && Input.GetMouseButtonDown(0))
        {
            Vector3 newMoveToPos = GetSelectedMapPosition();
            if (newMoveToPos != Vector3.zero)
            {
                StartMovement(newMoveToPos);
            }
        }
    }

    /// <summary>
    /// Handles movement of the player using WASD keys.
    /// </summary>
    private void PlayerWASDMovement()
    {
        StopCoroutine(MoveToTarget());
        //Remove Raw to add inertia
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Move the player
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // Flip the player based on the horizontal input
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        }

        if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
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
        targetPosition.y = transform.position.y; // Keep the player's y position constant

        // Stop any ongoing point-and-click movement
        if (isMoving && moveCoroutine != null)
        {
            SetCharacterState("Idle");
            StopCoroutine(moveCoroutine);

        }
        if (transform.position.x > targetPosition.x)
        {
            // Moving right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // Moving left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        SetCharacterState("Walk");
        var effect = Instantiate(pointAndClickEffect, new Vector3(targetPosition.x, targetPosition.y - 1.892f, targetPosition.z), pointAndClickEffect.transform.rotation);
        Destroy(effect.gameObject, 0.5f);
        moveCoroutine = StartCoroutine(MoveToTarget());
    }

    /// <summary>
    /// Coroutine that smoothly moves the player towards the target position.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveToTarget()
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Move towards the target position at the specified speed
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            yield return null;
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
        if (isMoving && moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            isMoving = false;
        }
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
