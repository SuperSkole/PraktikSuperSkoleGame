using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Scenes._02_LoginScene.Scripts
{
    /// <summary>
    /// Service for username/password authentication
    /// </summary>
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
            Debug.Log("SignInWithUsernamePasswordAsync");
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
                Debug.Log($"Signed in: Username: {username} Password: {password}");
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
                Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }
        
        public async Task SignOutAsync()
        {
            AuthenticationService.Instance.SignOut();
        }


        public async Task<bool> SignUpWithUsernamePasswordAsync()
        {
            Debug.Log("SignUpWithUsernamePasswordAsync");
            try
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
                Debug.Log($"AuthenticationManager.SignUpWithUsernamePasswordAsync: Username: {username} Password: {password}");
                Debug.Log("Sign-up successful.");
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
                Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
                AuthenticationService.Instance.SignOut();
                return true;
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }

            return false;
        }
        
        
        
        /// <summary>
        /// Updates the user's password.
        /// </summary>
        public async Task UpdatePasswordAsync(string currentPassword, string newPassword)
        {
            try
            {
                await AuthenticationService.Instance.UpdatePasswordAsync(currentPassword, newPassword);
                Debug.Log("Password updated.");
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
