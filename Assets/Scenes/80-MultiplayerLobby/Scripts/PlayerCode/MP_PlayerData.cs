using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Authentication;

public class MP_PlayerData : NetworkBehaviour
{
    public NetworkVariable<FixedString128Bytes> playerId = new();

    /// <summary>
    /// Saves the player's id to easily fetch it later.
    /// </summary>
    public void SetupId()
    {
        if (IsOwner)
            setIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    /// <summary>
    /// Sets the id on the server.
    /// </summary>
    /// <param name="id"></param>
    [ServerRpc(RequireOwnership = false)]
    private void setIdServerRpc(string id)
    {
        playerId.Value = id;
    }
}
