using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrovÆderSoundController : MonoBehaviour
{

    [SerializeField]
    private AudioSource audioLetterSource;

    
    private AudioClip letterSoundClip;

    public void SetGrovæderSound(AudioClip clip)
    {
        letterSoundClip = clip;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            audioLetterSource.PlayOneShot(letterSoundClip);
        }
    }
}
