using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public enum SoundType
{
    SFX,
    Music,
    Voice
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private GameObject mCamara;
    private AudioVolumeManager mVolumeManager;
    [SerializeField] private AudioMixerGroup sfx, music, voice;
    [SerializeField] private AudioMixer mixer;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Correctness", "UNT0008:Null propagation on Unity objects", Justification = "<Pending>")]
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        GetAudioListerner(SceneManager.GetActiveScene(),LoadSceneMode.Single);
        SceneManager.sceneLoaded += GetAudioListerner;
    }

    private void GetAudioListerner(Scene scene, LoadSceneMode mode)
    {
        if (mCamara == null)
            mCamara = FindAnyObjectByType<AudioListener>()?.gameObject;
        if (mCamara == null)
        {
            mCamara = Instantiate(new GameObject());
            mCamara.AddComponent<AudioListener>();
        }
        mVolumeManager = FindAnyObjectByType<AudioVolumeManager>(FindObjectsInactive.Include);
    }

    /// <summary>
    /// Playes a given audio clip at the sceans audio listener.
    /// </summary>
    /// <param name="audioClip">the clip you want to play.</param>
    /// <param name="audioType">what type of audio you are playing.</param>
    public void PlaySound(AudioClip audioClip, SoundType audioType)
    {
        AudioSource source = mCamara.AddComponent<AudioSource>();
        source.clip = audioClip;
        switch(audioType)
        {
            case SoundType.SFX:
                source.outputAudioMixerGroup = sfx;
                break;
            case SoundType.Music:
                source.outputAudioMixerGroup = music;
                break;
            case SoundType.Voice:
                source.outputAudioMixerGroup = voice;
                StartCoroutine(PlayingVoice(audioClip.length));
                break;
        }
        source.Play();
        Destroy(source,audioClip.length);
    }

    /// <summary>
    /// Playes a given audio clip at the sceans audio listener.
    /// </summary>
    /// <param name="audioClip">the clip you want to play.</param>
    /// <param name="audioType">what type of audio you are playing.</param>
    /// <param name="loop">if the clip suld loop or not</param>
    public void PlaySound(AudioClip audioClip, SoundType audioType, bool loop)
    {
        AudioSource source = mCamara.AddComponent<AudioSource>();
        source.clip = audioClip;
        switch (audioType)
        {
            case SoundType.SFX:
                source.outputAudioMixerGroup = sfx;
                break;
            case SoundType.Music:
                source.outputAudioMixerGroup = music;
                break;
            case SoundType.Voice:
                source.outputAudioMixerGroup = voice;
                StartCoroutine(PlayingVoice(audioClip.length));
                break;
        }
        source.loop = loop;
        source.Play();
        if(!loop)
            Destroy(source, audioClip.length);
    }

    /// <summary>
    /// Playes a given audio clip at the given point.
    /// </summary>
    /// <param name="audioClip">the clip you want to play.</param>
    /// <param name="audioType">what type of audio you are playing.</param>
    /// <param name="point">the point you want the audio played at.</param>
    public void PlaySound(AudioClip audioClip, SoundType audioType,Vector3 point)
    {
        GameObject go = Instantiate(new GameObject(), point, Quaternion.identity);
        DontDestroyOnLoad(go);
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = audioClip;
        switch (audioType)
        {
            case SoundType.SFX:
                source.outputAudioMixerGroup = sfx;
                break;
            case SoundType.Music:
                source.outputAudioMixerGroup = music;
                break;
            case SoundType.Voice:
                source.outputAudioMixerGroup = voice;
                StartCoroutine(PlayingVoice(audioClip.length));
                break;
        }
        source.Play();
        Destroy(go, audioClip.length);
    }

    /// <summary>
    /// Playes a given audio clip at the given point.
    /// </summary>
    /// <param name="audioClip">the clip you want to play.</param>
    /// <param name="audioType">what type of audio you are playing.</param> 
    /// <param name="loop">if the clip suld loop or not</param>
    /// <param name="point">the point you want the audio played at.</param>
    public void PlaySound(AudioClip audioClip, SoundType audioType, bool loop, Vector3 point)
    {
        GameObject go = Instantiate(new GameObject(), point, Quaternion.identity);
        DontDestroyOnLoad(go);
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = audioClip;
        switch (audioType)
        {
            case SoundType.SFX:
                source.outputAudioMixerGroup = sfx;
                break;
            case SoundType.Music:
                source.outputAudioMixerGroup = music;
                break;
            case SoundType.Voice:
                source.outputAudioMixerGroup = voice;
                StartCoroutine(PlayingVoice(audioClip.length));
                break;
        }
        source.loop = loop;
        source.Play();
        if(!loop)
            Destroy(go, audioClip.length);
    }

    private IEnumerator PlayingVoice(float time)
    {
        if (mVolumeManager != null)
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(mVolumeManager.savedSFXVolume / 2) * 20f);
            mixer.SetFloat("MusicVolume", Mathf.Log10(mVolumeManager.savedMusicVolume / 2) * 20f);
        }
        else
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(1 / 2) * 20f);
            mixer.SetFloat("MusicVolume", Mathf.Log10(1 / 2) * 20f);
        }
        yield return new WaitForSeconds(time);
        if (mVolumeManager != null)
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(mVolumeManager.savedSFXVolume) * 20f);
            mixer.SetFloat("MusicVolume", Mathf.Log10(mVolumeManager.savedMusicVolume) * 20f);
        }
        else
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(1) * 20f);
            mixer.SetFloat("MusicVolume", Mathf.Log10(1) * 20f);
        }
    }
}
