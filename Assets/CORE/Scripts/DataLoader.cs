using System.Collections;
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

        /// <summary>
        /// Starts loading all CSV, texture, and letter sound files simultaneously.
        /// </summary>
        public void Start()
        {
            StartCoroutine(LoadAllResources());
        }

        /// <summary>
        /// Coroutine to load CSV files, textures, and letter sounds concurrently.
        /// </summary>
        private IEnumerator LoadAllResources()
        {
            var csvCoroutine = LoadAllCsvFiles();
            var texturesCoroutine = LoadAllTextures();
            var letterSoundsCoroutine = LoadAllletterSounds();

            yield return StartCoroutine(csvCoroutine);
            yield return StartCoroutine(texturesCoroutine);
            yield return StartCoroutine(letterSoundsCoroutine);

            Debug.Log("All resources loaded.");
        }

        private IEnumerator LoadAllCsvFiles()
        {
            IsDataLoaded = true;
            string directoryPath = Application.streamingAssetsPath + "/WordData/";

            // List your CSV files here
            string[] csvFiles = new string[]
            {
                "Words_Danish_2L.csv",
                "Words_Danish_3L_Combination.csv",
                "Words_Danish_3L_Easy.csv",
                "Words_Danish_3L_Hard.csv",
            };
                
            foreach (string fileName in csvFiles)
            {
                string filePath = directoryPath + fileName;
                yield return StartCoroutine(LoadWordsFromCsvFile(filePath));
            }
        }

        private IEnumerator LoadWordsFromCsvFile(string filePath)
        {
            // Load CSV file from StreamingAssets using UnityWebRequest
            using (UnityWebRequest request = UnityWebRequest.Get(filePath))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error loading {filePath}: " + request.error);
                    yield break;
                }

                AddWordsToHashsetInLettersAndWordsManager(filePath, request);
            }
        }

        private static void AddWordsToHashsetInLettersAndWordsManager(string filePath, UnityWebRequest request)
        {
            string csvData = request.downloadHandler.text;
            string[] lines = csvData.Split('\n');
            string setName = GetFileNameWithoutExtension(filePath);

            for (int i = 1; i < lines.Length; i++) // Skip header
            {
                string word = lines[i].Trim();
                if (!string.IsNullOrWhiteSpace(word))
                {
                    WordsManager.AddWordToSet(setName, word);
                }
            }

            Debug.Log($"The File {filePath} was loaded successfully with words added to set \"{setName}\"");
        }

        private static string GetFileNameWithoutExtension(string filePath)
        {
            int lastSlash = filePath.LastIndexOf('/');
            string fileName = (lastSlash >= 0) ? filePath.Substring(lastSlash + 1) : filePath;
            int extensionIndex = fileName.LastIndexOf('.');
            return (extensionIndex >= 0) ? fileName.Substring(0, extensionIndex) : fileName;
        }

        #region Load Textures

        private IEnumerator LoadAllTextures()
        {
            string directoryPath = Application.streamingAssetsPath + "/Pictures/";

            // List your PNG files here
            string[] textureFiles = new string[] { "image1.png", "image2.png" }; // Replace with your actual texture files
            foreach (string fileName in textureFiles)
            {
                string filePath = directoryPath + fileName;
                yield return StartCoroutine(LoadAndSetDic(filePath));
            }
        }

        private IEnumerator LoadAndSetDic(string filePath)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(filePath);
            string setName = GetFileNameWithoutExtension(filePath);
            setName = GetName(setName);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error loading {filePath}: " + request.error);
                yield break;
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                ImageManager.AddImageToSet(setName, texture);
                WordsForImagesManager.AddNameToSet(setName);
            }
        }

        private string GetName(string name)
        {
            StringBuilder output = new StringBuilder(name);
            int space = output.ToString().IndexOf(" ");
            if (space != -1)
                output.Remove(space, output.Length - space);
            output.Replace("(aa)", "å");
            output.Replace("(ae)", "æ");
            output.Replace("(oe)", "ø");
            return output.ToString();
        }

        #endregion

        #region Load Letter Sounds

        private IEnumerator LoadAllletterSounds()
        {
            string directoryPath = Application.streamingAssetsPath + "/Audio/Letters/";

            // List your letter sound files here
            string[] soundFiles = new string[] { "letter1.mp3", "letter2.mp3" }; // Replace with your actual sound files
            foreach (string fileName in soundFiles)
            {
                string filePath = directoryPath + fileName;
                yield return StartCoroutine(LoadAndSetDicLetterSound(filePath));
            }
        }

        private IEnumerator LoadAndSetDicLetterSound(string filePath)
        {
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG);
            string setName = GetFileNameWithoutExtension(filePath);
            setName = GetNameLetterSound(setName);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error loading {filePath}: " + request.error);
                yield break;
            }
            else
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
                LetterAudioManager.AddAudioClipToSet(setName, audioClip);
            }
        }

        private string GetNameLetterSound(string name)
        {
            StringBuilder output = new StringBuilder(name);
            output.Replace("(aa)", "å");
            output.Replace("(ae)", "æ");
            output.Replace("(oe)", "ø");
            return output.ToString();
        }

        #endregion
    }
}
