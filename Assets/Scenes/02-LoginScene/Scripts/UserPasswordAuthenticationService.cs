using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

namespace Scenes._02_LoginScene.Scripts
{
    // Service for username/password authentication 
    public class UserPasswordAuthenticationService : IAuthenticationService
    {
        private string username;
        private string password;

        public UserPasswordAuthenticationService(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public async Task SignInAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
                Debug.Log("User Sign-in successful");
                Debug.Log("Player Id:" + AuthenticationService.Instance.PlayerId);
                Debug.Log("Username:" + AuthenticationService.Instance.GetPlayerNameAsync());
                //GameManager.Instance.CurrentUser = username;
            }
            catch (Exception ex)
            {
                Debug.LogError($"User Sign-in failed: {ex.Message}");
            }
        }


        public async Task SignOutAsync()
        {
            // TODO Implement sign out 
        }
    }
}
