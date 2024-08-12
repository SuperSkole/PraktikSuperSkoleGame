using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grov∆derSoundController : MonoBehaviour
{

    [SerializeField]
    private AudioSource audioLetterSource;

    
    private AudioClip letterSoundClip;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            audioLetterSource.PlayOneShot(letterSoundClip);
        }
    }
}
