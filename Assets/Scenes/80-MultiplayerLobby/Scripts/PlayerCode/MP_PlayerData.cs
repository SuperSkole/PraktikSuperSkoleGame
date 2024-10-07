using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

public class MP_PlayerData : NetworkBehaviour
{
    public NetworkVariable<FixedString128Bytes> playerId = new();

    public void SetupId()
    {
        if(IsOwner)
            setIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    [ServerRpc(RequireOwnership = false)]
    void setIdServerRpc(string id)
    {
        playerId.Value = id;
    }
}
