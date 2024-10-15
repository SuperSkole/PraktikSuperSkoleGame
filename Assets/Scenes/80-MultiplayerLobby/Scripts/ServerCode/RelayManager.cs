using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using UnityEngine;

namespace Scenes.MultiplayerLobby.Scripts
{
    public class RelayManager : MonoBehaviour
    {
        /// <summary>
        /// Initializes unity services.
        /// </summary>
        private async void Start()
        {
            await InitializeUnityServices();
        }

        /// <summary>
        /// Handles initializing unity services and authentication.
        /// </summary>
        /// <returns></returns>
        private async Task InitializeUnityServices()
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        /// <summary>
        /// Creates a relay set up to be able to run on a webgl build.
        /// </summary>
        public async Task<string> CreateRelay(int maxPlayers = 10)
        {
            try
            {
                Unity.Services.Relay.Models.Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                RelayServerData relayData = new(allocation, "wss");

                UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetRelayServerData(relayData);

                return joinCode;
            }
            catch (RelayServiceException e)
            {
                Debug.LogError($"Relay Error: {e}");
                return null;
            }
        }

        /// <summary>
        /// Attempts to join a relay. Currently unused.
        /// </summary>
        /// <param name="joinCode"></param>
        /// <returns></returns>
        public async Task JoinRelay(string joinCode)
        {
            try
            {
                Unity.Services.Relay.Models.JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            }
            catch (RelayServiceException e)
            {
                Debug.LogError($"Relay Join Error: {e}");
            }
        }
    }
}
