using System.Collections;
using CORE;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.Managers
{
    public class WordFactorySoundManager : PersistentSingleton<WordFactorySoundManager>
    {
        public enum SoundEvent
        {
            RotateGear,
            GainXP,
            GainGold,
            PronounceLetter,
            CheckWord,
            PullHandle
        }
        
        [SerializeField] private AudioClip gearRotationSound;
        [SerializeField] private AudioClip xpGainSound;
        [SerializeField] private AudioClip goldGainSound;
        [SerializeField] private AudioClip letterPronounceSound;
        [SerializeField] private AudioClip wordCheckSound;
        [SerializeField] private AudioClip handlePullSound;

        private AudioSource audioSource;

        private bool playingHandleSound = false;

        // // Singleton
        // public static WordFactorySoundManager Instance { get; private set; }
        //
        // void Awake()
        // {
        //     if (Instance != null && Instance != this)
        //     {
        //         Destroy(gameObject);
        //     }
        //     else
        //     {
        //         Instance = this;
        //         //DontDestroyOnLoad(gameObject);
        //     }
        // }
        


        IEnumerator PlayHandleSound()
        {
            playingHandleSound = true;
            AudioManager.Instance.PlaySound(handlePullSound, SoundType.SFX, transform.position);
            
            yield return new WaitForSeconds(handlePullSound.length);
            playingHandleSound = false;
        }

        // Method to play sound based on the event name
        public void PlaySound(SoundEvent soundEvent)
        {
            switch (soundEvent)
            {
                case SoundEvent.RotateGear:
                    AudioManager.Instance.PlaySound(gearRotationSound, SoundType.SFX, transform.position);
                    break;
                case SoundEvent.GainXP:
                    AudioManager.Instance.PlaySound(xpGainSound, SoundType.SFX, transform.position);
                    break;
                case SoundEvent.GainGold:
                    AudioManager.Instance.PlaySound(goldGainSound, SoundType.SFX, transform.position);
                    break;
                case SoundEvent.PronounceLetter:
                    AudioManager.Instance.PlaySound(letterPronounceSound, SoundType.SFX, transform.position);
                    break;
                case SoundEvent.CheckWord:
                    AudioManager.Instance.PlaySound(wordCheckSound, SoundType.SFX, transform.position);
                    break;
                case SoundEvent.PullHandle:
                    if (!playingHandleSound)
                    {
                        StartCoroutine(PlayHandleSound());
                        
                    }
                    
                    break;
            }
        }
    }
}