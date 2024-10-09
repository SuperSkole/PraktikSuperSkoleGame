using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CORE.Scripts
{

    public class WordsForImagesManager : MonoBehaviour
    {
        public static List<string> imageWords = new();

        private static List<string> soundCorrectWords = new List<string>
        {
            "alf", "and", "en", "et", "ild", "is", "os", "orm", "ur", "ulv", "ugle",
            "\u00e6g", "\u00f8l", "\u00e5l", "bad", "hval", "spand", "tand", "hat", "kat", "mel", "pil",
            "lim", "slim", "skib", "ris", "sort", "sut", "lus", "mus", "hus", "jul",
            "syl", "lys", "nys", "l\u00f8s", "s\u00e6l", "h\u00e6l", "p\u00e6l", "knus", "spa", "ske", "sne",
            "ble", "fri", "ski", "bi", "sko", "klo", "bro", "ko", "kno", "sky", "fly",
            "kl\u00f8", "kn\u00e6", "bl\u00e5", "gr\u00e5", "sm\u00e5", "ti", "to", "ve", "te", "t\u00f8", "h\u00f8", "g\u00f8",
            "du", "fe", "b\u00e6"
        };
        /// <summary>
        /// adds a word to the list of all wordsOrLetters, eatch word can only aprear once.
        /// </summary>
        /// <param name="name">the name to add to the list</param>
        public static void AddNameToSet(string name)
        {
            if (imageWords.Contains(name)) return;
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
        /// a function to get an amount of random wordsOrLetters that has an image.
        /// </summary>
        /// <param name="amount">the amount of wordsOrLetters you want</param>
        /// <returns>an array of wordsOrLetters that has a corresponding image</returns>
        public static string[] GetRandomWordForImage(int amount)
        {
            string[] words = new string[amount];
            for (int i = 0; i < amount; i++)
            {
                words[i] = GetRandomWordForImage();
            }
            return words;
        }

        public static string GetRandomSoundCorrectWordForImage()
        {
            int attempts = 0;
            string word;
            while (true)
            {
                word = soundCorrectWords[Random.Range(0, soundCorrectWords.Count)];
                word.Replace("\u00e6", "(ae)");
                word.Replace("\u00f8", "(oe)");
                word.Replace("\u00e5", "(aa)");
                if (imageWords.Contains(word))
                {
                    return word;
                }
                else
                {
                    attempts++;
                    if(attempts > 50)
                    {
                        Debug.Log("could not find a word in the list that fit an image, using GetRandomWordForImage() instead");
                        word = GetRandomWordForImage();
                        return word;
                    }
                }
            }
        }
    }

}