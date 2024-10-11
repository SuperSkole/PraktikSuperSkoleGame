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

        private bool kickedOut = false;

        private string fixedWord;


        private void Start()
        {
            langUnit = new List<ILanguageUnit>();
            List<ILanguageUnit> units = GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(80);
            foreach (ILanguageUnit unit in units)
            {
                if (unit.LanguageUnitType == LanguageUnit.Word && WordsForImagesManager.imageWords.Contains(unit.Identifier))
                {
                    langUnit.Add(unit);
                }
            }
        }


        private void Update()
        {
            if (langUnit.Count == 0 && !kickedOut)
            {
                kickedOut = true;
                SwitchScenes.SwitchToMainWorld();
            }
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

            if (randomWord.Length <= 1)
            {
                randomWord = WordsForImagesManager.GetRandomWordForImage();
            }



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