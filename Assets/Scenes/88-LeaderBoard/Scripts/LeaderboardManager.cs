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

        private async void Start()
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

        private async Task DisplayLeaderboards()
        {
            if (!mostWordsText || !mostLettersText)
            {
                Debug.LogError("Textmeshugui components are not assigned.");
                return;
            }
            
            await DisplayLeaderboard(LEADERBOARD_ID_WORDS, mostWordsText, "Flest Ord");
            await DisplayLeaderboard(LEADERBOARD_ID_LETTERS, mostLettersText, "Flest Bogstaver");
        }
        
        private async Task DisplayLeaderboard(string leaderboardId, TMP_Text displayText, string title)
        {
            try
            {
                // Fetch the top 5 scores
                var topScoresResponse = await LeaderboardsService.Instance.GetScoresAsync(
                    leaderboardId,
                    new GetScoresOptions
                    {
                        IncludeMetadata = true,
                        Limit = TOPX_ENTRIES, // Fetch top x players
                    });

                string leaderboardContent = $"<u><b>{title}</b></u>\n";

                // Display the top 5 players
                foreach (var entry in topScoresResponse.Results)
                {
                    string playerName = string.IsNullOrEmpty(entry.PlayerName) ? entry.PlayerId : entry.PlayerName;
                    playerName = entry.PlayerName.Contains("#") 
                        ? entry.PlayerName.Split('#')[0] 
                        : entry.PlayerName;

                    var metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(entry.Metadata);
                    string monsterName = metadata.ContainsKey("Monster") ? metadata["Monster"] : playerName;

                    leaderboardContent += $"{entry.Rank + 1}: {playerName} - {monsterName} - Antal: {entry.Score}\n";
                }

                // Attempt to fetch the player's rank if they are not in the top x
                try
                {
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

                        string playerName = string.IsNullOrEmpty(playerEntry.PlayerName) ? playerEntry.PlayerId : playerEntry.PlayerName;
                        playerName = playerEntry.PlayerName.Contains("#") 
                            ? playerEntry.PlayerName.Split('#')[0] 
                            : playerEntry.PlayerName;

                        var metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(playerEntry.Metadata);
                        string monsterName = metadata.ContainsKey("Monster") ? metadata["Monster"] : playerName;

                        // Add a separator and the player's rank if they are outside the top x
                        leaderboardContent += "\n------------------------\n";
                        leaderboardContent += $"{playerEntry.Rank + 1}: {playerName} - {monsterName} - Antal: {playerEntry.Score}\n";
                    }
                }
                catch (LeaderboardsException e)
                {
                    // Player has no entry in the leaderboard; skip displaying their rank
                    Debug.Log("Player has no entry in the leaderboard." + e);
                }

                // Set the final content
                displayText.text = leaderboardContent;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error retrieving leaderboard {leaderboardId}: {e.Message}\nStackTrace: {e.StackTrace}");
                displayText.text = $"Failed to load {title} leaderboard.";
            }
        }


        
        public async void GetPlayerRangeWithMetadata(string leaderboardId)
        {
            // Returns a total of 11 entries (the given player plus 5 on either side)
            var rangeLimit = 5;
            
            var scoreResponse
                = await LeaderboardsService.Instance.GetPlayerRangeAsync(
                    leaderboardId,
                    new GetPlayerRangeOptions
                    {
                        IncludeMetadata = true,
                        RangeLimit = rangeLimit
                    });
        }
    }
}