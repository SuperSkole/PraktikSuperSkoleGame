using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioVolumeManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject muteIcon;
    [SerializeField] private GameObject menu;
    private bool muted = false;
    private float savedmasterVolume = 0f;

    /// <summary>
    /// used to set the master volume
    /// </summary>
    /// <param name="volume">a float between 0 and 1 that represents the volume</param>
    public void SetMasterVolume(float volume)
    {
        savedmasterVolume = volume;
        if(muted) return;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
    }

    /// <summary>
    /// used to set the SFX volume
    /// </summary>
    /// <param name="volume">a float between 0 and 1 that represents the volume</param>
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20f);
    }

    /// <summary>
    /// used to set the Music volume
    /// </summary>
    /// <param name="volume">a float between 0 and 1 that represents the volume</param>
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
    }

    /// <summary>
    /// used to set the Voice volume
    /// </summary>
    /// <param name="volume">a float between 0 and 1 that represents the volume</param>
    public void SetVoiceVolume(float volume)
    {
        audioMixer.SetFloat("VoiceVolume", Mathf.Log10(volume) * 20f);
    }

    /// <summary>
    /// used to mute and unmute the game. toggles between the 2.
    /// </summary>
    public void Mute()
    {
        muted = !muted;
        if (muted)
        {
            audioMixer.SetFloat("MasterVolume", -80);
            muteIcon.SetActive(true);
        }
        else
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(savedmasterVolume) * 20f);
            muteIcon.SetActive(false);
        }
    }

    /// <summary>
    /// used to go back from the audio menu
    /// </summary>
    public void Back()
    {
        menu.SetActive(false);
    }
}
