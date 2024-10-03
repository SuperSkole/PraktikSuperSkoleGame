using System.Collections;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;


public class KatjaFe : MonoBehaviour
{
    //The prefab's assets
    [SerializeField] SkeletonGraphic katjaFeSkeleton;
    [SerializeField] Image offButton;

    //Stuff given
    [SerializeField] AudioSource audioSource;
    Coroutine currentCoroutine;

    //Stuff saved
    AudioClip oldaudio;

    //tutorial or tip
    public bool tutorial;

    public void Initialize(bool type, AudioClip audio)
    {
        audioSource = GetComponent<AudioSource>();

        //if tutorial
        if (type)
        {
            offButton.gameObject.SetActive(true);
            oldaudio = audio;
            
        }
        //if not, but only tip
        else
        {
            oldaudio = audio;
            CheckIfThereIsAudio();
        }

    }

    public void KatjaSpeak(AudioClip thisAudioClip, System.Action onComplete)
    {
        Clear();
        if(thisAudioClip != null)
        {
            Debug.Log("There is a audioclip");
        }

        //Play the sound
        audioSource.clip = thisAudioClip;
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

        Debug.Log("intro");

        //Set the button off
        if (offButton != null)
        {
            offButton.gameObject.SetActive(true);
        }

        //Animation go
        katjaFeSkeleton.gameObject.SetActive(true);
        katjaFeSkeleton.AnimationState.SetAnimation(0, "Intro", false);

        float animationDuration = katjaFeSkeleton.AnimationState.GetCurrent(0).Animation.Duration;
        currentCoroutine = StartCoroutine(WaitForAnimationToEnd(animationDuration,onComplete,true));

    }

    //clear the variables
    private void Clear()
    {
        if(currentCoroutine != null)
        {
        StopCoroutine(currentCoroutine);
        }

        katjaFeSkeleton.AnimationState.ClearTrack(0);
        currentCoroutine = null;
    }


    //---IEnumerators---//
    private IEnumerator WaitForAudio(System.Action onComplete)
    {
        yield return new WaitForSeconds(audioSource.clip.length);

        //Once the audio is done
        katjaFeSkeleton.AnimationState.SetAnimation(0, "Idle", true).MixDuration = 0.2f;

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
            katjaFeSkeleton.AnimationState.SetAnimation(0, "Idle", true).MixDuration = 0.2f;
        }
        if(!state)
        {
            katjaFeSkeleton.gameObject.SetActive(false);
            CheckIfThereIsAudio();
        }


        //invoke the callback
        if (onComplete != null)
        {
            onComplete.Invoke();
        }
    }

    //INFO BUTTON

    private void CheckIfThereIsAudio()
    {
        if (oldaudio != null)
        {
            offButton.gameObject.SetActive(false);
        }
    }


    public void Click()
    {
        //Example
        KatjaIntro(() => { KatjaSpeak(oldaudio, () => {KatjaExit();});});
    }
}
