using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Scenes.MultiplayerLobby.Scripts
{
    public class LobbyManager : MonoBehaviour
    {
        public static string lobbyId;

        /// <summary>
        /// Creates a multiplayer lobby.
        /// </summary>
        /// <returns></returns>
        public async Task CreateLobby(string relayCode, string lobbyName = "Lobby Manager", int maxPlayers = 10)
        {
            try
            {
                CreateLobbyOptions options = new();
                options.IsPrivate = false;
                options.Data = new Dictionary<string, DataObject>
                {
                    { "RelayCode", new DataObject(DataObject.VisibilityOptions.Public, relayCode, DataObject.IndexOptions.S1) }
                };

                Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
                StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"Lobby Error: {e}");
            }
        }

        /// <summary>
        /// Attempts to join a lobby.
        /// </summary>
        public async Task JoinLobby(string lobbyCode)
        {
            try
            {
                Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"Join Lobby Error: {e}");
            }
        }

        // TODO: Implement the UpdateLobbyRelayCode function fully.
        /// <summary>
        /// Attempts to update the lobby relay code.
        /// </summary>
        public async Task UpdateLobbyRelayCode(string lobbyId, string relayCode)
        {
            try
            {
                Dictionary<string, DataObject> data = new()
                {
                    { "RelayCode", new DataObject(DataObject.VisibilityOptions.Public, relayCode, DataObject.IndexOptions.S1) }
                };

                await Lobbies.Instance.UpdateLobbyAsync(lobbyId, new UpdateLobbyOptions { Data = data });
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"Update Lobby Error: {e}");
            }
        }

        /// <summary>
        /// Initiates the heartbeat routine that keeps a server alive.
        /// </summary>
        private IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
        {
            WaitForSecondsRealtime delay = new(waitTimeSeconds);

            while (true)
            {
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
                yield return delay;
            }
        }
    }
}

