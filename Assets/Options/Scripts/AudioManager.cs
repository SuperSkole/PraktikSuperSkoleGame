using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject muteIcon;
    [SerializeField] private GameObject menu;
    private bool muted = false;
    private float savedmasterVolume = 0f;


    public void SetMasterVolume(float volume)
    {
        savedmasterVolume = volume;
        if(muted) return;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
    }

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

    public void Back()
    {
        menu.SetActive(false);
    }
}
