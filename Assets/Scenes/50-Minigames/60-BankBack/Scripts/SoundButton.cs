using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code for the sound buttons in the bank back entrance minigame
/// </summary>
public class SoundButton : MonoBehaviour
{
    [SerializeField]private AudioClip audioClip;
    public AudioSource audioSource;
    public int amount;

    /// <summary>
    /// Onclick method for the button. starts a coroutine to play the audioclip a number of times equal to amount.
    /// </summary>
    public void OnClick()
    {
        StartCoroutine(PlaySound());
    }

    /// <summary>
    /// Plays a sound waits until it is done and then plays it again until it has been done an amount of times equal to the amount variable.
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlaySound()
    {
        for(int i = 0; i < amount; i++)
        {
            audioSource.PlayOneShot(audioClip);
            yield return new WaitUntil(()=> !audioSource.isPlaying);
        }
        
    }

}
