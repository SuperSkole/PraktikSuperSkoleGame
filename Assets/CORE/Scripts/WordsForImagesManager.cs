using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

namespace CORE.Scripts
{

    public class WordsForImagesManager : MonoBehaviour
    {
        private static List<string> imageWords = new();

        /// <summary>
        /// adds a word to the list of all words, eatch word can only aprear once.
        /// </summary>
        /// <param name="name">the name to add to the list</param>
        public static void AddNameToSet(string name)
        {
            if(imageWords.Contains(name)) return;
            imageWords.Add(name);
        }

        /// <summary>
        /// a function to get a random word that has an image.
        /// </summary>
        /// <returns>a word the has a corresponding image</returns>
        public static string GetRandomWordForImage()
        {
            return imageWords.ElementAt(Random.Range(0, imageWords.Count));
        }

        /// <summary>
        /// a function to get an amount of random words that has an image.
        /// </summary>
        /// <param name="amount">the amount of words you want</param>
        /// <returns>an array of words that has a corresponding image</returns>
        public static string[] GetRandomWordForImage(int amount)
        {
            string[] words = new string[amount];
            for (int i = 0; i < amount; i++)
            {
                words[i] = GetRandomWordForImage();
            }
            return words;
        }
    }

}