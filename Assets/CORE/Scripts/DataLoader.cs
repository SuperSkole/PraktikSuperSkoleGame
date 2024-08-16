using System.Collections;
using System.IO;
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
    }
}
