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
        
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Method to play sound based on the event name
        public void PlaySound(SoundEvent soundEvent)
        {
            switch (soundEvent)
            {
                case SoundEvent.RotateGear:
                    audioSource.PlayOneShot(gearRotationSound);
                    break;
                case SoundEvent.GainXP:
                    audioSource.PlayOneShot(xpGainSound);
                    break;
                case SoundEvent.GainGold:
                    audioSource.PlayOneShot(goldGainSound);
                    break;
                case SoundEvent.PronounceLetter:
                    audioSource.PlayOneShot(letterPronounceSound);
                    break;
                case SoundEvent.CheckWord:
                    audioSource.PlayOneShot(wordCheckSound);
                    break;
                case SoundEvent.PullHandle:
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(handlePullSound);
                    }

                    break;
            }
        }
    }
}