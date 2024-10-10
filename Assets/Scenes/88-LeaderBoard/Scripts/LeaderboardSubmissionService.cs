using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;
using UnityEngine;

namespace Scenes._88_LeaderBoard.Scripts
{
    public class LeaderboardSubmissionService : ILeaderboardSubmissionService
    {
        private const string LEADERBOARD_ID_WORDS = "Most_Words_Leaderboard";
        private const string LEADERBOARD_ID_LETTERS = "Most_Letters_Leaderboard";
        private const int MaxRetries = 3;

        /// <summary>
        /// Submits the player's total word count to the leaderboard with retry logic.
        /// </summary>
        public async Task SubmitMostWords(int wordCount, string monsterName)
        {
            await SubmitWithRetry(async () => 
            {
                Debug.Log($"Submitting {wordCount} words to leaderboard...");
                var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(
                    LEADERBOARD_ID_WORDS,
                    wordCount,
                    new AddPlayerScoreOptions
                    {
                        Metadata = new Dictionary<string, string>()
                        {
                            { "Monster", monsterName }
                        }
                    });

                Debug.Log("Most Words submitted successfully.");
            }, "Most Words");
        }

        /// <summary>
        /// Submits the player's total letter count to the leaderboard with retry logic.
        /// </summary>
        public async Task SubmitMostLetters(int letterCount, string monsterName)
        {
            await SubmitWithRetry(async () => 
            {
                Debug.Log($"Submitting {letterCount} letters to leaderboard...");
                var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(
                    LEADERBOARD_ID_LETTERS,
                    letterCount,
                    new AddPlayerScoreOptions
                    {
                        Metadata = new Dictionary<string, string>()
                        {
                            { "Monster", monsterName }
                        }
                    });

                Debug.Log("Most Letters submitted successfully.");
            }, "Most Letters");
        }

        /// <summary>
        /// Helper method to handle retry logic.
        /// </summary>
        private async Task SubmitWithRetry(Func<Task> submissionTask, string leaderboardName)
        {
            int attempt = 0;
            bool success = false;

            while (attempt < MaxRetries && !success)
            {
                try
                {
                    attempt++;
                    Debug.Log($"Attempt {attempt} to submit to {leaderboardName} leaderboard.");
                    await submissionTask();
                    success = true;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Attempt {attempt} failed to submit to {leaderboardName}: {e.Message}");

                    if (attempt >= MaxRetries)
                    {
                        Debug.LogError($"All attempts to submit to {leaderboardName} failed.");
                        throw;
                    }

                    // Wait for a second before retrying
                    await Task.Delay(1000);
                }
            }
        }

        /// <summary>
        /// Ensures the player is signed in to Unity's authentication service.
        /// </summary>
        public async Task EnsureSignedIn()
        {
            const int MaxRetries = 3;  // Set max retries
            int attempt = 0;
            bool success = false;

            while (attempt < MaxRetries && !success)
            {
                try
                {
                    attempt++;
                    Debug.Log($"Attempt {attempt} to ensure Unity Services are initialized and player is signed in.");

                    // Initialize Unity Services if needed
                    if (Unity.Services.Core.UnityServices.State != Unity.Services.Core.ServicesInitializationState.Initialized)
                    {
                        Debug.Log("Initializing Unity Services...");
                        await Unity.Services.Core.UnityServices.InitializeAsync();
                        Debug.Log("Unity Services initialized.");
                    }

                    // Sign in anonymously if not already signed in
                    if (!Unity.Services.Authentication.AuthenticationService.Instance.IsSignedIn)
                    {
                        Debug.Log("Signing in anonymously...");
                        await Unity.Services.Authentication.AuthenticationService.Instance.SignInAnonymouslyAsync();
                        Debug.Log("Anonymous sign-in successful.");
                    }
                    else
                    {
                        Debug.Log("Already signed in.");
                    }

                    // Break loop if successfuls
                    success = true;  
                }
                catch (Exception e)
                {
                    Debug.LogError($"Attempt {attempt} to sign in failed: {e.Message}");

                    if (attempt >= MaxRetries)
                    {
                        Debug.LogError("All sign-in attempts failed.");
                        throw;  // Rethrow after all retries fail
                    }

                    // Wait for a second before retrying
                    await Task.Delay(1000);  
                }
            }
        }
    }
}
