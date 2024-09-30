using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
            await lobbyManager.CreateLobby(relayCode: joinCode);
            
            //string currentLobbyId = lobbyManager.GetInstanceID().ToString();
            //await lobbyManager.UpdateLobbyRelayCode(currentLobbyId, joinCode);
            NetworkManager.Singleton.StartHost();
        }

    }
}
