using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE;
using LoadSave;
using Scenes._00_Bootstrapper;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

namespace Scenes._89_MultiPlayerHighScoreScene.Scripts
{
    public class AllTimeHighScoreManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text mostWordsText;
        [SerializeField] private TMP_Text mostLettersText;
        [SerializeField] private TMP_Text mostGoldText;
        [SerializeField] private TMP_Text highestLevelText;

        private void OnEnable()
        {
            // Subscribe to the event to wait for authentication
            PlayerStrapper.OnPlayerAuthenticated += OnPlayerAuthenticated;
        }

        private void OnDisable()
        {
            // Unsubscribe from the event when the object is disabled
            PlayerStrapper.OnPlayerAuthenticated -= OnPlayerAuthenticated;
        }

        private void Start()
        {
            // In case the player is already authenticated
            if (UnityServices.State == ServicesInitializationState.Initialized && 
                AuthenticationService.Instance.IsSignedIn)
            {
                StartCoroutine(WaitForInitialization());
            }
        }

        private IEnumerator WaitForInitialization()
        {
            // Wait for Unity services and authentication to be fully initialized
            while (UnityServices.State != ServicesInitializationState.Initialized || 
                   !AuthenticationService.Instance.IsSignedIn)
            {
                yield return null;
            }

            // Now we are sure that services are initialized and the user is authenticated
            yield return GetAndDisplayAllTimeHighScores();
        }

        private async void OnPlayerAuthenticated()
        {
            // If the authentication event is triggered, fetch and display scores
            await GetAndDisplayAllTimeHighScores();
        }

        private async Task GetAndDisplayAllTimeHighScores()
        {
            // Load all save keys
            var keys = await CloudSaveService.Instance.Data.Player.ListAllKeysAsync();
            

            List<string> saveKeys = new List<string>();

            foreach (var keyItem in keys)
            {
                saveKeys.Add(keyItem.Key);
            }
            
            // Load all player data
            List<PlayerData> allPlayerData = new List<PlayerData>();

            foreach (var saveKey in saveKeys)
            {
                // PlayerData playerData = await GameManager.Instance.SaveGameController.LoadSaveDataAsync(saveKey);
                // if (playerData)
                // {
                //     allPlayerData.Add(playerData);
                // }
            }

            // Calculate and display the top users
            UpdateHighScores(allPlayerData);
        }

        private void UpdateHighScores(List<PlayerData> allPlayerData)
        {
            // Sort by Most Words
            var topWordsUsers = allPlayerData.OrderByDescending(p => p.CollectedWords.Count).Take(3).ToList();
            mostWordsText.text = $"<u><b>Most Words</b></u>\n{FormatTopUsers(topWordsUsers, p => p.CollectedWords.Count)}";

            // Sort by Most Letters
            var topLettersUsers = allPlayerData.OrderByDescending(p => p.CollectedLetters.Count).Take(3).ToList();
            mostLettersText.text = $"<u><b>Most Letters</b></u>\n{FormatTopUsers(topLettersUsers, p => p.CollectedLetters.Count)}";

            // Sort by Highest Level
            var topLevelUsers = allPlayerData.OrderByDescending(p => p.CurrentLevel).Take(3).ToList();
            highestLevelText.text = $"<u><b>Highest Level</b></u>\n{FormatTopUsers(topLevelUsers, p => p.CurrentLevel)}";

            // Sort by Most Gold
            var topGoldUsers = allPlayerData.OrderByDescending(p => p.CurrentGoldAmount).Take(3).ToList();
            mostGoldText.text = $"<u><b>Most Gold</b></u>\n{FormatTopUsers(topGoldUsers, p => p.CurrentGoldAmount)}";

            // Uncomment for Most Clothes:
            // var topClothesUsers = allPlayerData.OrderByDescending(p => p.BoughtClothes.Count).Take(3).ToList();
            // mostClothesText.text = $"<u><b>Most Clothes</b></u>\n{FormatTopUsers(topClothesUsers, p => p.BoughtClothes.Count)}";
        }

        private string FormatTopUsers(List<PlayerData> topUsers, Func<PlayerData, int> valueSelector)
        {
            // Formats the top 3 users into a string for the UI with each on a new line
            if (topUsers.Count == 0)
            {
                return "No data available";
            }
            
            return string.Join("\n", topUsers.Select((p, index) => $"{index + 1}. {p.Username}: {valueSelector(p)}"));
        }
        
        private void SetNoDataAvailable()
        {
            mostWordsText.text = "No data available";
            mostLettersText.text = "No data available";
            mostGoldText.text = "No data available";
            highestLevelText.text = "No data available";
        }
    }
}