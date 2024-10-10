using Cinemachine;
using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MP_MovementHandler : NetworkBehaviour
{
    /// <summary>
    /// The network script that should be on all player instances instantiated by the server.
    /// It handles movement, colors, clothes and animation.
    /// ClientRPC: Things done on the client when called by the server.
    /// ServerRPC: Things done on the server, usually to call ClientRPC.
    /// </summary>


    #region fields
    public float moveSpeed = 5f;
    private Vector3 movementInput;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerAnimatior animator;
    [SerializeField] private Transform spriteTransform;
    private ISkeletonComponent skeleton;
    MP_PlayerSetupHandler playerSetup;

    [SerializeField] private GameObject textDisplay;
    public NetworkVariable<Vector3> textRotation = new();
    #endregion

    #region NetworkSpawn/Despawn
    /// <summary>
    /// Sets up the entire gameobject when it spawns in the network
    /// Both when it is a new client joining the server, but also everyone else for that client
    /// Important note: If any additions are made, then it NEEDS to set here what it is like when joining
    /// Else new players can't see what is up with older players
    /// </summary>
    public override void OnNetworkSpawn()
    {
        skeleton = GetComponentInChildren<ISkeletonComponent>();
        playerSetup = GetComponent<MP_PlayerSetupHandler>();
        playerSetup.SetupCharacter();
        rb.transform.position = GameObject.Find("MPPlayerSpawn").transform.position;
    }
    #endregion

    #region update
    /// <summary>
    /// Updates the player to check their input and animation
    /// </summary>
    private void Update()
    {
        if (IsOwner)
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                HandleInput();
                HandleAnimation();
            }
            else
            {
                movementInput = new Vector3();
                UpdateAnimationStateServerRpc("Idle");
            }
        }
    }

    /// <summary>
    /// Asks the server to move the player in the desired direction
    /// </summary>
    private void FixedUpdate()
    {
        if (IsOwner)
        {
            RequestMovePlayerServerRpc(movementInput);
        }
    }
    #endregion

    #region Player Input
    /// <summary>
    /// Handles the players input for their desired movement
    /// </summary>
    private void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        movementInput = new Vector3(horizontalInput, 0f, verticalInput).normalized * moveSpeed;
        movementInput = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * movementInput;
    }

    /// <summary>
    /// Handle animation based on movement input
    /// </summary>
    private void HandleAnimation()
    {
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            UpdateAnimationStateServerRpc("Walk");
        }
        else
        {
            UpdateAnimationStateServerRpc("Idle");
        }
    }
    #endregion

    #region Movement
    /// <summary>
    /// Server-side method to handle the movement
    /// </summary>
    /// <param name="movement">Movement Input</param>
    [ServerRpc]
    private void RequestMovePlayerServerRpc(Vector3 movement)
    {
        rb.velocity = new(movement.x, rb.velocity.y, movement.z);
        if (movement.x != 0)
        {
            bool isMovingRight = movement.x < 0;
            if (isMovingRight)
            {
                textDisplay.transform.localScale = new Vector3(Mathf.Abs(textDisplay.transform.localScale.x), textDisplay.transform.localScale.y, textDisplay.transform.localScale.z);
                rb.transform.localScale = new Vector3(Mathf.Abs(rb.transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
            }
            else
            {
                textDisplay.transform.localScale = new Vector3(-Mathf.Abs(textDisplay.transform.localScale.x), textDisplay.transform.localScale.y, textDisplay.transform.localScale.z);
                rb.transform.localScale = new Vector3(-Mathf.Abs(rb.transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
            }
            textRotation.Value = textDisplay.transform.localScale;
            UpdateSpriteFlipClientRpc(isMovingRight);
        }
        SyncPlayerPositionClientRpc(rb.position);
    }

    /// <summary>
    /// Sets the players position
    /// </summary>
    /// <param name="newPosition">Sets the players new position</param>
    [ClientRpc]
    private void SyncPlayerPositionClientRpc(Vector3 newPosition)
    {
        if (!IsOwner)
        {
            rb.position = newPosition;
        }
    }

    /// <summary>
    /// Tells the player to flip the sprite to look the other direction.
    /// </summary>
    /// <param name="isMovingRight">Is the player moving towards the right?</param>
    [ClientRpc]
    private void UpdateSpriteFlipClientRpc(bool isMovingRight)
    {
        if (isMovingRight)
        {
            textDisplay.transform.localScale = new Vector3(Mathf.Abs(textDisplay.transform.localScale.x), textDisplay.transform.localScale.y, textDisplay.transform.localScale.z);
            rb.transform.localScale = new Vector3(Mathf.Abs(rb.transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
        }
        else
        {
            textDisplay.transform.localScale = new Vector3(-Mathf.Abs(textDisplay.transform.localScale.x), textDisplay.transform.localScale.y, textDisplay.transform.localScale.z);
            rb.transform.localScale = new Vector3(-Mathf.Abs(rb.transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
        }
    }
    #endregion

    #region AnimationHandler
    /// <summary>
    /// Updates the server side of animations
    /// </summary>
    /// <param name="animationState">Sets the players new animation state</param>
    [ServerRpc]
    private void UpdateAnimationStateServerRpc(string animationState)
    {
        UpdateAnimationStateClientRpc(animationState);
        animator.SetCharacterState(animationState);
    }

    /// <summary>
    /// Sets the players active animation
    /// </summary>
    /// <param name="animationState">The active animation</param>
    [ClientRpc]
    private void UpdateAnimationStateClientRpc(string animationState)
    {
        animator.SetCharacterState(animationState);
    }
    #endregion

    #region CheckIfTrue
    /// <summary>
    /// Checks if the clothing or color exists
    /// </summary>
    /// <param name="CheckedItem">What is to be checked</param>
    /// <returns>Is the value not null or similar?</returns>
    private bool StringIsSetCheck(string CheckedItem)
    {
        if (CheckedItem is not "white" and not "" and not null)
            return true;
        return false;
    }
    #endregion
}