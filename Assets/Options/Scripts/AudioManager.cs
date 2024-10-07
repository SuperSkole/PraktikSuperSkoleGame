using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
    [SerializeField] private AudioMixerGroup sfx, music, voice;
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
                break;
        }
        source.Play();
        Destroy(source,audioClip.length);
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
                break;
        }
        source.Play();
        Destroy(go, audioClip.length);
    }
}
