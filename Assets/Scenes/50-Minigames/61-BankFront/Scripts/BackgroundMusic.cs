using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField]private List<AudioClip> musicFiles;
    [SerializeField]private AudioSource musicPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(!musicPlayer.isPlaying)
        {
            musicPlayer.PlayOneShot(musicFiles[Random.Range(0, musicFiles.Count)]);
        }
    }
}
