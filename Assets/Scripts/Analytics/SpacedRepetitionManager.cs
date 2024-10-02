using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CORE;
using CORE.Scripts;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

namespace Analytics
{
    public class SpacedRepetitionManager : PersistentSingleton<SpacedRepetitionManager>, ISpacedRepetitionManager
    {
        private readonly IWeightManager weightManager;
        private readonly TimeSpan repetitionInterval = TimeSpan.FromDays(7);

        private ConcurrentDictionary<string, ILanguageUnit> letterWeights;

        protected override void Awake()
        {
            base.Awake();
            SetWeightConnection();
            InitializeTimeWeights();
        }

        private void SetWeightConnection()
        {
            if (letterWeights == null)
            {
                letterWeights = PlayerManager.Instance.PlayerData.LettersWeightsProperty;
            }
        }

        private void InitializeTimeWeights()
        {
            foreach (var kvp in letterWeights)
            {
                var languageUnit = kvp.Value;
                var elapsedTimeInDays = DateTime.Now - languageUnit.LastUsed;

                if (elapsedTimeInDays >= repetitionInterval)
                {
                    languageUnit.TimeWeight += CalculateTimeWeight(elapsedTimeInDays);
                }
            }
        }

        private float CalculateTimeWeight(TimeSpan elapsedTime)
        {
            
            int days = (int)elapsedTime.TotalDays;
            return days * 0.1f; 
        }

        public void UpdateWeightsBasedOnTime()
        {
            foreach (var kvp in letterWeights)
            {
                var languageUnit = kvp.Value;
                var elapsedTime = DateTime.Now - languageUnit.LastUsed;

                if (elapsedTime >= repetitionInterval)
                {
                    languageUnit.TimeWeight += CalculateTimeWeight(elapsedTime);
                }
            }
        }

        public void RecordUsage(string identifier)
        {
            if (letterWeights.TryGetValue(identifier,
                    out ILanguageUnit languageUnit))
            {
            
                languageUnit.LastUsed = DateTime.Now;

                languageUnit.Weight
                    = Math.Max(0,
                        languageUnit.TimeWeight -
                        0.5f); 
            }
            else
            {
                Debug.Log("Identifier not found in letterWeights: " + identifier);
            }
        }

        public void AdjustWeightsBasedOnPerformance(string identifier, bool isCorrect)
        {
            if (letterWeights.TryGetValue(identifier, out ILanguageUnit languageUnit))
            {
                if (isCorrect)
                {
                    // Øg vægten mindre, da brugeren husker det
                    languageUnit.TimeWeight += WeightSettings.WeightIncrement;
                }
                else
                {
                    // Øg vægten mere, da brugeren glemte det
                    languageUnit.TimeWeight += WeightSettings.WeightDecrement;
                }

                // Opdater LastUsed
                languageUnit.LastUsed = DateTime.Now;
            }
        }

        public Dictionary<char, int> GetCurrentTimeWeights()
        {
            var currentWeights = new Dictionary<char, int>();

            foreach (var kvp in letterWeights)
            {
                if (char.TryParse(kvp.Key, out char letter))
                {
                    currentWeights[letter] = (int)kvp.Value.TimeWeight;
                }
            }

            return currentWeights;
        }
    }
}
