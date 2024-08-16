using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrovÆderSoundController : MonoBehaviour
{

    [SerializeField]
    private AudioSource audioLetterSource;

    
    private AudioClip letterSoundClip;

    private bool keydown = false;


    //so we can update letterSoundClip via other scipts
    public void SetGrovæderSound(AudioClip clip)
    {
        letterSoundClip = clip;
        
    }

    // Update is called once per frame
    void Update()
    {

        //plays the audioLetterSource once by pressing space
        if (Input.GetKeyDown(KeyCode.Space) && keydown == false)
        {
            keydown = true;
            
            audioLetterSource.PlayOneShot(letterSoundClip);
        }

        if (Input.GetKeyUp(KeyCode.Space) && keydown == true)
        {
            keydown = false;
        }

    }
}
