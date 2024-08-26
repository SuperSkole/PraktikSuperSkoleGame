using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace CORE.Scripts
{
    /// <summary>
    /// Class containing static methods that can manipulate lists that contain audioclips that praise the player for doing well. 
    /// </summary>
    public class CongratsAudioManager : MonoBehaviour
    {
        private static List<AudioClip> danishPraiseList = new List<AudioClip>();

        private static List<AudioClip> englishPraiseList = new List<AudioClip>();
        public static bool IsDataLoaded { get; private set; } = false;



        /// <summary>
        /// Adds a audio clip to a danishPraiseList list containing all the danish congrats sounds
        /// </summary>
        /// <param name="input"></param>
        public static void AddAudioClipToDanishSet(AudioClip input)
        {
            danishPraiseList.Add(input);
            IsDataLoaded = true;

        }

        /// <summary>
        /// Adds an audio clip to a englishPraiseList containing all the english congrats sounds
        /// </summary>
        /// <param name="input"></param>
        public static void AddAudioClipToEnglishSet(AudioClip input)
        {
            englishPraiseList.Add(input);
            IsDataLoaded = true;

        }

        /// <summary>
        /// Get lenght of the danishPraiseList containing all the danish congrats audio clips. 
        /// </summary>
        /// <param name="input"></param>
        public static int GetLenghtOfAudioClipDanishList()
        {
            return danishPraiseList.Count;
        }


        /// <summary>
        /// Get lenght of the englishPraiseList containing all the english congrats audio clips. 
        /// </summary>
        /// <param name="input"></param>

        public static int GetLenghtOfAudioClipEnglishList()
        {
            return englishPraiseList.Count;
        }


        /// <summary>
        /// Get an audio clip from the danishPraiseList containing all the danish congrats audio clips. 
        /// </summary>
        /// <param name="input"></param>
        public static AudioClip GetAudioClipFromDanishSet(int index)
        {
            return danishPraiseList[index];
        }




        /// <summary>
        /// Get an audio clip from the englisPraiseList containing all the english congrats audio clips. 
        /// </summary>
        /// <param name="input"></param>

        public static AudioClip GetAudioClipFromEnglishSet(int index)
        {
            return englishPraiseList[index];
        }




    }
}
