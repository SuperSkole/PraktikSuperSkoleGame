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
    private bool activateDebugMessages = false;
    #endregion

    #region setup
    private void Start()
    {
        // Subscribe to the disconnection event
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
    }

    public override void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if(NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
    }
    #endregion
    public void PlayerDoorExit()
    {
        if (!exiting)
        {
            exiting = true;

            LeaveLobbyAndRelay();
        }
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("Disconnected from the host.");

            QuickLeave();
        }
    }

    public void LeaveLobbyAndRelay()
    {
        if (IsOwner)
        {
            if (IsHost)
                HostLeave();
            else
                PlayerLeave();
        }
    }

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

    public void HostLeave()
    {
        string serverId = NetworkManager.Singleton.GetComponent<StartClient>().serverID;
        LeaveLevel();
        CleanupNetworkManager(true, true, serverId);
    }

    private void QuickLeave(bool destroysManagerSelf = false)
    {
        if (IsOwner && !IsHost)
        {
            LeaveLevel();
            if (NetworkManager.Singleton != null && destroysManagerSelf == false)
                Destroy(NetworkManager.Singleton.gameObject);
        }
    }

    public void LeaveLevel()
    {
        SwitchScenes.SwitchToMainWorld();
    }

    private async void LeaveAllLobbies(string id)
    {
        try
        {
            List<string> lobbies = await LobbyService.Instance.GetJoinedLobbiesAsync();
            if (lobbies.Count > 0)
                foreach (string lobby in lobbies)
                    if(!string.IsNullOrWhiteSpace(lobby))
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
