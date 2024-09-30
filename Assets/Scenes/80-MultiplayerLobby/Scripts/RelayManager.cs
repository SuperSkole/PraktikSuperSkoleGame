using Unity.Services.Relay;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    async void Start()
    {
        await InitializeUnityServices();
    }

    private async Task InitializeUnityServices()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async Task<string> CreateRelay(int maxPlayers)
    {
        try
        {
            var allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError($"Relay Error: {e}");
            return null;
        }
    }

    public async Task JoinRelay(string joinCode)
    {
        try
        {
            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch (RelayServiceException e)
        {
            Debug.LogError($"Relay Join Error: {e}");
        }
    }
}
