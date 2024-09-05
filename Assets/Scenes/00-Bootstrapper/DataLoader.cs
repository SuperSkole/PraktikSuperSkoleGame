using System.Collections;
using System.Collections.Generic;
using System.Text;
using CORE.Scripts;
using UnityEngine;
using UnityEngine.Networking;

namespace Scenes._00_Bootstrapper
{
    /// <summary>
    /// Load Words From CSV
    /// Run during login screen
    /// </summary>
    public class DataLoader : MonoBehaviour
    {
        public static bool IsDataLoaded { get; private set; } = false;

        public List<Texture2D> images = new();
        public List<AudioClip> letterSounds = new();
        public List<AudioClip> danskCongrats = new();
        public List<AudioClip> englishCongrats = new();
        public List<AudioClip> explosionSounds = new();


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
            var congratsSoundCoroutine = LoadAllCongratsSounds();
            var soundFxCourutine = LoadAllSoundFX();

            yield return StartCoroutine(csvCoroutine);
            yield return StartCoroutine(texturesCoroutine);
            yield return StartCoroutine(letterSoundsCoroutine);
            yield return StartCoroutine(congratsSoundCoroutine);
            yield return StartCoroutine(soundFxCourutine);

            Debug.Log("All resources loaded.");
        }

        private IEnumerator LoadAllCsvFiles()
        {
            IsDataLoaded = true;
            string directoryPath = Application.streamingAssetsPath + "/WordData/";
            string configFilePath = directoryPath + "files.txt"; 

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

            //Debug.Log($"The File {filePath} was loaded successfully with words added to set \"{setName}\"");
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
            
            foreach (Texture2D fileName in images)
            {
                yield return StartCoroutine(LoadAndSetDic(fileName));
            }
        }

        private IEnumerator LoadAndSetDic(Texture2D texture)
        {
            string name = texture.name;
            name = GetName(name);
            ImageManager.AddImageToSet(name, texture);
            WordsForImagesManager.AddNameToSet(name);
            yield return null;
        }

        private string GetName(string name)
        {
            StringBuilder output = new(name);
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
            foreach (AudioClip fileName in letterSounds)
            {
                yield return StartCoroutine(LoadAndSetDicLetterSound(fileName));
            }
        }

        private IEnumerator LoadAndSetDicLetterSound(AudioClip clip)
        {
            string name = clip.name;
            name = GetNameLetterSound(name);
            LetterAudioManager.AddAudioClipToSet(name, clip);
            yield return null;
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

        /// <summary>
        /// Loads all the congrats Sounds. 
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadAllCongratsSounds()
        {
            foreach (AudioClip clip in danskCongrats)
            {
                StartCoroutine(LoadAndSetListDanishCongratsSounds(clip));
            }

            foreach (AudioClip clip in englishCongrats)
            {
                StartCoroutine(LoadAndSetListEnglishCongratsSounds(clip));
            }

            yield return null;
        }



        /// <summary>
        /// loades the danish congrats AudioClips based on a filepath and calls the CongratsAudioManager to add it to the danishPraiselist of audioClips.
        /// </summary>
        /// <param name="clip">the path of the file you are loading</param>
        /// <returns></returns>
        IEnumerator LoadAndSetListDanishCongratsSounds(AudioClip clip)
        {
            CongratsAudioManager.AddAudioClipToDanishSet(clip);
            yield return null;
        }

        /// <summary>
        /// loades the danish congrats AudioClips based on a filePath and calls the CongratsAudioManager to add it to the danishPraiselist of audioClips.
        /// </summary>
        /// <param name="clip"></param>
        /// <returns></returns>
        IEnumerator LoadAndSetListEnglishCongratsSounds(AudioClip clip)
        {
            CongratsAudioManager.AddAudioClipToEnglishSet(clip);
            yield return null;
        }




        /// <summary>
        /// Loads all the soundFX. 
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadAllSoundFX()
        {
            foreach (AudioClip clip in explosionSounds)
            {
                StartCoroutine(LoadAndSetListExplosionSounds(clip));
            }


            yield return null;
        }



        /// <summary>
        /// Loads and sets the explosionsound. 
        /// </summary>
        /// <param name="clip">the path of the file you are loading</param>
        /// <returns></returns>
        IEnumerator LoadAndSetListExplosionSounds(AudioClip clip)
        {
            SoundFXManager.AddAudioClipToExplosionsList(clip);
            yield return null;
        }


    }
}
