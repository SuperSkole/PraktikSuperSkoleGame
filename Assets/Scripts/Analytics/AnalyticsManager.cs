using System.Collections.Generic;
using UnityEngine;
using System;
using Scenes._10_PlayerScene.Scripts;

namespace Analytics
{
    public class AnalyticsManager : MonoBehaviour
    {
        // Lists to store analytics data
        private List<(string word, DateTime time)> wordLog = new List<(string, DateTime)>();
        private List<(string letter, DateTime time)> letterLog = new List<(string, DateTime)>();

        private void OnEnable()
        {
            PlayerEvents.OnWordAdded += LogWordAdded;
            PlayerEvents.OnLetterAdded += LogLetterAdded;
        }

        private void OnDisable()
        {
            PlayerEvents.OnWordAdded -= LogWordAdded;
            PlayerEvents.OnLetterAdded -= LogLetterAdded;
        }

        /// <summary>
        /// Logs the word added along with the timestamp and stores it for later analysis.
        /// </summary>
        private void LogWordAdded(string word, DateTime time)
        {
            Debug.Log($"Word Added: {word} at {time}");
            wordLog.Add((word, time)); // Store word and time in the list
        }

        /// <summary>
        /// Logs the letter added along with the timestamp and stores it for later analysis.
        /// </summary>
        private void LogLetterAdded(string letter, DateTime time)
        {
            Debug.Log($"Letter Added: {letter} at {time}");
            letterLog.Add((letter, time)); // Store letter and time in the list
        }

        /// <summary>
        /// Method to analyze the stored data for patterns, averages, or other insights.
        /// </summary>
        public void AnalyzeData()
        {
            // Example: Print the total number of logged words and letters
            Debug.Log($"Total Words Logged: {wordLog.Count}");
            Debug.Log($"Total Letters Logged: {letterLog.Count}");
            
            // Additional analysis can go here (e.g., time between word additions)
        }
    }
}