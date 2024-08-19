using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace CORE.Scripts
{

    public class ImageManager : MonoBehaviour
    {

        private static Dictionary<string, List<Texture2D>> imageDictionary = new();
        public static bool IsDataLoaded { get; private set; } = false;


        /// <summary>
        /// adds an image to the dictionary.
        /// </summary>
        /// <param name="name">the name/key for the image</param>
        /// <param name="image">the image to add</param>
        public static void AddImageToSet(string name,Texture2D image)
        {
            if (imageDictionary.ContainsKey(name))
            {
                imageDictionary[name].Add(image);
            }
            else
            {
                imageDictionary.Add(name, new List<Texture2D>());
                imageDictionary[name].Add(image);
            }
        }


        /// <summary>
        /// takes in a word and reterns an image corrisponting.
        /// </summary>
        /// <param name="inputWord">the word you want to get an image for</param>
        /// <returns>a image or if it couldent find an image it returnes NULL</returns>
        public static Texture2D GetImageFromWord(string inputWord)
        {
            if (!imageDictionary.TryGetValue(inputWord, out List<Texture2D> data))
                data = null;
            Texture2D image;
            if (data == null)
            {
                Debug.LogError($"Error getting image for the word: {inputWord.ToLower()}");
            }
            if (data.Count > 1)
                image = data[UnityEngine.Random.Range(0, data.Count)];
            else
                image = data[0];

            return image;
        }

        /// <summary>
        /// takes in an array of words and reterns an array of corrisponting images.
        /// </summary>
        /// <param name="inputWords">the words you want to get images for</param>
        /// <returns>a image or if it couldent find anny image it returnes NULL</returns>
        public static Texture2D[] GetImageFromWord(string[] inputWords)
        {
            Texture2D[] images = new Texture2D[inputWords.Length];

            for (int i = 0; i < inputWords.Length; i++)
            {
                List<Texture2D> data;
                if (!imageDictionary.TryGetValue(inputWords[i], out data))
                    data = null;
                if (data == null)
                {
                    Debug.LogError($"Error getting image for the word: {inputWords[i].ToLower()}");
                }
                if (data.Count > 1)
                    images[i] = data[UnityEngine.Random.Range(0, data.Count)];
                else
                    images[i] = data[0];
            }

            return images;
        }

        /// <summary>
        /// gets a random image from the libarry.
        /// </summary>
        /// <returns>a random image</returns>
        public static Texture2D GetRandomImage()
        {
            List<Texture2D> data;
            data = imageDictionary.ElementAt(UnityEngine.Random.Range(0, imageDictionary.Keys.Count)).Value;
            Texture2D image;
            if (data == null)
            {
                Debug.LogError($"Error getting a random image");
            }
            if (data.Count > 1)
                image = data[UnityEngine.Random.Range(0, data.Count)];
            else
                image = data[0];

            return image;
        }

        /// <summary>
        /// gets multibull random images.
        /// </summary>
        /// <param name="amonunt">the amount of images you want</param>
        /// <returns>an array of random images</returns>
        public static Texture2D[] GetRandomImage(int amonunt)
        {
            Texture2D[] images = new Texture2D[amonunt];

            for (int i = 0; i < amonunt; i++)
            {
                List<Texture2D> data;
                data = imageDictionary.ElementAt(UnityEngine.Random.Range(0, imageDictionary.Keys.Count)).Value;
                if (data == null)
                {
                    Debug.LogError($"Error getting random images");
                }
                if (data.Count > 1)
                    images[i] = data[UnityEngine.Random.Range(0, data.Count)];
                else
                    images[i] = data[0];
            }

            return images;
        }
    }
}
