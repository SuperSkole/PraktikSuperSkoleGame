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
            Debug.Log("LeaderboardManager.start(): Initializing Unity Services...");
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                Debug.Log("LeaderboardManager.start(): Unity Services not initialized. Initializing...");
                try
                {
                    await UnityServices.InitializeAsync();
                    Debug.Log("LeaderboardManager.start(): Unity Services initialized successfully.");
                }
                catch (Exception e)
                {
                    Debug.LogError("LeaderboardManager.start(): Failed to initialize Unity Services: " + e.Message);
                    return;
                }
            }
    
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("LeaderboardManager.start(): not signed in. Signing in anonymously...");
                try
                {
                    Debug.Log("LeaderboardManager.start(): Signing in anonymously...");
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    Debug.Log("LeaderboardManager.start(): Anonymous sign-in successful.");
                }
                catch (Exception e)
                {
                    Debug.LogError("LeaderboardManager.start(): Failed to sign in anonymously: " + e.Message);
                    return;
                }
            }
        }


        /// <summary>
        /// Display leaderboards for most words and most letters.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task<bool> DisplayLeaderboards()
        {
            Debug.Log("Insdie DisplayLeaderboards");
            try
            {
                Debug.Log("LeaderboardManager.DisplayLeaderboards(): Displaying leaderboards...");

                // Add delay to ensure WebGL async timing issues don't occur
                await Task.Delay(500); 

                if (!mostWordsText || !mostLettersText)
                {
                    Debug.LogError("TextMeshPro components are not assigned.");
                    // Return false if required components are missing
                    return false;  
                }

                Debug.Log("Fetching words leaderboard...");
                await DisplayLeaderboard(LEADERBOARD_ID_WORDS, mostWordsText, "Flest Ord");

                Debug.Log("Fetching letters leaderboard...");
                await DisplayLeaderboard(LEADERBOARD_ID_LETTERS, mostLettersText, "Flest Bogstaver");

                Debug.Log("LeaderboardManager.DisplayLeaderboards(): Leaderboards fetched successfully.");
                // Return true if everything went well
                return true;  
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception in DisplayLeaderboards: {e.Message}\n{e.StackTrace}");
                // Return false if an error occurred
                return false;  
            }
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
            
            Debug.Log($"LeaderboardManager.OnSceneLoaded(): Scene loaded: {scene.name}");
            if (scene.name == SceneNames.LeaderBoard)
            {
                Debug.Log("LeaderboardManager.Onscenelaoded: inside Leaderboard scene");
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
                
                Debug.Log("LeaderboardManager.OnSceneLoaded: Displaying Leaderboard");

                try
                {
                    Debug.Log("LeaderboardManager.OnSceneLoaded: inside try for Displaying Leaderboard");
                    //await Task.Delay(100);
                    bool success = await DisplayLeaderboards();
                    if (success)
                    {
                        Debug.Log("Leaderboards displayed successfully.");
                    }
                    else
                    {
                        Debug.LogError("Failed to display leaderboards.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Exception occurred while displaying leaderboards: {e.Message}\n{e.StackTrace}");
                }
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