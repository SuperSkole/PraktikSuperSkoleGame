using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Scenes._02_LoginScene.Scripts
{
    /// <summary>
    /// Manages the authentication of the player in the Unity game.
    /// </summary>
    public class AuthenticationManager : MonoBehaviour
    {
        private bool useAnonymousLogin = false; // Default to false in builds
        public bool IsSignedIn { get; private set; } = false;

        /// <summary>
        /// Sets whether anonymous login should be used. Only valid in the editor.
        /// </summary>
        public void SetUseAnonymousLogin(bool isAnonLogin)
        {
            useAnonymousLogin = isAnonLogin;
        }
    
        /// <summary>
        /// Initializes Unity services on start and signs the user in anonymously.
        /// </summary>
        private async void Start()
        {
            

#if UNITY_EDITOR
            if (useAnonymousLogin)
            {
                await SignInAnonymouslyAsync();
            }
            else
            {
               // await SignInWithUsernamePasswordAsync(, );
            }
#else
            // In build, always use username/password login
            await SignInWithUsernamePasswordAsync("testuser", "testpassword");
#endif
        }

        
        

        /// <summary>
        /// Signs in the user anonymously.
        /// </summary>
        public async Task SignInAsync()
        {
            await SignInAnonymouslyAsync();
        }

        /// <summary>
        /// Signs in the user anonymously using Unity Authentication service.
        /// </summary>
        public async Task<bool> SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign-in successful. Player ID: " + AuthenticationService.Instance.PlayerId);
                Debug.Log("Player ID: " + AuthenticationService.Instance.PlayerId);
                return true;
            }
            catch (AuthenticationException ex)
            {
                Debug.Log("Sign-in failed!");
                Debug.LogException(ex);
            }
            return false;
        }

        /// <summary>
        /// Signs up the user using a username and password.
        /// </summary>
        public async Task SignUpWithUsernamePasswordAsync(string username, string password)
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

        /// <summary>
        /// Signs in the user using a username and password.
        /// </summary>
        public async Task<bool> SignInWithUsernamePasswordAsync(string username, string password)
        {
            Debug.Log("SignInWithUsernamePasswordAsync");
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
                Debug.Log($"Signed in: Username: {username} Password: {password}");
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
                Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
                IsSignedIn = true;
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
            
            IsSignedIn = false;
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


