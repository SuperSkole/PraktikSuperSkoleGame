using Scenes;
using System.Collections;
using TMPro;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    [SerializeField]
    private GameObject catGlass;

    [SerializeField]
    private Material defaultMaterial, wrongMaterial, correctMaterial;

    [SerializeField]
    private AudioClip correctCat, wrongCat, buttonTick, backgroundMusic;


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

    private Coroutine courtine;

    //used to rotate the cat tail
    public float rotationAngle = 35f;
    public float rotationSpeed = 100f;

    private Quaternion initialRotation;

    private void Start()
    {

        initialRotation = submitAnswerLever.transform.rotation;
        scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        AudioManager.Instance.PlaySound(backgroundMusic, SoundType.Music, true);

    }


    // When you press Gaet, you submit answer.
    public void SubmitAnswer()
    {

        if (courtine == null)
        {


            courtine = StartCoroutine(RotateLever());

            if (hourTime == watch.randoHour && minuteTime == int.Parse(watch.randoMinute))
            {

                StartCoroutine(CorrectAnswer());

                score++;
                scoreText.text = $"Point: {score}";

                watch.GetRandoText();

                if (score >= 5)
                {

                    SwitchScenes.SwitchToArcadeScene();
                }
            }
            else
            {
                StartCoroutine(WrongAnswer());
            }

        }

    }


    /// <summary>
    /// code for controlling the Clocks Minut needle
    /// </summary>
    public void MinuteArrowRight()
    {

        minuteGameObject.Rotate(Vector3.back, -30);
        hourGameObject.Rotate(Vector3.back, -2.5f);

        minuteTime += 5;

        if (minuteTime >= 60)
        {
            minuteTime = 0;
            hourTime++;
            if (hourTime >= 12)
            {
                hourTime = 0;
            }
        }
        AudioManager.Instance.PlaySound(buttonTick, SoundType.SFX);

    }

    /// <summary>
    /// code for controlling the Clocks Minut needle
    /// </summary>
    public void MinuteArrowLeft()
    {

        minuteGameObject.Rotate(Vector3.back, 30);
        hourGameObject.Rotate(Vector3.back, 2.5f);

        minuteTime -= 5;

        if (minuteTime <= -5)
        {
            minuteTime = 55;
            hourTime--;
            if (hourTime <= -1)
            {
                hourTime = 11;
            }
        }
        AudioManager.Instance.PlaySound(buttonTick, SoundType.SFX);
    }

    /// <summary>
    /// code for controlling the Clocks hour needle
    /// </summary>
    public void HourArrowRight()
    {

        hourGameObject.Rotate(Vector3.back, -30);

        hourTime += 1;

        if (hourTime >= 12)
        {
            hourTime = 0;
        }
        AudioManager.Instance.PlaySound(buttonTick, SoundType.SFX);
    }

    /// <summary>
    /// code for controlling the Clocks hour needle
    /// </summary>
    public void HourArrowLeft()
    {

        hourGameObject.Rotate(Vector3.back, 30);

        hourTime -= 1;

        if (hourTime <= -1)
        {
            hourTime = 11;
        }
        AudioManager.Instance.PlaySound(buttonTick, SoundType.SFX);
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

        courtine = null;
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

    /// <summary>
    /// changes the color of the cats glass to red
    /// </summary>
    /// <returns></returns>
    IEnumerator WrongAnswer()
    {
        catGlass.GetComponent<MeshRenderer>().material = wrongMaterial;

        AudioManager.Instance.PlaySound(wrongCat, SoundType.SFX);

        yield return new WaitForSeconds(1);

        catGlass.GetComponent<MeshRenderer>().material = defaultMaterial;
    }

    /// <summary>
    /// changes the color of the cats glass to green
    /// </summary>
    /// <returns></returns>
    IEnumerator CorrectAnswer()
    {
        catGlass.GetComponent<MeshRenderer>().material = correctMaterial;

        AudioManager.Instance.PlaySound(correctCat, SoundType.SFX);

        yield return new WaitForSeconds(1);

        catGlass.GetComponent<MeshRenderer>().material = defaultMaterial;
    }

}
