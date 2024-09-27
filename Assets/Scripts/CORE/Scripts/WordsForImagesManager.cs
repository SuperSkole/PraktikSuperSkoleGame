using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CORE.Scripts
{

    public class WordsForImagesManager : MonoBehaviour
    {
        private static List<string> imageWords = new();

        private static List<string> soundCorrectWords = new List<string>
        {
            "alf", "and", "en", "et", "ild", "is", "os", "orm", "ur", "ulv", "ugle",
            "�g", "�l", "�l", "bad", "hval", "spand", "tand", "hat", "kat", "mel", "pil",
            "lim", "slim", "skib", "ris", "sort", "sut", "lus", "mus", "hus", "jul",
            "syl", "lys", "nys", "l�s", "s�l", "h�l", "p�l", "knus", "spa", "ske", "sne",
            "ble", "fri", "ski", "bi", "sko", "klo", "bro", "ko", "kno", "sky", "fly",
            "kl�", "kn�", "bl�", "gr�", "sm�", "ti", "to", "ve", "te", "t�", "h�", "g�",
            "du", "fe", "b�"
        };
        /// <summary>
        /// adds a word to the list of all words, eatch word can only aprear once.
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

        public static string GetRandomSoundCorrectWordForImage()
        {
            int attempts = 0;
            string word;
            while (true)
            {
                word = soundCorrectWords[Random.Range(0, soundCorrectWords.Count)];
                word.Replace("�", "(ae)");
                word.Replace("�", "(oe)");
                word.Replace("�", "(aa)");
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