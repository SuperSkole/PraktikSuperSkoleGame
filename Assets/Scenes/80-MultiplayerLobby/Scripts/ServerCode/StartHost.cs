using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;

namespace Scenes.MultiplayerLobby.Scripts
{
    public class StartHost : MonoBehaviour
    {
        private RelayManager relayManager;
        private LobbyManager lobbyManager;

        /// <summary>
        /// Fetches the relevant components.
        /// </summary>
        private void Start()
        {
            relayManager = GetComponent<RelayManager>();
            lobbyManager = GetComponent<LobbyManager>();
        }

        /// <summary>
        /// Starts hosting the game.
        /// </summary>
        public async void StartHostGame()
        {
            string joinCode = await relayManager.CreateRelay();
            NetworkManager.Singleton.StartHost();
            await lobbyManager.CreateLobby(relayCode: joinCode);
            List<string> lobby = await LobbyService.Instance.GetJoinedLobbiesAsync();
            LobbyManager.lobbyId = lobby[0];
            GetComponent<StartClient>().serverID = lobby[0];
        }
    }
}
