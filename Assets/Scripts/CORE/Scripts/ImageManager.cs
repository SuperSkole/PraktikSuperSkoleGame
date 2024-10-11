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

        private static Dictionary<string, List<Texture2D>> letterImageDictionary = new();

        private static List<string> firstLettersForImages=new();
       
        public static bool IsDataLoaded { get; private set; } = false;


        /// <summary>
        /// adds an image to the dictionary.
        /// </summary>
        /// <param name="name">the name/key for the image</param>
        /// <param name="image">the image to add</param>
        public static void AddImageToSet(string name,Texture2D image)
        {
            if (imageDictionary.ContainsKey(name.ToLower()))
                imageDictionary[name.ToLower()].Add(image);
            else
            {
                imageDictionary.Add(name.ToLower(), new List<Texture2D>());
                imageDictionary[name.ToLower()].Add(image);
            }
            IsDataLoaded = true;
        }

        /// <summary>
        /// Adds an image to the letterImageDictionary
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="image"></param>
        public static void AddImageToLetterSet(string letter, Texture2D image)
        {
            
            if (letterImageDictionary.ContainsKey(letter.ToLower()))
                letterImageDictionary[letter.ToLower()].Add(image);
            else
            {
                letterImageDictionary.Add(letter.ToLower(), new List<Texture2D>());
                letterImageDictionary[letter.ToLower()].Add(image);
                
            }
            IsDataLoaded = true;
        }


        /// <summary>
        /// Get an image from the letterImageDictionary 
        /// </summary>
        /// <param name="inputLetter"></param>
        /// <returns></returns>
        public static Texture2D GetImageFromLetter(string inputLetter)
        {

            string letterToGet = inputLetter;

            letterToGet.Replace("(aa)", "\u00e5");
            letterToGet.Replace("(ae)", "\u00e6");
            letterToGet.Replace("(oe)", "\u00f8");

            if (!letterImageDictionary.TryGetValue(letterToGet.ToLower(), out List<Texture2D> data))
                data = null;
            Texture2D image;
            if (data == null)
                Debug.LogError($"Error getting image for the word: {letterToGet}");
            if (data.Count > 1)
                image = data[UnityEngine.Random.Range(0, data.Count)];
            else
                image = data[0];

             

            return image;
        }


        public static string GetRandomFirstLetterFromImageDic()
        {
            string returnedLetter="";

            int randIndex = UnityEngine.Random.Range(0, letterImageDictionary.Count);

            int currentindex = 0;

            foreach (var item in letterImageDictionary.Keys)
            {
                if(currentindex==randIndex)
                {
                    returnedLetter = item;
                }
                currentindex++;   
            }


            return returnedLetter;
           
        }

        /// <summary>
        /// takes in a word and reterns an image corrisponting.
        /// </summary>
        /// <param name="inputWord">the word you want to get an image for</param>
        /// <returns>a image or if it couldent find an image it returnes NULL</returns>
        public static Texture2D GetImageFromWord(string inputWord)
        {
            if (!imageDictionary.TryGetValue(inputWord.ToLower(), out List<Texture2D> data))
                data = null;
            Texture2D image;
            if (data == null)
                Debug.LogError($"Error getting image for the word: {inputWord}");
            if (data.Count > 1)
                image = data[UnityEngine.Random.Range(0, data.Count)];
            else
                image = data[0];

            return image;
        }

        /// <summary>
        /// takes in an array of wordsOrLetters and reterns an array of corrisponting images.
        /// </summary>
        /// <param name="inputWords">the wordsOrLetters you want to get images for</param>
        /// <returns>a image or if it couldent find anny image it returnes NULL</returns>
        public static Texture2D[] GetImageFromWord(string[] inputWords)
        {
            Texture2D[] images = new Texture2D[inputWords.Length];
            for (int i = 0; i < inputWords.Length; i++)
            {
                if (!imageDictionary.TryGetValue(inputWords[i].ToLower(), out List<Texture2D> data))
                    data = null;
                if (data == null)
                    Debug.LogError($"Error getting image for the word: {inputWords[i]}");
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
            List<Texture2D> data = imageDictionary.ElementAt(UnityEngine.Random.Range(0, imageDictionary.Keys.Count)).Value;
            Texture2D image;
            if (data == null)
                Debug.LogError($"Error getting a random image");
            if (data.Count > 1)
                image = data[UnityEngine.Random.Range(0, data.Count)];
            else
                image = data[0];
            return image;
        }

        /// <summary>
        /// Gets a random image and its key in the imagedictionary
        /// </summary>
        /// <returns></returns>
        public static Tuple<Texture2D,string> GetRandomImageWithKey()
        {
            List<Texture2D> data;
            string name = imageDictionary.ElementAt(UnityEngine.Random.Range(0, imageDictionary.Keys.Count)).Key;
            data = imageDictionary[name];
           
            Texture2D image;
         
            if (data == null)
                Debug.LogError($"Error getting a random image");
            if (data.Count > 1)
                image = data[UnityEngine.Random.Range(0, data.Count)];
            else
            {
                image = data[0];
            }




            return new Tuple<Texture2D, string>(image, name);
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
