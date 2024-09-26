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

        /// <summary>
        /// gets one letter and checks if its correct or not
        /// </summary>
        /// <returns></returns>
        public string GetLetters()
        {
            string randomLetter = LetterManager.GetRandomLetter().ToString();

            return randomLetter;
        }

        /// <summary>
        /// gets a random word for image.
        /// </summary>
        /// <returns></returns>
        public string GetImages()
        {
            string randomWord = WordsForImagesManager.GetRandomWordForImage();

            return randomWord;
        }


        
        
    }

}