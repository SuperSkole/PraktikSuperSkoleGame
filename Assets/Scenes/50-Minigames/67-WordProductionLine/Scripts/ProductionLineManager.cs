using CORE.Scripts;
using Scenes._50_Minigames._67_WordProductionLine.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._50_Minigames._67_WordProductionLine.Scripts
{


    public class ProductionLineManager : MonoBehaviour
    {

        [SerializeField]
        private ProductionLineObjectPool objectPool;

        private int countDown = 0;

        private string fixedWord;

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

            string randomWord = WordsForImagesManager.GetRandomWordForImage();

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
    }

}