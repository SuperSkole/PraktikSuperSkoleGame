using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CORE.Scripts
{


    public class LetterAudioManager
    {
        private static Dictionary<string, List<AudioClip>> letterDictionary = new();
        public static bool IsDataLoaded { get; private set; } = false;

        public static void AddAudioClipToSet(string name, AudioClip input)
        {
            if (letterDictionary.ContainsKey(name.ToLower()))
            {
                letterDictionary[name.ToLower()].Add(input);
            }
            else
            {
                letterDictionary.Add(name.ToLower(), new List<AudioClip>());
                letterDictionary[name.ToLower()].Add(input);
            }
            IsDataLoaded = true;
            
        }

        public static AudioClip GetAudioClipFromLetter(string inputLetter)
        {
            if (!letterDictionary.TryGetValue(inputLetter.ToLower(), out List<AudioClip> data))
                data = null;
            AudioClip audioClip;
            if (data == null)
            {
                Debug.LogError($"Error getting audio for the word: {inputLetter}");
            }
            if (data.Count > 1)
                audioClip = data[UnityEngine.Random.Range(0, data.Count)];
            else
                audioClip = data[0];

            return audioClip;
        }
    }

}