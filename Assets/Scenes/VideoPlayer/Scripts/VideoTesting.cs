using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoTesting : MonoBehaviour
{
    public VideoPlayer vid;
    [SerializeField] private Slider lengthSlider;


    // Flag to check if the user is dragging the slider
    private bool isDraggingSlider = false;
    private void Start()
    {
        lengthSlider.maxValue = (float)vid.clip.length;
    }

    private void Update()
    {
        // Only update the slider if the user is not dragging it
        if (!isDraggingSlider)
        {
            lengthSlider.value = (float)vid.time;
        }

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
    // Method to be called when the user starts dragging the slider
    public void OnSliderBeginDrag()
    {
        isDraggingSlider = true;
    }
    // Method to be called when the user ends dragging the slider
    public void OnSliderEndDrag()
    {
        isDraggingSlider = false;
        vid.time = lengthSlider.value; // Set the video time to the slider value after dragging ends
    }

    public void OnSlideValueChanged()
    {
        // This method is now only responsible for handling value changes when the user drags the slider
        if (isDraggingSlider)
        {
            vid.time = lengthSlider.value;
        }
    }
    /*
     * vid.time | current time of video in seconds
     * vid.clip.length | Total length of the video in seconds
     */

}
