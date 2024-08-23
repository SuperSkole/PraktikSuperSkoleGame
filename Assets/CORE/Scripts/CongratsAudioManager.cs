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

        public static void AddAudioClipToDanishSet(AudioClip input)
        {
            danishPraiseList.Add(input);
            IsDataLoaded = true;

        }

        public static void AddAudioClipToEnglishSet(AudioClip input)
        {
            englishPraiseList.Add(input);
            IsDataLoaded = true;

        }


        public static int GetLenghtOfAudioClipDanishList()
        {
            return danishPraiseList.Count;
        }

        public static int GetLenghtOfAudioClipEnglishList()
        {
            return englishPraiseList.Count;
        }



        public static AudioClip GetAudioClipFromDanishSet(int index)
        {
            return danishPraiseList[index];
        }

        public static AudioClip GetAudioClipFromEnglishSet(int index)
        {
            return englishPraiseList[index];
        }




    }
}
