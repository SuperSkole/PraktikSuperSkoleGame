using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        private void Start()
        {
            StartCoroutine(LoadAllTextures());
        }

        #region loadTexturesAndSetupDic

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
        }

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
            Texture2D image = null;

            List<Texture2D> data = imageDictionary[inputWord];
            if (data.Count > 1)
                image = data[UnityEngine.Random.Range(0,data.Count)];
            else
                image = data[0];
            if(image == null)
            {
                Debug.LogError($"Error getting image for the word: {inputWord.ToLower()}");
            }

            return image;
        }

        /// <summary>
        /// takes in an array of words and reterns an array of corrisponting images.
        /// </summary>
        /// <param name="inputWords">the words you want to get images for</param>
        /// <returns>a UnityEngine.UI image or if it couldent find anny image it returnes NULL</returns>
        public static Texture2D[] GetImageFromWord(string[] inputWords)
        {
            Texture2D[] images = new Texture2D[inputWords.Length];

            for (int i = 0; i < inputWords.Length; i++)
            {
                List<Texture2D> data = imageDictionary[inputWords[i]];
                if (data.Count > 1)
                    images[i] = data[UnityEngine.Random.Range(0, data.Count)];
                else
                    images[i] = data[0];
                if (images[i] == null)
                {
                    Debug.LogError($"Error getting image for the word: {inputWords[i].ToLower()}");
                }
            }

            return images;
        }

        /// <summary>
        /// gets a random image from the libarry.
        /// </summary>
        /// <returns>a random image</returns>
        public static Texture2D GetRandomImage()
        {
            Texture2D image = null;


            return image;
        }

        /// <summary>
        /// gets multibull random images.
        /// </summary>
        /// <param name="amonunt">the amount of images you want</param>
        /// <returns>an array of random images</returns>
        public static Texture2D[] GetRandomImage(int amonunt)
        {
            Texture2D[] image = null;


            return image;
        }
    }


    public struct LoadeImage : IJob
    {
        public FixedString128Bytes path;
        public FixedString64Bytes fileName;
        public Texture2D texture;
        public void Execute()
        {
            //loade data
            byte[] bytes = UnityEngine.Windows.File.ReadAllBytes(path.ToString());
            texture.LoadImage(bytes);
        }
    }
}
