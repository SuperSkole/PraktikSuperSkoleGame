using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GernalSoundMangement : MonoBehaviour
{
    public AudioSource src;
    public List<AudioClip> TalkingSoundEffects;
    public List<AudioClip> clip;

    public bool StillSpeaking = false;

    public void CallIE()
    {
        StartCoroutine(CallSpeakingSound());
    }
    IEnumerator CallSpeakingSound()
    {
        StillSpeaking=true;
        while (StillSpeaking)
        {
            src.Stop();
            int RandomI = Random.Range(0, TalkingSoundEffects.Count);
            src.clip = TalkingSoundEffects[RandomI];
            src.Play();
            yield return new WaitForSeconds(0.2f);
            while (src.isPlaying)
            {
                yield return null;
            }
        }
    }
}

