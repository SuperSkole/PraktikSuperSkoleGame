using System.Collections;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;


public class KatjaFe : MonoBehaviour
{
    //The prefab's assets
    [SerializeField] SkeletonGraphic katjaFeSkeleton;
    [SerializeField] Image offButton;

    Coroutine currentCoroutine;

    //Stuff saved
    AudioClip oldaudio;

    //tutorial or tip
    public bool tutorial;
    private bool itIsOut;

    /// <summary>
    /// used to initialize the farry
    /// </summary>
    /// <param name="type">if the farry is a tutorial or not</param>
    /// <param name="audio">the clip you want to play</param>
    public void Initialize(bool type, AudioClip audio)
    {

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

    /// <summary>
    /// playes the audioClip given and playes the talking animation.
    /// </summary>
    /// <param name="thisAudioClip">the audio you want to speak</param>
    /// <param name="onComplete">a callback that is called when the audioclip is done playing</param>
    public void KatjaSpeak(AudioClip thisAudioClip, System.Action onComplete)
    {
        Clear();

        //Play the sound
        AudioManager.Instance.PlaySound(thisAudioClip,SoundType.Voice);

        //Set the works in go
        katjaFeSkeleton.AnimationState.SetAnimation(0, "Speaking", true);

        currentCoroutine = StartCoroutine(WaitForAudio(thisAudioClip.length,onComplete));
    }

    /// <summary>
    /// makes the farry exit.
    /// </summary>
    /// <param name="onComplete">a callback that is called after the exit animation is played</param>
    public void KatjaExit(System.Action onComplete = null)
    {
        if(!itIsOut)
        {
            itIsOut = true;

            Clear();

            katjaFeSkeleton.AnimationState.SetAnimation(0, "Exit", false);

            float animationDuration = katjaFeSkeleton.AnimationState.GetCurrent(0).Animation.Duration;

            currentCoroutine = StartCoroutine(WaitForAnimationToEnd(animationDuration, onComplete, false));
        }
    }

    /// <summary>
    /// makes the farry enter.
    /// </summary>
    /// <param name="onComplete">a callback that is called after the Intro animation is played</param>
    public void KatjaIntro(System.Action onComplete)
    {
        Clear();

        itIsOut = false;

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

    /// <summary>
    /// clears valubles
    /// </summary>
    private void Clear()
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        AudioManager.Instance.StopVoice();

        katjaFeSkeleton.AnimationState.ClearTrack(0);
        currentCoroutine = null;
    }


    /// <summary>
    /// used to wait until a audio is done playing
    /// </summary>
    /// <param name="time">how long to wait</param>
    /// <param name="onComplete">a callback that is called when the audioclip is done playing</param>
    /// <returns></returns>
    private IEnumerator WaitForAudio(float time,System.Action onComplete)
    {
        yield return new WaitForSeconds(time);

        //Once the audio is done
        katjaFeSkeleton.AnimationState.SetAnimation(0, "Idle", true).MixDuration = 0.2f;

        if (onComplete != null)
        {
            onComplete.Invoke();
        }
    }

    /// <summary>
    /// used to wait until a animation is done playing
    /// </summary>
    /// <param name="duration">how long to wait</param>
    /// <param name="onComplete">a callback that is called when the animation is done playing</param>
    /// <param name="state">if it's a end or continues animation</param>
    /// <returns></returns>
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

    public void End()
    {
       
          KatjaExit();

        
    }
}
