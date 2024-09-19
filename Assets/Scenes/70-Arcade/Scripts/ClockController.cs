using Scenes;
using System.Collections;
using TMPro;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    [SerializeField]
    private Transform hourGameObject, minuteGameObject;

    [SerializeField] private GameObject submitAnswerLever;

    [SerializeField] private float hourTime = 10;

    [SerializeField] private float minuteTime = 10;

    [SerializeField]
    private WatchSpawner watch;

    [SerializeField]
    private float score = 0;

    [SerializeField]
    private GameObject scoreTextObject;

    public TextMeshProUGUI scoreText;

    private bool guessing = false;

    //used to rotate the cat tail
    public float rotationAngle = 35f;
    public float rotationSpeed = 100f;

    private Quaternion initialRotation;

    private void Start()
    {
        guessing = false;
        initialRotation = submitAnswerLever.transform.rotation;
        scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();


    }


    // When you press Spacebar you submit answer.
    public void SubmitAnswer()
    {
        guessing = true;
        StartCoroutine(RotateLever());

        if (hourTime == watch.randoHour && minuteTime == int.Parse(watch.randoMinute))
        {
            score++;
            scoreText.text = $"Point: {score}";

            watch.GetRandoText();

            if (score >= 5)
            {

                SwitchScenes.SwitchToArcadeScene();
            }
        }


    }


    public void MinuteArrowRight()
    {

        minuteGameObject.Rotate(Vector3.back, -30);

        minuteTime += 5;

        if (minuteTime >= 60)
        {
            minuteTime = 0;
        }
    }

    public void MinuteArrowLeft()
    {

        minuteGameObject.Rotate(Vector3.back, 30);

        minuteTime -= 5;

        if (minuteTime <= -5)
        {
            minuteTime = 55;
        }
    }

    public void HourArrowRight()
    {

        hourGameObject.Rotate(Vector3.back, -30);

        hourTime += 1;

        if (hourTime >= 13)
        {
            hourTime = 1;
        }
    }

    public void HourArrowLeft()
    {

        hourGameObject.Rotate(Vector3.back, 30);

        hourTime -= 1;

        if (hourTime <= 0)
        {
            hourTime = 12;
        }
    }


    /// <summary>
    /// Move the Clockneedles on the cat clock
    /// </summary>


    //The RotateLever coroutine handles the sequence of rotations:
    IEnumerator RotateLever()
    {
        
        if (guessing == true)
        {


            // Rotate to the left by rotationAngle
            yield return RotateOverTime(-rotationAngle);

            // Rotate to the right by 2x the rotationAngle (to go in the opposite direction)
            yield return RotateOverTime(rotationAngle * 2);

            // Rotate back to the initial position
            yield return RotateOverTime(-rotationAngle);

        }
        guessing = false;
    }

    //The RotateOverTime coroutine gradually rotates the lever towards the desired angle
    IEnumerator RotateOverTime(float angle)
    {
        Quaternion targetRotation = Quaternion.Euler(submitAnswerLever.transform.eulerAngles + new Vector3(0, 0, angle));
        while (Quaternion.Angle(submitAnswerLever.transform.rotation, targetRotation) > 0.1f)
        {
            submitAnswerLever.transform.rotation = Quaternion.RotateTowards(
                submitAnswerLever.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime
            );
            yield return null;
        }
        // Ensure we reach the exact target rotation
        submitAnswerLever.transform.rotation = targetRotation;
    }

}
