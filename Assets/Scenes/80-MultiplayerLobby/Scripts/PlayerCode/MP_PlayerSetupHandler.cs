using Cinemachine;
using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class MP_PlayerSetupHandler : NetworkBehaviour
{
    #region Fields
    private ColorChanging colorChange;
    private ClothChanging clothChange;
    private GameObject textDisplay;
    private TextMeshProUGUI textField;

    private ISkeletonComponent skeleton;
    private CinemachineVirtualCamera playerCamera;

    public NetworkVariable<FixedString32Bytes> colorPick = new();
    public NetworkVariable<FixedString32Bytes> clothMid = new();
    public NetworkVariable<FixedString32Bytes> clothTop = new();
    public NetworkVariable<FixedString32Bytes> text = new();
    private MP_MovementHandler movementHandler;
    #endregion

    #region setup
    /// <summary>
    /// Sets up the MP character to be a copy of the SP character.
    /// </summary>
    public void SetupCharacter()
    {
        GetComponents();

        SetupListener();

        colorChange.SetSkeleton(skeleton);

        playerCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();

        if (IsClient)
        {
            if (IsOwner)
            {
                playerCamera.Follow = gameObject.transform;
                GameObject originPlayer = GameObject.Find("PlayerMonster");
                string monsterColor = originPlayer.GetComponent<PlayerData>().MonsterColor;
                RequestColorPickServerRpc(monsterColor);
                string clothMid = originPlayer.GetComponent<PlayerData>().ClothMid;
                string clothTop = originPlayer.GetComponent<PlayerData>().ClothTop;
                RequestclothingMidPickServerRpc(clothMid);
                RequestclothingTopPickServerRpc(clothTop);
                textField.text = originPlayer.GetComponent<PlayerData>().MonsterName;
                RequestNamePickServerRpc(textField.text);

                GetComponent<MP_PlayerData>().SetupId();
            }
        }
        lastSetup();
        NetworkManager.Singleton.GetComponent<Scenes.MultiplayerLobby.Scripts.StartClient>().isCharacterSpawned = true;
    }

    /// <summary>
    /// Removes several calls when despawning.
    /// </summary>
    public override void OnNetworkDespawn()
    {
        colorPick.OnValueChanged -= UpdateColorServerRpc;
        clothMid.OnValueChanged -= UpdateCloth;
        clothTop.OnValueChanged -= UpdateCloth;
        text.OnValueChanged -= UpdateNameServerRpc;
    }

    /// <summary>
    /// Fetches relevant components.
    /// </summary>
    private void GetComponents()
    {
        skeleton = GetComponentInChildren<ISkeletonComponent>();
        clothChange = GetComponent<ClothChanging>();
        colorChange = GetComponent<ColorChanging>();
        textField = GetComponentInChildren<TextMeshProUGUI>();
        textDisplay = gameObject.transform.Find("Canvas").Find("NameDisplay").GameObject();
        movementHandler = GetComponent<MP_MovementHandler>();
    }

    /// <summary>
    /// Sets up listeners to ensure everyone gets updated properly.
    /// </summary>
    private void SetupListener()
    {
        colorPick.OnValueChanged += UpdateColorServerRpc;
        clothMid.OnValueChanged += UpdateCloth;
        clothTop.OnValueChanged += UpdateCloth;
        text.OnValueChanged += UpdateNameServerRpc;
    }
    #endregion

    #region ColorHandling
    /// <summary>
    /// Request server handling of a picked color
    /// </summary>
    /// <param name="color">The new color</param>
    [ServerRpc(RequireOwnership = false)]
    private void RequestColorPickServerRpc(string color)
    {
        colorPick.Value = new FixedString32Bytes(color);
    }

    /// <summary>
    /// Updates the player colors when the networkvariable is changed
    /// </summary>
    /// <param name="past">What the previous color was</param>
    /// <param name="current">What the current color is</param>
    [ServerRpc(RequireOwnership = false)]
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
        if (string.IsNullOrWhiteSpace(current.ToString()))
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
        if (string.IsNullOrWhiteSpace(current.ToString()))
        {
            clothChange.ChangeClothes(current.ToString(), skeleton);
        }
    }
    #endregion

    #region NameHandling
    /// <summary>
    /// Request server handling of a player name
    /// </summary>
    /// <param name="name">The new color</param>
    [ServerRpc(RequireOwnership = false)]
    private void RequestNamePickServerRpc(string name)
    {
        text.Value = name;
    }

    /// <summary>
    /// Updates the player name when the networkvariable is changed
    /// </summary>
    /// <param name="past">What the previous name was</param>
    /// <param name="current">What the current name is</param>
    [ServerRpc(RequireOwnership = false)]
    private void UpdateNameServerRpc(FixedString32Bytes past, FixedString32Bytes current)
    {
        textField.text = current.ToString();
        UpdateNameClientRpc(past, current);
    }

    /// <summary>
    /// Updates the player names on the clientside, when called by the server
    /// </summary>
    /// <param name="past">What the previous name was</param>
    /// <param name="current">What the current name is</param>
    [ClientRpc]
    private void UpdateNameClientRpc(FixedString32Bytes past, FixedString32Bytes current)
    {
        if (string.IsNullOrWhiteSpace(current.ToString()))
        {
            textField.text = current.ToString();
        }
    }
    #endregion

    /// <summary>
    /// The last bits of setup to handle, mainly to set up the visuals properly.
    /// </summary>
    private void lastSetup()
    {
        if (string.IsNullOrWhiteSpace(colorPick.Value.ToString()))
            colorChange.ColorChange(colorPick.Value.ToString());
        if (string.IsNullOrWhiteSpace(clothMid.Value.ToString()))
            clothChange.ChangeClothes(clothMid.Value.ToString(), skeleton);
        if (string.IsNullOrWhiteSpace(clothTop.Value.ToString()))
            clothChange.ChangeClothes(clothTop.Value.ToString(), skeleton);
        if (string.IsNullOrWhiteSpace(text.Value.ToString()))
            textField.text = text.Value.ToString();
        if (movementHandler.textRotation.Value != new Vector3())
            textDisplay.transform.localScale = movementHandler.textRotation.Value;
    }
}
