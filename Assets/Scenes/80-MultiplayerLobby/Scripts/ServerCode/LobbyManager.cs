using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

namespace Scenes.MultiplayerLobby.Scripts
{
    public class LobbyManager : MonoBehaviour
    {
        static public string lobbyId;
        public async Task CreateLobby(string lobbyName = "Lobby Manager", int maxPlayers = 10, string relayCode = "")
        {
            try
            {
                var options = new CreateLobbyOptions();
                options.IsPrivate = false;
                options.Data = new Dictionary<string, DataObject>
                {
                    { "RelayCode", new DataObject(DataObject.VisibilityOptions.Public, relayCode, DataObject.IndexOptions.S1) }
                };

                Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
                Debug.Log($"Created Lobby: {lobby.Name}, LobbyCode: {lobby.LobbyCode}");
                StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"Lobby Error: {e}");
            }
        }

        public async Task JoinLobby(string lobbyCode)
        {
            try
            {
                Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);
                Debug.Log($"Joined Lobby: {lobby.Name}");
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"Join Lobby Error: {e}");
            }
        }

        public async Task UpdateLobbyRelayCode(string lobbyId, string relayCode)
        {
            try
            {
                var data = new Dictionary<string, DataObject>
                {
                    { "RelayCode", new DataObject(DataObject.VisibilityOptions.Public, relayCode, DataObject.IndexOptions.S1) }
                };

                await Lobbies.Instance.UpdateLobbyAsync(lobbyId, new UpdateLobbyOptions { Data = data });
                Debug.Log("Lobby updated with relay code.");
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"Update Lobby Error: {e}");
            }
        }
        IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
        {
            var delay = new WaitForSecondsRealtime(waitTimeSeconds);

            while (true)
            {
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
                yield return delay;
            }
        }
    }
}

