using Scenes;
using Scenes.MultiplayerLobby.Scripts;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using UnityEngine;

public class ConnectionHandler : NetworkBehaviour
{
    private LobbyManager lobbyManager;
    private RelayManager relayManager;
    private readonly NetworkManager manager;
    private bool exiting = false;

    private void Start()
    {
        // Get references to the Lobby and Relay managers
        lobbyManager = GetComponent<LobbyManager>();
        relayManager = GetComponent<RelayManager>();

        // Subscribe to the disconnection event
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
    }

    public override void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
        }
    }

    public void playerDoorExit(GameObject player)
    {
        if (!exiting)
        {
            exiting = true;

            MP_PlayerData playerIdHolder = player.GetComponent<MP_PlayerData>();

            LeaveLobbyAndRelay(playerIdHolder.playerId.Value.ToString(), player);
        }
    }

    // This function is called when the client is disconnected from the server
    private void OnClientDisconnect(ulong clientId)
    {
        // If this client is disconnected from the host
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("Disconnected from the host.");

            // Leave the Lobby
            SwitchScenes.SwitchToMainWorld();
        }
    }

    public void LeaveLevel()
    {
        SwitchScenes.SwitchToMainWorld();
    }

    // Leave the Lobby and Relay (cleanup)
    public async void LeaveLobbyAndRelay(string playerId, GameObject player)
    {
        try
        {
            if (IsOwner && !IsHost)
            {
                LeaveLevel();
                string serverId = NetworkManager.Singleton.GetComponent<Scenes.MultiplayerLobby.Scripts.StartClient>().serverID;
                string id = AuthenticationService.Instance.PlayerId;
                await LobbyService.Instance.RemovePlayerAsync(serverId, id);
                Destroy(NetworkManager.Singleton.gameObject);
                //NetworkManager.Singleton.Shutdown();
            }
            else if (IsOwner && IsHost)
            {
                //List<string> lobbies = await LobbyService.Instance.GetJoinedLobbiesAsync();
                string serverId = NetworkManager.Singleton.GetComponent<Scenes.MultiplayerLobby.Scripts.StartClient>().serverID;
                //await LobbyService.Instance.RemovePlayerAsync(serverId, playerId);
                LeaveLevel();
                NetworkManager.Singleton.Shutdown();
                await LobbyService.Instance.DeleteLobbyAsync(serverId);
                Destroy(NetworkManager.Singleton.gameObject);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error leaving lobby or relay: {ex.Message}");
        }           
    }
}
