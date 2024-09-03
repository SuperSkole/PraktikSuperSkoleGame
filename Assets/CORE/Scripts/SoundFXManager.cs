using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace CORE.Scripts
{
    /// <summary>
    /// Class containing static methods that can manipulate lists that contain audioclips that praise the player for doing well. 
    /// </summary>
    public class SoundFXManager : MonoBehaviour
    {
        private static List<AudioClip> Explosions = new List<AudioClip>();

   
        public static bool IsDataLoaded { get; private set; } = false;



        /// <summary>
        /// Adds a audio clip to a Explosion list containing all the Explosion sounds
        /// </summary>
        /// <param name="input"></param>
        public static void AddAudioClipToExplosionsList(AudioClip input)
        {
            Explosions.Add(input);
            IsDataLoaded = true;

        }


        /// <summary>
        /// Get an audio clip from the Explosion list containing all the Explosion audio clips. 
        /// </summary>
        /// <param name="input"></param>
        public static AudioClip GetAudioClipFromExplosionsList(int index)
        {
            return Explosions[index];
        }



    }
}
