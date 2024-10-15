using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Scenes.MultiplayerLobby.Scripts
{
    public class StartClient : MonoBehaviour
    {
        private StartHost host;
        public string serverID = null;
        private const float CharacterSpawnTimeout = 10f;
        public bool isCharacterSpawned = false;

        /// <summary>
        /// Gets the host and begins booting multiplayer.
        /// </summary>
        private void Start()
        {
            host = GetComponent<StartHost>();
            QuickJoinGame();
        }

        /// <summary>
        /// Calls the spawn timeout and begins filtering to fetch the possible servers.
        /// Afterwards, attempts to join the server.
        /// </summary>
        public async void QuickJoinGame()
        {
            try
            {
                if (!isCharacterSpawned)
                {
                    StartCoroutine(CheckCharacterSpawnTimeout());
                }
                // Query available lobbies
                QueryLobbiesOptions options = new()
                {
                    Filters = new List<QueryFilter>
                    {
                        // Add filters if needed, such as to only join public lobbies, not full lobbies, etc.
                        new(QueryFilter.FieldOptions.AvailableSlots, "1", QueryFilter.OpOptions.GT)
                    }
                };

                // Get a list of available lobbies
                QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(options);
                if (queryResponse.Results != null && queryResponse.Results.Count > 0)
                {
                    // Join the first available lobby
                    Lobby firstAvailableLobby = queryResponse.Results[0];
                    serverID = firstAvailableLobby.Id;
                    Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(serverID);

                    // Retrieve the relay join code from the lobby data
                    if (joinedLobby.Data.ContainsKey("RelayCode"))
                    {
                        string relayJoinCode = joinedLobby.Data["RelayCode"].Value;

                        if (!string.IsNullOrEmpty(relayJoinCode))
                        {
                            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);

                            RelayServerData relayServerData = new(joinAllocation, "wss");
                            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                            // Start the Netcode client
                            NetworkManager.Singleton.StartClient();

                            LobbyManager.lobbyId = joinedLobby.Id;
                        }
                        else
                        {
                            Debug.LogError("Relay join code is missing from the lobby.");
                            SwitchScenes.SwitchToMainWorld();
                        }
                    }
                    else
                    {
                        Debug.LogError("Lobby does not contain relay join code.");
                        SwitchScenes.SwitchToMainWorld();
                    }
                }
                else if (queryResponse.Results != null)
                {
                    host.StartHostGame();
                }
                else
                {
                    Debug.LogWarning("Query returned null.");
                    SwitchScenes.SwitchToMainWorld();
                }
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"Quick join failed: {e}");
                SwitchScenes.SwitchToMainWorld();
            }
        }

        /// <summary>
        /// Waits a time to check if the MP character has spawned.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckCharacterSpawnTimeout()
        {
            float timer = 0f;

            while (timer < CharacterSpawnTimeout)
            {
                if (isCharacterSpawned)
                {
                    yield break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            Debug.LogError("Character failed to spawn within the time limit. Returning to main world.");
            SwitchScenes.SwitchToMainWorld();
        }


    }
}
