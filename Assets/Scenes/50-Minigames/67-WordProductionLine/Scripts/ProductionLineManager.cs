using Analytics;
using CORE;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._50_Minigames._67_WordProductionLine.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._50_Minigames._67_WordProductionLine.Scripts
{


    public class ProductionLineManager : MonoBehaviour, IMinigameSetup
    {

        [SerializeField]
        private ProductionLineObjectPool objectPool;

        List<ILanguageUnit> langUnit;


        private string fixedWord;


        private void Start()
        {
            langUnit = GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(14);
            
        }

        /// <summary>
        /// gets one letter and checks if its correct or not
        /// </summary>
        /// <returns> random letter.</returns>
        public string GetLetters()
        {
            string randomLetter = LetterManager.GetRandomLetter().ToString();

            return randomLetter;
        }

        /// <summary>
        /// gets a random word for image.
        /// </summary>
        /// <returns> random word.</returns>
        public string GetImages()
        {
            
            string randomWord = langUnit[Random.Range(0, langUnit.Count)].Identifier;

            return randomWord;
        }

        /// <summary>
        /// Fixes the answer.
        /// </summary>
        /// <returns> a letter from a randomword.</returns>
        public string GetFixedCorrect()
        {
            string randomWord = GetImages();

            char randomCharLetter = randomWord.ToString()[0];

            string letter = $"{randomCharLetter}";

            return letter;


        }

        public void SetupGame(IGenericGameMode gameMode, IGameRules gameRules)
        {
            
        }
    }

}