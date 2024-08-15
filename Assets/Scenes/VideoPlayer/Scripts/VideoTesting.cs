using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoTesting : MonoBehaviour
{
    public VideoPlayer vid;
    [SerializeField] private Slider lengthSlider;

    private void Start()
    {
        lengthSlider.maxValue = (float)vid.clip.length;
    }

    private void Update()
    {
       lengthSlider.value = (float)vid.time;

        if (vid.time >= vid.clip.length)
        {
            Debug.Log("The end of the video has ben reached, do something different");
        }

    }
    public void SkipTime(float amount)
    {
        vid.time += amount;

    }

    public void ChangeVideo(VideoClip clip)
    {
        vid.clip = clip;
        lengthSlider.maxValue = (float)vid.clip.length;
    }
    public void OnSlideValueChanged()
    {
        //vid.time = lengthSlider.value;
    }
    /*
     * vid.time | current time of video in seconds
     * vid.clip.length | Total length of the video in seconds
     */

}
