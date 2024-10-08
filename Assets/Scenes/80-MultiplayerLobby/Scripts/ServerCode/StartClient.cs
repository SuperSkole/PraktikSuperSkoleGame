using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Scenes.MultiplayerLobby.Scripts
{
    [RequireComponent(typeof(RelayManager))]
    [RequireComponent(typeof(LobbyManager))]
    [RequireComponent(typeof(StartHost))]
    [RequireComponent(typeof(NetworkManager))]
    [RequireComponent(typeof(NetworkTransport))]
    public class StartClient : MonoBehaviour
    {
        private RelayManager relayManager;
        private LobbyManager lobbyManager;
        private NetworkTransport transport;
        private NetworkManager networkManager;
        private StartHost host;
        public string serverID = null;
        private const float CharacterSpawnTimeout = 10f;
        public bool isCharacterSpawned = false;

        private void Start()
        {
            relayManager = GetComponent<RelayManager>();
            lobbyManager = GetComponent<LobbyManager>();
            transport = GetComponent<NetworkTransport>();
            networkManager = GetComponent<NetworkManager>();
            host = GetComponent<StartHost>();
            QuickJoinGame();
        }
        public async void QuickJoinGame()
        {
            try
            {
                if (!isCharacterSpawned)
                {
                    StartCoroutine(CheckCharacterSpawnTimeout());
                }
                // Query available lobbies
                var options = new QueryLobbiesOptions
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
                    Debug.Log($"Joining Lobby: {firstAvailableLobby.Name}");
                    serverID = firstAvailableLobby.Id;
                    Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(serverID);
                    Debug.Log($"Successfully joined lobby: {joinedLobby.Name}");
                    
                    // Retrieve the relay join code from the lobby data
                    if (joinedLobby.Data.ContainsKey("RelayCode"))
                    {
                        string relayJoinCode = joinedLobby.Data["RelayCode"].Value;

                        if (!string.IsNullOrEmpty(relayJoinCode))
                        {
                            // Join the relay using the retrieved join code

                            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);

                            RelayServerData relayServerData = new RelayServerData(joinAllocation, "wss");
                            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                            // Start the Netcode client
                            NetworkManager.Singleton.StartClient();

                            LobbyManager.lobbyId = joinedLobby.Id;
                        }
                        else
                        {
                            Debug.LogError("Relay join code is missing from the lobby.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Lobby does not contain relay join code.");
                    }
                }
                else if(queryResponse.Results != null)
                {
                    host.StartHostGame();
                }
                else
                {
                    Debug.LogWarning("Query returned null.");
                }
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"Quick join failed: {e}");
                SwitchScenes.SwitchToMainWorld();
            }
        }

        private IEnumerator CheckCharacterSpawnTimeout()
        {
            float timer = 0f;

            while (timer < CharacterSpawnTimeout)
            {
                if (isCharacterSpawned) // Implement this method to check if the character is spawned
                {
                    yield break; // If character is spawned, exit the coroutine
                }

                timer += Time.deltaTime;
                yield return null;
            }

            Debug.LogError("Character failed to spawn within the time limit. Returning to main world.");
            SwitchScenes.SwitchToMainWorld();
        }


    }
}
