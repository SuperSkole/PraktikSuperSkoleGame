using Scenes;
using Scenes.MultiplayerLobby.Scripts;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using UnityEngine;

public class DisconnectHandler : NetworkBehaviour
{
    #region fields
    private bool exiting = false;
    [SerializeField]
    #endregion

    #region setup
    /// <summary>
    /// Sets the client to disconnect when receiving a disconnect callback.
    /// </summary>
    private void Start()
    {
        // Subscribe to the disconnection event
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
    }

    /// <summary>
    /// When destroyed, removes the client disconnect callback.
    /// </summary>
    public override void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
    }
    #endregion

    /// <summary>
    /// Ensures the exitcode is only called once at a time.
    /// </summary>
    public void PlayerDoorExit()
    {
        if (!exiting)
        {
            exiting = true;

            LeaveLobbyAndRelay();
        }
    }

    /// <summary>
    /// when the client is disconnected, check if they are the local client and if so, leave.
    /// </summary>
    /// <param name="clientId"></param>
    private void OnClientDisconnect(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            QuickLeave();
        }
    }

    /// <summary>
    /// Checks if the one leaving is the host or a player.
    /// </summary>
    public void LeaveLobbyAndRelay()
    {
        if (IsOwner)
        {
            if (IsHost)
                HostLeave();
            else
                PlayerLeave();
        }
        exiting = false;
    }

    /// <summary>
    /// Handles the player leaving properly.
    /// </summary>
    public async void PlayerLeave()
    {
        QuickLeave(true);
        string serverId = NetworkManager.Singleton.GetComponent<StartClient>().serverID;
        string id = AuthenticationService.Instance.PlayerId;
        if (!string.IsNullOrWhiteSpace(serverId))
            await LobbyService.Instance.RemovePlayerAsync(serverId, id);
        LeaveAllLobbies(id);
        CleanupNetworkManager();
    }

    /// <summary>
    /// Handles the host leaving.
    /// </summary>
    public void HostLeave()
    {
        string serverId = NetworkManager.Singleton.GetComponent<StartClient>().serverID;
        LeaveLevel();
        CleanupNetworkManager(true, true, serverId);
    }

    /// <summary>
    /// Handles the player leaving by throwing them to a new scene and destroying the networkmanager if it exists.
    /// </summary>
    private void QuickLeave(bool destroysManagerSelf = false)
    {
        if (IsOwner && !IsHost)
        {
            LeaveLevel();
            if (NetworkManager.Singleton != null && destroysManagerSelf == false)
                Destroy(NetworkManager.Singleton.gameObject);
        }
    }

    /// <summary>
    /// Sends the player to the mainworld.
    /// </summary>
    public void LeaveLevel()
    {
        SwitchScenes.SwitchToMainWorld();
    }

    /// <summary>
    /// Attempts to leave all lobbies.
    /// </summary>
    /// <param name="id"></param>
    private async void LeaveAllLobbies(string id)
    {
        try
        {
            List<string> lobbies = await LobbyService.Instance.GetJoinedLobbiesAsync();
            if (lobbies.Count > 0)
                foreach (string lobby in lobbies)
                    if (!string.IsNullOrWhiteSpace(lobby))
                        await LobbyService.Instance.RemovePlayerAsync(lobby, id);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError($"Lobby service error: {ex.Message}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error leaving lobby or relay: {ex.Message}");
        }
    }

    /// <summary>
    /// Cleans up the network by shutting down the server, checking there is a server and then deleting it and lastly destroying the networkmanager.
    /// </summary>
    private async void CleanupNetworkManager(bool shouldDestroy = true, bool shouldShutDown = false, string serverId = null)
    {
        if (NetworkManager.Singleton != null)
        {
            if (shouldShutDown)
                NetworkManager.Singleton.Shutdown();
            if (string.IsNullOrWhiteSpace(serverId))
                await LobbyService.Instance.DeleteLobbyAsync(serverId);
            if (shouldDestroy)
                Destroy(NetworkManager.Singleton.gameObject);
        }
    }
}
