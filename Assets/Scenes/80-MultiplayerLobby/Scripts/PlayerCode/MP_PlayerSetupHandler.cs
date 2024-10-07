using Cinemachine;
using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Spine.Unity;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MP_PlayerSetupHandler : NetworkBehaviour
{
    #region Fields
    private ColorChanging colorChange;
    private ClothChanging clothChange;
    private GameObject textDisplay;
    private TextMeshProUGUI textField;

    private ISkeletonComponent skeleton;
    MP_PlayerData MPData;
    CinemachineVirtualCamera playerCamera;

    public NetworkVariable<FixedString32Bytes> colorPick = new();
    public NetworkVariable<FixedString32Bytes> clothMid = new();
    public NetworkVariable<FixedString32Bytes> clothTop = new();
    public NetworkVariable<FixedString32Bytes> text = new();

    MP_MovementHandler movementHandler;
    #endregion

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
                playerCamera.Follow = this.gameObject.transform;
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
    }

    public override void OnNetworkDespawn()
    {
        colorPick.OnValueChanged -= UpdateColorServerRpc;
        clothMid.OnValueChanged -= UpdateCloth;
        clothTop.OnValueChanged -= UpdateCloth;
        text.OnValueChanged -= UpdateNameServerRpc;
    }

    private void GetComponents()
    {
        MPData = GetComponent<MP_PlayerData>();
        skeleton = GetComponentInChildren<ISkeletonComponent>();
        clothChange = GetComponent<ClothChanging>();
        colorChange = GetComponent<ColorChanging>();
        textField = GetComponentInChildren<TextMeshProUGUI>();
        textDisplay = this.gameObject.transform.Find("Canvas").Find("NameDisplay").GameObject();
        movementHandler = GetComponent<MP_MovementHandler>();
    }

    private void SetupListener()
    {
        colorPick.OnValueChanged += UpdateColorServerRpc;
        clothMid.OnValueChanged += UpdateCloth;
        clothTop.OnValueChanged += UpdateCloth;
        text.OnValueChanged += UpdateNameServerRpc;
    }

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
        if (StringIsSetCheck(current.ToString()))
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
        if (StringIsSetCheck(current.ToString()))
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
        if (StringIsSetCheck(current.ToString()))
        {
            textField.text = current.ToString();
        }
    }
    #endregion

    private void lastSetup()
    {
        if (StringIsSetCheck(colorPick.Value.ToString()))
            colorChange.ColorChange(colorPick.Value.ToString());
        if (StringIsSetCheck(clothMid.Value.ToString()))
            clothChange.ChangeClothes(clothMid.Value.ToString(), skeleton);
        if (StringIsSetCheck(clothTop.Value.ToString()))
            clothChange.ChangeClothes(clothTop.Value.ToString(), skeleton);
        if (StringIsSetCheck(text.Value.ToString()))
            textField.text = text.Value.ToString();
        Debug.Log("0");
        if (movementHandler.textRotation.Value != new Vector3())
        {
            Debug.Log("1: " + textDisplay.transform);
            Debug.Log("2: " + movementHandler.textRotation.Value);
            textDisplay.transform.localScale = movementHandler.textRotation.Value;
            Debug.Log("3");
        }
    }

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
}
