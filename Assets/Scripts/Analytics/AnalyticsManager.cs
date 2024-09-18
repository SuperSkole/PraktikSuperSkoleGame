using System;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

namespace Analytics
{
    public class AnalyticsManager : MonoBehaviour
    {
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

        private void LogWordAdded(string word, DateTime time)
        {
            // Log or store the word and time
            Debug.Log($"Word Added: {word} at {time}");
            
        }

        private void LogLetterAdded(string letter, DateTime time)
        {
            // Log or store the letter and time
            Debug.Log($"Letter Added: {letter} at {time}");
        }

    }
}
