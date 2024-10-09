using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Scenes._10_PlayerScene.Scripts;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes._88_LeaderBoard.Scripts
{
    public class LeaderboardManager : MonoBehaviour
    {
        public GameObject PlayerSpawnPoint;
        [SerializeField] private TextMeshProUGUI mostWordsText;
        [SerializeField] private TextMeshProUGUI mostLettersText;
        [SerializeField] private Image exitImageButton;
        
        private const int TOPX_ENTRIES = 3;
        private const int LIMIT_ENTRY_RANGE = 1;
        private const string LEADERBOARD_ID_WORDS = "Most_Words_Leaderboard";
        private const string LEADERBOARD_ID_LETTERS = "Most_Letters_Leaderboard";

        /// <summary>
        /// Initialize Unity Services and sign in anonymously.
        /// </summary>
        private async void Start()
        {
            Debug.Log("Initializing Unity Services...");
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
                Debug.Log("Unity Services initialized.");
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("Signing in anonymously...");
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Signed in successfully.");
            }
        }

        /// <summary>
        /// Display leaderboards for most words and most letters.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task DisplayLeaderboards()
        {
            if (!mostWordsText || !mostLettersText)
            {
                Debug.LogError("TextMeshPro components are not assigned.");
                return;
            }

            Debug.Log("Displaying words leaderboard...");
            await DisplayLeaderboard(LEADERBOARD_ID_WORDS, mostWordsText, "Flest Ord");

            Debug.Log("Displaying letters leaderboard...");
            await DisplayLeaderboard(LEADERBOARD_ID_LETTERS, mostLettersText, "Flest Bogstaver");
        }

        /// <summary>
        /// Displays the leaderboard based on the provided parameters.
        /// </summary>
        /// <param name="leaderboardId">The unique identifier of the leaderboard to be displayed.</param>
        /// <param name="displayText">The text component where the leaderboard content will be displayed.</param>
        /// <param name="title">The title of the leaderboard to be displayed.</param>
        private async Task DisplayLeaderboard(
            string leaderboardId,
            TMP_Text displayText,
            string title)
        {
            try
            {
                Debug.Log($"Fetching top scores for leaderboard: {leaderboardId}");
                var topScoresResponse = await LeaderboardsService.Instance.GetScoresAsync(
                    leaderboardId,
                    new GetScoresOptions
                    {
                        IncludeMetadata = true,
                        Limit = TOPX_ENTRIES,
                    });

                string leaderboardContent = $"<u><b>{title}</b></u>\n";
                foreach (var entry in topScoresResponse.Results)
                {
                    Debug.Log($"Player: {entry.PlayerName}, Score: {entry.Score}");
                    string playerName = string.IsNullOrEmpty(entry.PlayerName) ? entry.PlayerId : entry.PlayerName;
                    playerName = entry.PlayerName.Contains("#") 
                        ? entry.PlayerName.Split('#')[0] 
                        : playerName;

                    var metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(entry.Metadata);
                    string monsterName = metadata.ContainsKey("Monster") ? metadata["Monster"] : playerName;
                    leaderboardContent += $"{entry.Rank + 1}: {playerName} - {monsterName} - Antal: {entry.Score}\n";
                }

                try
                {
                    Debug.Log("Fetching player score outside top 3.");
                    var playerScoreResponse = await LeaderboardsService.Instance.GetPlayerRangeAsync(
                        leaderboardId,
                        new GetPlayerRangeOptions
                        {
                            IncludeMetadata = true,
                            RangeLimit = LIMIT_ENTRY_RANGE
                        });

                    if (playerScoreResponse.Results.Count > 0 && playerScoreResponse.Results[0].Rank + 1 > TOPX_ENTRIES)
                    {
                        var playerEntry = playerScoreResponse.Results[0];
                        Debug.Log($"Player outside top 3: {playerEntry.PlayerName}, Rank: {playerEntry.Rank}");

                        string playerName = string.IsNullOrEmpty(playerEntry.PlayerName) ? playerEntry.PlayerId : playerEntry.PlayerName;
                        playerName = playerEntry.PlayerName.Contains("#") 
                            ? playerEntry.PlayerName.Split('#')[0] 
                            : playerName;

                        var metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(playerEntry.Metadata);
                        string monsterName = metadata.ContainsKey("Monster") ? metadata["Monster"] : playerName;

                        leaderboardContent += "\n------------------------\n";
                        leaderboardContent += $"{playerEntry.Rank + 1}: {playerName} - {monsterName} - Antal: {playerEntry.Score}\n";
                    }
                }
                catch (LeaderboardsException e)
                {
                    Debug.LogWarning($"Player has no entry in the leaderboard: {e.Message}");
                }

                displayText.text = leaderboardContent;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error retrieving leaderboard {leaderboardId}: {e.Message}\nStackTrace: {e.StackTrace}");
                displayText.text = $"Failed to load {title} leaderboard.";
            }
        }

        /// <summary>
        /// Handles actions to be performed when a scene is loaded, specifically for positioning the player and loading the leaderboard if the LeaderBoard scene is loaded.
        /// </summary>
        /// <param name="scene">The scene that has been loaded.</param>
        /// <param name="mode">The mode in which the scene has been loaded.</param>
        private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlayerManager.Instance.PositionPlayerAt(PlayerSpawnPoint);
            PlayerManager.Instance.SpawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
            
            if (scene.name == SceneNames.LeaderBoard)
            {
                if (UnityServices.State != ServicesInitializationState.Initialized)
                {
                    Debug.Log("unity service is not initialized");
                    return;
                }
            
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    Debug.Log("not signed in");
                    return;
                }
                
                //Debug.Log("Displaying Leaderboard");
                // Display the leaderboard if leaderboard scene is loaded
                await Task.Delay(100);
                await DisplayLeaderboards();
            }
        }
        
        public void OnExitButton()
        {
            PlayerManager.Instance.SpawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = true;
            
            SceneManager.LoadScene(SceneNames.Main); 
        }
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}