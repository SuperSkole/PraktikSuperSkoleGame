using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CORE;
using Newtonsoft.Json;
using Scenes._10_PlayerScene.Scripts;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._88_LeaderBoard.Scripts
{
    public class LeaderboardManager : MonoBehaviour, ILeaderboardService
    {
        [SerializeField] private TMP_Text mostWordsText;
        [SerializeField] private TMP_Text mostLettersText;
        [SerializeField] private TMP_Text mostGoldText;
        [SerializeField] private TMP_Text highestLevelText;
        
        private const int NUMBER_OF_ENTRIES = 10;
        private const string LEADERBOARD_ID = "TestLeaderBoard";
        private const string LEADERBOARD_ID_WORDS = "Most_Words_Leaderboard";

        // Player Manager Singleton
        private static LeaderboardManager instance;

        public static LeaderboardManager Instance => instance;
        
        public static void Initialize()
        {
            if (!instance)
            {
                GameObject leaderboardObject = new GameObject("LeaderboardManager");
                instance = leaderboardObject.AddComponent<LeaderboardManager>();
                
                // set as a child of PlayerManager
                leaderboardObject.transform.SetParent(PlayerManager.Instance.transform);
            }
        }

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
        
        private void OnEnable()
        {
            PlayerEvents.OnAddWord += OnAddWordHandler;
            // PlayerEvents.OnXPChanged += OnXPChangedHandler;
            // PlayerEvents.OnGoldChanged += OnGoldChangedHandler;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            PlayerEvents.OnAddWord -= OnAddWordHandler;
            // PlayerEvents.OnXPChanged -= OnXPChangedHandler;
            // PlayerEvents.OnGoldChanged -= OnGoldChangedHandler;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == SceneNames.LeaderBoard)
            {
                // Display the leaderboard if leaderboard scene is loaded
                await DisplayLeaderboards();
            }
        }
        
        private async void OnAddWordHandler(string word)
        {
            
            int totalWords = PlayerManager.Instance.PlayerData.CollectedWords.Count;
            await SubmitMostWords(totalWords);
        }

        private async void OnXPChangedHandler(int xp)
        {
            
            await SubmitXP(xp);
        }

        private async void OnGoldChangedHandler(int gold)
        {
            
            int totalGold = PlayerManager.Instance.PlayerData.CurrentGoldAmount;
            await SubmitMostGold(totalGold);
        }

        private async void OnLevelUpHandler(int level)
        {
            await SubmitHighestLevel(level);
        }

        private async Task DisplayLeaderboards()
        {
            await DisplayLeaderboard("LEADERBOARD_ID_WORDS", mostWordsText, "Most Words");
            // await DisplayLeaderboard("most_letters_leaderboard", mostLettersText, "Most Letters");
            // await DisplayLeaderboard("most_gold_leaderboard", mostGoldText, "Most Gold");
            // await DisplayLeaderboard("highest_level_leaderboard", highestLevelText, "Highest Level");
        }
        
        private async Task DisplayLeaderboard(string leaderboardId, TMP_Text displayText, string title)
        {
            try
            {
                var scoresResponse
                    = await LeaderboardsService.Instance.GetScoresAsync(
                        leaderboardId,
                        new GetScoresOptions
                        {
                            IncludeMetadata = true, 
                            Offset = 10,
                            Limit = NUMBER_OF_ENTRIES,
                        });

                string leaderboardContent = $"<u><b>{title}</b></u>\n";

                foreach (var entry in scoresResponse.Results)
                {
                    string playerName = string.IsNullOrEmpty(entry.PlayerName) ? entry.PlayerId : entry.PlayerName;
                    leaderboardContent += $"Rank {entry.Rank}: {playerName} - Score: {entry.Score}\n";
                }

                displayText.text = leaderboardContent;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error retrieving leaderboard {leaderboardId}: {e.Message}");
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

        public async Task SubmitScore(int score)
        {
            try
            {
                await LeaderboardsService.Instance.AddPlayerScoreAsync(LEADERBOARD_ID, score);
                Debug.Log("Score submitted successfully.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error submitting score: {e.Message}");
            }
        }
        
        public async Task SubmitXP(int xp)
        {
            try
            {
                await LeaderboardsService.Instance.AddPlayerScoreAsync(LEADERBOARD_ID, xp);
                Debug.Log("XP submitted successfully.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error submitting XP: {e.Message}");
            }
        }
        
        public async Task SubmitMostWords(int wordCount)
        {
            try
            {
                var metadata = new Dictionary<string, string>() { {"Navn", $"{GameManager.Instance.CurrentUser}"} };
                var playerEntry
                    = await LeaderboardsService.Instance.AddPlayerScoreAsync(
                        LEADERBOARD_ID_WORDS,
                        wordCount,
                        new AddPlayerScoreOptions
                        {
                            Metadata = new Dictionary<string, string>()
                            {
                                {
                                    "Navn",
                                    $"{GameManager.Instance.CurrentUser}"
                                }
                            }
                        });
                
                Debug.Log("Most Words submitted successfully: " +JsonConvert.SerializeObject(playerEntry));
            }
            catch (Exception e)
            {
                Debug.LogError($"Error submitting Most Words: {e.Message}");
            }
        }

        public async Task SubmitMostLetters(int letterCount)
        {
            try
            {
                await LeaderboardsService.Instance.AddPlayerScoreAsync("most_letters_leaderboard", letterCount);
                Debug.Log("Most Letters submitted successfully.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error submitting Most Letters: {e.Message}");
            }
        }

        public async Task SubmitMostGold(int goldAmount)
        {
            try
            {
                await LeaderboardsService.Instance.AddPlayerScoreAsync("most_gold_leaderboard", goldAmount);
                Debug.Log("Most Gold submitted successfully.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error submitting Most Gold: {e.Message}");
            }
        }

        public async Task SubmitHighestLevel(int level)
        {
            try
            {
                await LeaderboardsService.Instance.AddPlayerScoreAsync("highest_level_leaderboard", level);
                Debug.Log("Highest Level submitted successfully.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error submitting Highest Level: {e.Message}");
            }
        }
    }
}
