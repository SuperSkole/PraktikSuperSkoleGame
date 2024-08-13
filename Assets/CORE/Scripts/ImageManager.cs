using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace CORE.Scripts
{

    public class ImageManager : MonoBehaviour
    {

        static Dictionary<string, Texture2D[]> imageDictionary = new();
        /// <summary>
        /// takes in a word and reterns an image corrisponting.
        /// </summary>
        /// <param name="inputWord">the word you want to get an image for</param>
        /// <returns>a UnityEngine.UI image or if it couldent find an image it returnes NULL</returns>
        public static Sprite GetImageFromWord(string inputWord)
        {
            Sprite image = null;

            image = Resources.Load<Sprite>($"Pictures/{inputWord.ToLower()}_image");
            if(image == null)
            {
                Debug.LogError($"Error loading image: {inputWord.ToLower()}_image");
            }

            return image;
        }

        /// <summary>
        /// takes in an array of words and reterns an array of corrisponting images.
        /// </summary>
        /// <param name="inputWords">the words you want to get images for</param>
        /// <returns>a UnityEngine.UI image or if it couldent find anny image it returnes NULL</returns>
        public static Sprite[] GetImageFromWord(string[] inputWords)
        {
            Sprite[] images = new Sprite[inputWords.Length];

            for (int i = 0; i < inputWords.Length; i++)
            {
                images[i] = Resources.Load<Sprite>($"Pictures/{inputWords[i].ToLower()}_image");
                if (images[i] == null)
                {
                    Debug.LogError($"Error loading image: {inputWords[i].ToLower()}_image");
                }
            }

            return images;
        }

        /// <summary>
        /// gets a random image from the libarry.
        /// </summary>
        /// <returns>a random image</returns>
        public static Sprite GetRandomImage()
        {
            Sprite image = null;


            return image;
        }

        /// <summary>
        /// gets multibull random images.
        /// </summary>
        /// <param name="amonunt">the amount of images you want</param>
        /// <returns>an array of random images</returns>
        public static Sprite[] GetRandomImage(int amonunt)
        {
            Sprite[] image = null;


            return image;
        }
    }


    public struct LoadeImage : IJob
    {
        public string path;
        public Texture2D texture;
        public void Execute()
        {
            //loade data
            byte[] bytes = File.ReadAllBytes(path);
            texture.LoadImage(bytes);
        }
    }
}
