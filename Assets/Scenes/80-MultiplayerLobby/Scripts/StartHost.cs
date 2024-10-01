using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Scenes.MultiplayerLobby.Scripts
{
    public class StartHost : MonoBehaviour
    {
        RelayManager relayManager;
        LobbyManager lobbyManager;
        string lobbyName = $@"Host Lobby";
        private void Start()
        {
            relayManager = GetComponent<RelayManager>();
            lobbyManager = GetComponent<LobbyManager>();
        }
        public async void StartHostGame()
        {
            string joinCode = await relayManager.CreateRelay();
            NetworkManager.Singleton.StartHost();
            await lobbyManager.CreateLobby(relayCode: joinCode);
            var lobby = await LobbyService.Instance.GetJoinedLobbiesAsync();
            LobbyManager.lobbyId = lobby[0];
        }
    }
}
