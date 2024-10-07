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
                // Debug.Log("Sign-in successful. Player ID: " + AuthenticationService.Instance.PlayerId);
                // Debug.Log("Player ID: " + AuthenticationService.Instance.PlayerId);
            }
            catch (AuthenticationException ex)
            {
                Debug.Log("Sign-in failed!");
                Debug.LogException(ex);
            }
        }

        public async Task SignOutAsync()
        {
            AuthenticationService.Instance.SignOut();
        }
    }
}