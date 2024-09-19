using Scenes;
using System.Collections;
using Unity.VisualScripting;
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
    /// Where everything plays out.
    /// </summary>
 

    // When you press Spacebar you submit answer.
    private void SubmitAnswer()
    {
        StartCoroutine(RotateLever());

        if (hourTime == watch.randoHour && minuteTime == int.Parse(watch.randoMinute))
        {
            score++;


            watch.GetRandoText();
            
            if (score >= 5)
            {
               
                SwitchScenes.SwitchToArcadeScene();
            }
        }


    }


    /// <summary>
    /// Move the Clockneedles on the cat clock
    /// </summary>
    private void OnMouseOver()
    {

        if (minuteTime >= 60)
        {
            minuteTime = 0;
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
