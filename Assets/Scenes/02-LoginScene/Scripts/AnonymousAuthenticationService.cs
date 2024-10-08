using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

namespace Scenes._02_LoginScene.Scripts
{
    /// <summary>
    /// Service for anonymous authentication
    /// </summary>
    public class AnonymousAuthenticationService : IAuthenticationService
    {
        private const int MaxRetries = 3;

        /// <summary>
        /// Attempts to sign in anonymously, with retry logic in case of failure.
        /// </summary>
        public async Task SignInAsync()
        {
            int attempt = 0;
            bool success = false;

            while (attempt < MaxRetries && !success)
            {
                try
                {
                    attempt++;
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    success = true; // Break loop if successful
                    Debug.Log("Sign-in successful on attempt: " + attempt);
                }
                catch (AuthenticationException ex)
                {
                    Debug.LogError($"Sign-in attempt {attempt} failed.");
                    Debug.LogException(ex);

                    if (attempt >= MaxRetries)
                    {
                        // If all retries fail, report the failure
                        Debug.LogError("All sign-in attempts failed.");
                        throw;
                    }

                    // Wait 1 second before retrying
                    await Task.Delay(1000); 
                }
            }
        }

        /// <summary>
        /// Attempts to sign out the authenticated user.
        /// </summary>
        /// <returns>A task representing the asynchronous sign-out operation.</returns>
        public async Task SignOutAsync()
        {
            AuthenticationService.Instance.SignOut();
        }
    }
}