using Scenes;
using System.Collections;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    [SerializeField]
    private Transform hourGameObject, minuteGameObject;

    [SerializeField] private GameObject submitAnswerLever;

    [SerializeField] private float hourTime = 10;

    [SerializeField] private float minuteTime = 10;

    private WatchSpawner watch;

    private float score = 0;

    private bool youWin = false;

    private bool keydown = false;

    //used to rotate the cat tail
    public float rotationAngle = 35f;
    public float rotationSpeed = 100f;

    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = submitAnswerLever.transform.rotation;
    }


    /// <summary>
    /// Update each frame
    /// </summary>
    private void Update()
    {
        ClockGame();
    }

    /// <summary>
    /// Where everything plays out.
    /// </summary>
    private void ClockGame()
    {
        while (!youWin)
        {
            watch.GetRandoText();


        }
    }

    private void SubmitAnswer()
    {
        StartCoroutine(RotateLever());




        if (hourTime == int.Parse(watch.hourText.text) && minuteTime == int.Parse(watch.minuteText.text))
        {
            score++;
        }

        if (score >= 5)
        {
            SwitchScenes.SwitchToArcadeScene();
        }
    }


    /// <summary>
    /// Move the Clockneedles on the cat clock
    /// </summary>
    private void OnMouseOver()
    {

        if (minuteTime >= 65)
        {
            minuteTime = 5;
        }

        if (hourTime >= 13)
        {
            hourTime = 1;
        }

        if (Input.GetMouseButtonDown(1))
        {
            minuteGameObject.Rotate(Vector3.back, -30);

            minuteTime += 5;
        }

        if (Input.GetMouseButtonDown(0))
        {
            hourGameObject.Rotate(Vector3.back, -30f);

            hourTime += 1;
        }

        //plays the audioLetterSource once by pressing space
        if (Input.GetKeyDown(KeyCode.Space) && keydown == false)
        {
            SubmitAnswer();
            keydown = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) && keydown == true)
        {
            keydown = false;
        }
    }

    //The RotateLever coroutine handles the sequence of rotations:
    IEnumerator RotateLever()
    {
        // Rotate to the left by rotationAngle
        yield return RotateOverTime(-rotationAngle);

        // Rotate to the right by 2x the rotationAngle (to go in the opposite direction)
        yield return RotateOverTime(rotationAngle * 2);

        // Rotate back to the initial position
        yield return RotateOverTime(-rotationAngle);
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
