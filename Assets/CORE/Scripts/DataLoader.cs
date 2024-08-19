using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace CORE.Scripts
{
    /// <summary>
    /// Load Words From CSV
    /// Run during login screen
    /// </summary>
    public class DataLoader : MonoBehaviour
    {
        public static bool IsDataLoaded { get; private set; } = false;
        
        public void Start()
        {
            StartCoroutine(LoadAllCsvFiles());
            StartCoroutine(LoadAllTextures());
        }
        
        private IEnumerator LoadAllCsvFiles()
        {
            IsDataLoaded = true;
            string directoryPath = Path.Combine(Application.streamingAssetsPath, "WordData");

            // Get all CSV files in the directory
            string[] fileEntries = Directory.GetFiles(directoryPath, "*.csv");
            foreach (string filePath in fileEntries)
            {
                yield return StartCoroutine(LoadWordsFromCsvFile(filePath));
            }
        }

        private IEnumerator LoadWordsFromCsvFile(string filePath)
        {
            // use unitywebrequest and get the data from the path
            using (UnityWebRequest request = UnityWebRequest.Get(filePath))
            {
                yield return request.SendWebRequest();
                // Early out if the request failed.
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error loading {filePath}:" + request.error);
                    yield break;
                }

                // Store words here temp. or send directly to LettersAndWordsManager
                AddWordsToHashsetInLettersAndWordsManager(filePath, request);
            }
        }

        private static void AddWordsToHashsetInLettersAndWordsManager(string filePath, UnityWebRequest request)
        {
            string csvData = request.downloadHandler.text;
            string[] lines = csvData.Split('\n');
            string setName = Path.GetFileNameWithoutExtension(filePath);

            // Start from index 1 to skip the header
            for (int i = 1; i < lines.Length; i++)
            {
                string word = lines[i].Trim();
                if (!string.IsNullOrWhiteSpace(word))
                {
                    WordsManager.AddWordToSet(setName, word);
                }
            }

            Debug.Log($"The File {filePath} was loaded successfully with words added to set \"{setName}\"");
        }

        #region loadTextures


        /// <summary>
        /// loads all the images in the pictures folder in the streamingAssest folder.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadAllTextures()
        {
            string directoryPath = Path.Combine(Application.streamingAssetsPath, "Pictures");

            // Get all PNG files in the directory
            string[] fileEntries = System.IO.Directory.GetFiles(directoryPath, "*.png");
            foreach (string filePath in fileEntries)
            {
                StartCoroutine(LoadAndSetDic(filePath));
            }
            yield return null;
        }


        /// <summary>
        /// loades the textures and calles the ImageManager to add it.
        /// </summary>
        /// <param name="filePath">the path of the file you are loading</param>
        /// <returns></returns>
        IEnumerator LoadAndSetDic(string filePath)
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
                ImageManager.AddImageToSet(setName, texture);
                WordsForImagesManager.AddNameToSet(setName);
            }
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
            if (space != -1)
                output.Remove(space, output.Length - space);
            else if (index != -1)
                output.Remove(index, output.Length - index);
            return output.ToString();
        }

        #endregion
    }
}
