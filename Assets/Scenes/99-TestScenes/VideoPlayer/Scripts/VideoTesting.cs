using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Scenes._99_TestScenes.VideoPlayer.Scripts
{
    public class VideoTesting : MonoBehaviour
    {
        public UnityEngine.Video.VideoPlayer vid;
        [SerializeField] private Slider lengthSlider;

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

            if (vid.time >= (int)vid.clip.length)
            {
                Debug.Log("The end of the video has ben reached, do something different");
            }

        }
        /// <summary>
        /// Skips a certain amount of time into a video
        /// </summary>
        /// <param name="amount"></param>
        public void SkipTime(float amount)
        {
            vid.time += amount;

        }
        /// <summary>
        /// Changes the played video
        /// </summary>
        /// <param name="clip">Which Video to be played</param>
        public void ChangeVideo(VideoClip clip)
        {
            vid.clip = clip;
            lengthSlider.maxValue = (float)vid.clip.length;
        }
        /// <summary>
        /// Method to be called from Event Trigger when the user starts dragging the slider
        /// </summary>
        public void OnSliderBeginDrag()
        {
            isDraggingSlider = true;
        }
        /// <summary>
        /// Method to be called from Event Trigger when the user ends dragging the slider
        /// </summary>
        public void OnSliderEndDrag()
        {
            isDraggingSlider = false;
            vid.time = lengthSlider.value; // Set the video time to the slider value after dragging ends
        }

        public void SkipVideo() { vid.time = vid.clip.length;}

        /*
     * vid.time | current time of video in seconds
     * vid.clip.length | Total length of the video in seconds
     */

    }
}
