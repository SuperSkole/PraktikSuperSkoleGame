using Scenes;
using Scenes.MultiplayerLobby.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using UnityEngine;

public class ConnectionHandler : MonoBehaviour
{
    private LobbyManager lobbyManager;
    private RelayManager relayManager;
    private NetworkManager manager;

    private void Start()
    {
        // Get references to the Lobby and Relay managers
        lobbyManager = GetComponent<LobbyManager>();
        relayManager = GetComponent<RelayManager>();

        // Subscribe to the disconnection event
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
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
            LeaveLobbyAndRelay();

            // Call the custom function to switch the world
            
        }
    }

    public void LeaveLevel()
    {
        SwitchScenes.SwitchToPlayerHouseScene();
    }

    // Leave the Lobby and Relay (cleanup)
    public async void LeaveLobbyAndRelay()
    {
        if(NetworkManager.Singleton.IsHost)
            NetworkManager.Singleton.Shutdown();
        try
        {
            Debug.Log("Leaving lobby...");
            List<string> lobbies = await LobbyService.Instance.GetJoinedLobbiesAsync();
            Debug.Log("1");
            string playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log("2");
            foreach (string lobby in lobbies)
            {
                Debug.Log("3");
                await LobbyService.Instance.RemovePlayerAsync(lobby, playerId);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error leaving lobby or relay: {ex.Message}");
        }
    }
}
