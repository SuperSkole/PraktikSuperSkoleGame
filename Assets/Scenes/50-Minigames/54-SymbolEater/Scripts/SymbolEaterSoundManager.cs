using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._50_Minigames._54_SymbolEater.Scripts
{

    public class SymbolEaterSoundController : MonoBehaviour
    {


        private AudioClip letterSoundClip;

        private bool keydown = false;


        /// <summary>
        /// so we can update letterSoundClip via other scipts
        /// </summary>
        /// <param name="clip"></param>
        public void SetSymbolEaterSound(AudioClip clip)
        {
            letterSoundClip = clip;

        }


        void Update()
        {

            //plays the audioLetterSource once by pressing space
            if (Input.GetKeyDown(KeyCode.Space) && keydown == false)
            {
                
                AudioManager.Instance.PlaySound(letterSoundClip, SoundType.Voice);
                keydown = true;
            }

            if (Input.GetKeyUp(KeyCode.Space) && keydown == true)
            {
                keydown = false;
            }

        }
    }
}