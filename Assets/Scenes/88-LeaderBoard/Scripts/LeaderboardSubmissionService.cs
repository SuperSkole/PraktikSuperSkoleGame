using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;

namespace Scenes._88_LeaderBoard.Scripts
{
    public class LeaderboardSubmissionService : ILeaderboardSubmissionService
    {
        private const string LEADERBOARD_ID_WORDS = "Most_Words_Leaderboard";
        private const string LEADERBOARD_ID_LETTERS = "Most_Letters_Leaderboard";

        /// <summary>
        /// Submits the player's total word count to the leaderboard.
        /// </summary>
        public async Task SubmitMostWords(int wordCount, string monsterName)
        {
            try
            {
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

                Debug.Log("Most Words submitted successfully: " + JsonConvert.SerializeObject(playerEntry));
            }
            catch (Exception e)
            {
                Debug.LogError($"Error submitting Most Words: {e.Message}");
            }
        }

        /// <summary>
        /// Submits the player's total letter count to the leaderboard.
        /// </summary>
        public async Task SubmitMostLetters(int letterCount, string monsterName)
        {
            try
            {
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

                Debug.Log("Most Letters submitted successfully: " + JsonConvert.SerializeObject(playerEntry));
            }
            catch (Exception e)
            {
                Debug.LogError($"Error submitting Most Letters: {e.Message}");
            }
        }

        /// <summary>
        /// Ensures the player is signed into Unity's authentication service.
        /// </summary>
        public async Task EnsureSignedIn()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
    }
}
