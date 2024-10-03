using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    SFX,
    Music,
    Voice
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioMixer m_Mixer;
    private GameObject mCamara;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        if(mCamara == null)
            mCamara = FindAnyObjectByType<AudioListener>().gameObject;
    }


    public void PlaySound(AudioClip audioClip, SoundType audioType)
    {
        AudioSource source = mCamara.AddComponent<AudioSource>();
        source.clip = audioClip;
        switch(audioType)
        {
            case SoundType.SFX:
                source.outputAudioMixerGroup = m_Mixer.outputAudioMixerGroup;
                break;
            case SoundType.Music:

                break;
            case SoundType.Voice:

                break;
        }
    }
}
