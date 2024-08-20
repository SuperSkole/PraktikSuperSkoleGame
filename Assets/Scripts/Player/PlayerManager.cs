using System.Collections;
using System.Collections.Generic;
using LoadSave;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerData playerData;

    public void InitializePlayerData(PlayerData data)
    {
        playerData = data;

        // Init other player settings or components
    }
}