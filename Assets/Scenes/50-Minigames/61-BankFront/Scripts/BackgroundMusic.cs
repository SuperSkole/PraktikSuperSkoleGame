using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField]private List<AudioClip> musicFiles;
    /// <summary>
    /// Starts playing the background music
    /// </summary>
    void Start()
    {
        StartCoroutine(PlayMusic());
    }
    
    /// <summary>
    /// Plays the background music. waits some time equivelent to the length of the current track and afterwards restarts
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayMusic(){
        AudioClip music = musicFiles[Random.Range(0, musicFiles.Count)];
        AudioManager.Instance.PlaySound(music, SoundType.Music);
        yield return new WaitForSeconds(music.length);
        PlayMusic();
    }
}
