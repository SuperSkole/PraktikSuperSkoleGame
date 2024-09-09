using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

namespace Scenes._02_LoginScene.Scripts
{
    // Service for anonymous authentication
    public class AnonymousAuthenticationService : IAuthenticationService
    {
        public async Task SignInAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Anonymous Sign-in successful");
                Debug.Log("Player Id:" + AuthenticationService.Instance.PlayerId);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Anonymous Sign-in failed: {ex.Message}");
            }
        }

        public async Task SignOutAsync()
        {
            // TODO Implement sign out 
        }
    }
}