using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class MP_Movement : NetworkBehaviour
{
    /// <summary>
    /// The network script that should be on all player instances instantiated by the server.
    /// It handles movement, colors, clothes and animation.
    /// ClientRPC: Things done on the client when called by the server.
    /// ServerRPC: Things done on the server, usually to call ClientRPC.
    /// </summary>


    #region fields
    [SerializeField] private ColorChanging colorChange;
    [SerializeField] private ClothChanging clothChange;
    public float moveSpeed = 5f;
    private Vector3 movementInput;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerAnimatior animator;
    [SerializeField] private Transform spriteTransform;
    private ISkeletonComponent skeleton;

    public NetworkVariable<Vector3> Position = new();
    public NetworkVariable<FixedString32Bytes> colorPick = new();
    public NetworkVariable<FixedString32Bytes> clothMid = new();
    public NetworkVariable<FixedString32Bytes> clothTop = new();
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
        colorChange.SetSkeleton(skeleton);
        colorPick.OnValueChanged += UpdateColorServerRpc;
        clothMid.OnValueChanged += UpdateCloth;
        clothTop.OnValueChanged += UpdateCloth;
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
                string clothMid = originPlayer.GetComponent<PlayerData>().ClothMid;
                string clothTop = originPlayer.GetComponent<PlayerData>().ClothTop;
                RequestclothingMidPickServerRpc(clothMid);
                RequestclothingTopPickServerRpc(clothTop);
            }
        }
        if (MonsterColorOrClothingIsSet(colorPick.Value.ToString()))
            colorChange.ColorChange(colorPick.Value.ToString());
        if (MonsterColorOrClothingIsSet(clothMid.Value.ToString()))
            clothChange.ChangeClothes(clothMid.Value.ToString(), skeleton);
        if (MonsterColorOrClothingIsSet(clothTop.Value.ToString()))
            clothChange.ChangeClothes(clothTop.Value.ToString(), skeleton);
    }

    /// <summary>
    /// Ensures we remove some calls when a player is removed from the network, usually by disconnecting
    /// </summary>
    public override void OnNetworkDespawn()
    {
        colorPick.OnValueChanged -= UpdateColorServerRpc;
        clothMid.OnValueChanged -= UpdateCloth;
        clothTop.OnValueChanged -= UpdateCloth;
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
            HandleInput();
            HandleAnimation();
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
                rb.transform.localScale = new Vector3(Mathf.Abs(rb.transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
            }
            else
            {
                rb.transform.localScale = new Vector3(-Mathf.Abs(rb.transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
            }
            UpdateSpriteFlipClientRpc(isMovingRight);
        }
        SyncPlayerPositionClientRpc(rb.position);
    }

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
    /// Sets the players active animation
    /// </summary>
    /// <param name="animationState">The active animation</param>
    [ClientRpc]
    private void UpdateAnimationStateClientRpc(string animationState)
    {
        animator.SetCharacterState(animationState);
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
            rb.transform.localScale = new Vector3(Mathf.Abs(rb.transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
        }
        else
        {
            rb.transform.localScale = new Vector3(-Mathf.Abs(rb.transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
        }
    }
    #endregion

    #region ColorHandling
    /// <summary>
    /// Request server handling of a picked color
    /// </summary>
    /// <param name="color">The new color</param>
    [ServerRpc]
    private void RequestColorPickServerRpc(string color)
    {
        colorPick.Value = new FixedString32Bytes(color);
    }

    /// <summary>
    /// Updates the player colors when the networkvariable is changed
    /// </summary>
    /// <param name="past">What the previous color was</param>
    /// <param name="current">What the current color is</param>
    [ServerRpc]
    private void UpdateColorServerRpc(FixedString32Bytes past, FixedString32Bytes current)
    {
        colorChange.ColorChange(current.ToString());
        UpdateColorClientRpc(past, current);
    }

    /// <summary>
    /// Updates the player colors on the clientside, when called by the server
    /// </summary>
    /// <param name="past">What the previous color was</param>
    /// <param name="current">What the current color is</param>
    [ClientRpc]
    private void UpdateColorClientRpc(FixedString32Bytes past, FixedString32Bytes current)
    {
        if (MonsterColorOrClothingIsSet(current.ToString()))
        {
            colorChange.ColorChange(current.ToString());
        }
    }
    #endregion

    #region ClothHandling
    /// <summary>
    /// Requests setting the mid clothing item
    /// </summary>
    /// <param name="mid">Clothing item</param>
    [ServerRpc]
    private void RequestclothingMidPickServerRpc(string mid)
    {
        clothMid.Value = mid;
    }

    /// <summary>
    /// Requests setting the top clothing item
    /// </summary>
    /// <param name="top">Clothing item</param>
    [ServerRpc]
    private void RequestclothingTopPickServerRpc(string top)
    {
        clothTop.Value = top;
    }

    /// <summary>
    /// Updates the clothing item on self and for people when the value changes
    /// </summary>
    /// <param name="past">Previous clothing item</param>
    /// <param name="current">Current clothing item</param>
    private void UpdateCloth(FixedString32Bytes past, FixedString32Bytes current)
    {
        clothChange.ChangeClothes(current.ToString(), skeleton);
        UpdateClothClientRpc(past, current);
    }

    /// <summary>
    /// Updates the clothes client-side
    /// </summary>
    /// <param name="past">Former clothing</param>
    /// <param name="current">New clothing</param>
    [ClientRpc]
    private void UpdateClothClientRpc(FixedString32Bytes past, FixedString32Bytes current)
    {
        if (MonsterColorOrClothingIsSet(current.ToString()))
        {
            clothChange.ChangeClothes(current.ToString(), skeleton);
        }
    }
    #endregion

    #region CheckIfTrue
    /// <summary>
    /// Checks if the clothing or color exists
    /// </summary>
    /// <param name="CheckedItem">What is to be checked</param>
    /// <returns>Is the value not null or similar?</returns>
    private bool MonsterColorOrClothingIsSet(string CheckedItem)
    {
        if (CheckedItem is not "white" and not "" and not null)
            return true;
        return false;
    }
    #endregion
}