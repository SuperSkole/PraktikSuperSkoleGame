using CORE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.Analytics;


public class KatjaFe : PersistentSingleton<SceneLoader>
{
    [SerializeField] SkeletonGraphic katjaFeSkeleton;

    //Stuff given
    AudioSource audioSource;
    Coroutine currentCoroutine;


    public void KatjaSpeak(AudioClip audioClip, System.Action onComplete)
    {
        Clear();

        //Play the sound
        audioSource.clip = audioClip;
        audioSource.Play();


        //Set the works in go
        katjaFeSkeleton.AnimationState.SetAnimation(0, "Speaking", true);

        currentCoroutine = StartCoroutine(WaitForAudio(onComplete));
    }

    public void KatjaExit(System.Action onComplete = null)
    {
        Clear();

        katjaFeSkeleton.AnimationState.SetAnimation(0, "Exit", false);

        float animationDuration = katjaFeSkeleton.AnimationState.GetCurrent(0).Animation.Duration;

        currentCoroutine = StartCoroutine(WaitForAnimationToEnd(animationDuration, onComplete, false));


    }

    public void KatjaIntro(System.Action onComplete)
    {
        Clear();

        //Animation go
        katjaFeSkeleton.gameObject.SetActive(true);
        katjaFeSkeleton.AnimationState.SetAnimation(0, "Intro", false);

        float animationDuration = katjaFeSkeleton.AnimationState.GetCurrent(0).Animation.Duration;
        currentCoroutine = StartCoroutine(WaitForAnimationToEnd(animationDuration,onComplete,true));

    }

    //clear the variables
    private void Clear()
    {
        audioSource = null;
        currentCoroutine = null;
        katjaFeSkeleton.AnimationState.ClearTrack(0);
    }

    //---IEnumerators---//
    private IEnumerator WaitForAudio(System.Action onComplete)
    {
        yield return new WaitForSeconds(audioSource.clip.length);

        //Once the audio is done
        katjaFeSkeleton.AnimationState.ClearTrack(0);
        katjaFeSkeleton.AnimationState.SetAnimation(0, "Idle", true);

        if (onComplete != null)
        {
            onComplete.Invoke();
        }
    }

    private IEnumerator WaitForAnimationToEnd(float duration, System.Action onComplete, bool state)
    {
        yield return new WaitForSeconds(duration);


        katjaFeSkeleton.AnimationState.ClearTrack(0);

        //check if it's a end or continues animation
        if (state)
        {
            katjaFeSkeleton.AnimationState.SetAnimation(0, "Idle", true);
        }
        if(!state)
        {
            katjaFeSkeleton.gameObject.SetActive(false);
        }


        //invoke the callback
        if (onComplete != null)
        {
            onComplete.Invoke();
        }
    }
}
