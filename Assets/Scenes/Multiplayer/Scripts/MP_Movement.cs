using CORE;
using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Scenes._20_MainWorld.Scripts;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class MP_Movement : NetworkBehaviour
{
    [SerializeField] ColorChanging colorChange;
    public float moveSpeed = 5f;
    private Vector3 movementInput;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerAnimatior animator;
    [SerializeField] private Transform spriteTransform;

    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<FixedString32Bytes> colorPick = new NetworkVariable<FixedString32Bytes>();
    public override void OnNetworkSpawn()
    {
        ISkeletonComponent skeleton = GetComponentInChildren<ISkeletonComponent>();
        colorChange.SetSkeleton(skeleton);
        colorPick.OnValueChanged += UpdateColor;
        if (skeleton == null)
        {
            Debug.LogError("PlayerManager.SetupPlayer(): " +
                           "ISkeleton component not found on spawned player.");
            return;
        }
        if (IsServer)
        {
            rb.position = GameObject.Find("SpawnPoint").transform.position;
        }
        if (IsClient)
        {
            if (IsOwner)
            {
                GameObject originPlayer = GameObject.Find("PlayerMonster");
                string monsterColor = originPlayer.GetComponent<PlayerData>().MonsterColor;
                RequestColorPickServerRpc(monsterColor);
                Debug.Log("color sent as " + monsterColor);
            }
        }
        if (MonsterColorIsSet(colorPick.Value.ToString()))
        {
            colorChange.ColorChange(colorPick.Value.ToString());
        }
    }

    private void Update()
    {
        // Only process input and movement for the local player
        if (IsOwner)
        {
            HandleInput();
            HandleAnimation(); // Handle the animation for walking/idle states
        }
    }

    private void FixedUpdate()
    {
        // Only the owner requests the movement, but the server executes it
        if (IsOwner)
        {
            RequestMovePlayerServerRpc(movementInput);
        }
    }

    bool MonsterColorIsSet(string color)
    {
        if (color != "white" && color != "" && color != null)
            return true;
        return false;
    }

    // ServerRpc to allow the client to request a color update
    [ServerRpc]
    private void RequestColorPickServerRpc(string color)
    {
        Debug.Log("setting color");
        colorPick.Value = new FixedString32Bytes(color);
        Debug.Log(color);
    }

    void UpdateColor(FixedString32Bytes past, FixedString32Bytes current)
    {
        colorChange.ColorChange(current.ToString());
        UpdateColorClientRpc(past, current);
    }

    [ClientRpc]
    private void UpdateColorClientRpc(FixedString32Bytes past, FixedString32Bytes current)
    {
        if (MonsterColorIsSet(current.ToString()))
        {
            colorChange.ColorChange(current.ToString());
        }
    }

    // Handle WASD input
    private void HandleInput()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down arrow
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        movementInput = new Vector3(horizontalInput, 0f, verticalInput).normalized * moveSpeed;
        movementInput = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * movementInput;
    }

    // Handle animation based on movement input
    private void HandleAnimation()
    {
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            // Walking animation
            UpdateAnimationStateServerRpc("Walk"); // Send animation state to server
        }
        else
        {
            // Idle animation
            UpdateAnimationStateServerRpc("Idle"); // Send animation state to server
        }
    }

    // Server-side method to handle the movement
    [ServerRpc]
    private void RequestMovePlayerServerRpc(Vector3 movement)
    {
        // Apply the movement on the server-side for the Rigidbody
        rb.velocity = new(movement.x, rb.velocity.y, movement.z);
        // Handle sprite flip based on the movement direction
        if (movement.x != 0)
        {
            Debug.Log("Flip!");
            bool isMovingRight = movement.x < 0; // Corrected logic for determining right movement
                                                 // Flip the sprite based on the direction the server detected
            if (isMovingRight)
            {
                // Moving right: set sprite's X scale to positive
                rb.transform.localScale = new Vector3(Mathf.Abs(rb.transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
                Debug.Log("Moving right!");
            }
            else
            {
                // Moving left: set sprite's X scale to negative
                rb.transform.localScale = new Vector3(-Mathf.Abs(rb.transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
                Debug.Log("Moving left!");
            }
            UpdateSpriteFlipClientRpc(isMovingRight); // Send flip state to all clients
        }

        // Notify all clients about the position update
        SyncPlayerPositionClientRpc(rb.position);
    }

    // Synchronize the position across all clients
    [ClientRpc]
    private void SyncPlayerPositionClientRpc(Vector3 newPosition)
    {
        if (!IsOwner)
        {
            // Update the position for all other clients
            rb.position = newPosition;
        }
    }

    // ServerRpc to handle animation state updates sent by the client
    [ServerRpc]
    private void UpdateAnimationStateServerRpc(string animationState)
    {
        // Tell all clients to update their animation state based on the owner's input
        UpdateAnimationStateClientRpc(animationState);
        animator.SetCharacterState(animationState);
    }

    // ClientRpc to broadcast the animation state to all clients
    [ClientRpc]
    private void UpdateAnimationStateClientRpc(string animationState)
    {
        animator.SetCharacterState(animationState);
    }

    // ClientRpc to handle sprite flip, broadcasted by the server
    [ClientRpc]
    private void UpdateSpriteFlipClientRpc(bool isMovingRight)
    {
        // Flip the sprite based on the direction the server detected
        if (isMovingRight)
        {
            // Moving right: set sprite's X scale to positive
            rb.transform.localScale = new Vector3(Mathf.Abs(rb.transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
            Debug.Log("Moving right!");
        }
        else
        {
            // Moving left: set sprite's X scale to negative
            rb.transform.localScale = new Vector3(-Mathf.Abs(rb.transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
            Debug.Log("Moving left!");
        }
    }
}
