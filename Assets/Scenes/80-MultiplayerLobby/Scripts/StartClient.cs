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
        private RelayManager relayManager;
        private LobbyManager lobbyManager;
        private NetworkTransport transport;
        private NetworkManager networkManager;

        private void Start()
        {
            relayManager = GetComponent<RelayManager>();
            lobbyManager = GetComponent<LobbyManager>();
            transport = GetComponent<NetworkTransport>();
            networkManager = GetComponent<NetworkManager>();
        }
        public async void QuickJoinGame()
        {
            //string joinCode = await relayManager.CreateRelay();

            try
            {
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
                Debug.Log("test 1");
                if (queryResponse.Results != null && queryResponse.Results.Count > 0)
                {
                    Debug.Log("test 2");
                    // Join the first available lobby
                    Lobby firstAvailableLobby = queryResponse.Results[0];
                    Debug.Log($"Joining Lobby: {firstAvailableLobby.Name}");

                    Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(firstAvailableLobby.Id);
                    Debug.Log($"Successfully joined lobby: {joinedLobby.Name}");
                    
                    // Retrieve the relay join code from the lobby data
                    if (joinedLobby.Data.ContainsKey("RelayCode"))
                    {
                        string relayJoinCode = joinedLobby.Data["RelayCode"].Value;

                        if (!string.IsNullOrEmpty(relayJoinCode))
                        {
                            // Join the relay using the retrieved join code
                            //await relayManager.JoinRelay(relayJoinCode);
                            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);
                            //Allocation allocation = await RelayService.Instance.CreateAllocationAsync(7);
                            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
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
                else
                {
                    Debug.LogWarning("No available lobbies found. Lobby result: " + queryResponse.Results);
                    if (queryResponse.Results != null)
                    {
                        Debug.LogWarning(queryResponse.Results.Count);
                    }
                    else
                        Debug.LogWarning("No queryResponse found");
                }
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError($"Quick join failed: {e}");
            }
        }


    }
}
