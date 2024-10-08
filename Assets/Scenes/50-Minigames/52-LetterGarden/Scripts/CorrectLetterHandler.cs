using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CorrectLetterHandler : MonoBehaviour
{
    private bool moving = false;
    private Vector3 startPos;
    private Vector3 endPos = new Vector3(-0.15f, 10.4f, -15.15f);
    private float startDistance;
    private float speed = 25;
    public TextMeshProUGUI exampleLetter;
    public RawImage image;
    /// <summary>
    /// Starts a coroutine to automaticly close the window after some time
    /// </summary>
    void Start()
    {
        StartCoroutine(AutomaticClose());
    }

    /// <summary>
    /// if movement has begun the window is moved towards the upper right corner and scaled down at the same time
    /// </summary>
    void Update()
    {
        if(moving)
        {
            float velocity = speed * Time.deltaTime;
            
            transform.position = Vector3.MoveTowards(transform.position, endPos, velocity);
            float newScale = Vector3.Distance(transform.position, endPos)/ startDistance;
            transform.localScale = new Vector3(newScale, newScale, newScale);

        }
    }

    /// <summary>
    /// Starts moving the window after 2 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator AutomaticClose()
    {
        yield return new WaitForSeconds(2);
        if(!moving)
        {
            StartMoving();
        }
    }

    /// <summary>
    /// sets various movement related variables
    /// </summary>
    private void StartMoving()
    {
        moving = true;
        startPos = transform.position;
        startDistance = Vector3.Distance(startPos, endPos);
    }

    /// <summary>
    /// Starts the movement when clicking on the close button
    /// </summary>
    public void OnClick()
    {
        StartMoving();
    }

    /// <summary>
    /// Destroys the window if it is no longer visible
    /// </summary>
    private void OnBecameInvisible()
    {
        moving = false;
        gameObject.SetActive(false);

    }
}
