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
            await InitializeUnityServices();

#if UNITY_EDITOR
            if (useAnonymousLogin)
            {
                await SignInAnonymouslyAsync();
            }
            else
            {
                await SignInWithUsernamePasswordAsync("testuser", "testpassword");
            }
#else
            // In build, always use username/password login
            await SignInWithUsernamePasswordAsync("testuser", "testpassword");
#endif
        }

        /// <summary>
        /// Initializes the Unity services.
        /// </summary>
        private async Task InitializeUnityServices()
        {
            try
            {
                await UnityServices.InitializeAsync();
                Debug.Log("Unity services initialized.");
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
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
        public async Task SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign-in successful. Player ID: " + AuthenticationService.Instance.PlayerId);
                Debug.Log("Player ID: " + AuthenticationService.Instance.PlayerId); 
            }
            catch (AuthenticationException ex)
            {
                Debug.Log("Sign-in failed!");
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// Signs up the user using a username and password.
        /// </summary>
        public async Task SignUpWithUsernamePasswordAsync(string username, string password)
        {
            try
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
                Debug.Log("Sign-up successful.");
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
        public async Task SignInWithUsernamePasswordAsync(string username, string password)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
                Debug.Log("Sign-in successful.");
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


