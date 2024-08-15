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
using UnityEngine.UI;
using UnityEngine.Windows;

namespace CORE.Scripts
{

    public class ImageManager : MonoBehaviour
    {

        static Dictionary<string, List<Texture2D>> imageDictionary = new();
        public static bool IsDataLoaded { get; private set; } = false;

        private void Start()
        {
            StartCoroutine(LoadAllTextures());
        }

        #region loadTexturesAndSetupDic


        /// <summary>
        /// loads all the images in the pictures folder in the streamingAssest folder, and safes it in an dictionary.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadAllTextures()
        {
            //IsDataLoaded = true;
            string directoryPath = Path.Combine(Application.streamingAssetsPath, "Pictures");

            // Get all CSV files in the directory
            string[] fileEntries = System.IO.Directory.GetFiles(directoryPath, "*.png");
            foreach (string filePath in fileEntries)
            {
                UnityWebRequest request = UnityWebRequestTexture.GetTexture(filePath);
                string setName = Path.GetFileNameWithoutExtension(filePath);
                setName = GetName(setName);
                yield return request.SendWebRequest();
                // Early out if the request failed.
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error loading {filePath}:" + request.error);
                    yield break;
                }
                else
                {
                    // Get downloaded asset bundle
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    if (imageDictionary.ContainsKey(setName))
                    {
                        imageDictionary[setName].Add(texture);
                    }
                    else
                    {
                        imageDictionary.Add(setName, new List<Texture2D>());
                        imageDictionary[setName].Add(texture);
                    }
                }
            }
            IsDataLoaded = true;
        }


        /// <summary>
        /// removes numbers and extentions of names so they can be combined in the dic.
        /// </summary>
        /// <param name="name">the name that needs to be "fixed"</param>
        /// <returns>a fixed vertion of the name</returns>
        string GetName(string name)
        {
            StringBuilder output = new();
            output.Append(name);
            int index = output.ToString().LastIndexOf('.');
            int space = output.ToString().LastIndexOf(" ");
            if(space != -1)
                output.Remove(space, output.Length - space);
            else if(index != -1)
                output.Remove(index, output.Length - index);
            return output.ToString();
        }

        #endregion



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
