using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CORE.Scripts
{

    public class ImageManager : MonoBehaviour
    {
        /// <summary>
        /// takes in a word and reterns an image corrisponting.
        /// </summary>
        /// <param name="inputWord">the word you want to get an image for</param>
        /// <returns>a UnityEngine.UI image or if it couldent find an image it returnes NULL</returns>
        public static Image GetImageFromWord(string inputWord)
        {
            Image image = null;





            return image;
        }

        /// <summary>
        /// takes in an array of words and reterns an array of corrisponting images.
        /// </summary>
        /// <param name="inputWords">the words you want to get images for</param>
        /// <returns>a UnityEngine.UI image or if it couldent find anny image it returnes NULL</returns>
        public static Image[] GetImageFromWord(string[] inputWords)
        {
            Image[] images = null;





            return images;
        }

        /// <summary>
        /// gets a random image from the libarry.
        /// </summary>
        /// <returns>a random image</returns>
        public static Image GetRandomImage()
        {
            Image image = null;


            return image;
        }

        /// <summary>
        /// gets multibull random images.
        /// </summary>
        /// <param name="amonunt">the amount of images you want</param>
        /// <returns>an array of random images</returns>
        public static Image GetRandomImage(int amonunt)
        {
            Image image = null;


            return image;
        }
    }
}
