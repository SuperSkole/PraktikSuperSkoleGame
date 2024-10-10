using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

/// <summary>
/// script to handle clicking on an in game button in the bank front minigame
/// </summary>
public class KeyPress : MonoBehaviour
{
    Vector3 startingPosition;
    float pressedY = 0.0024f;
    [SerializeField]Vector3 currentDestination;

    public delegate void OnClickMethod();

    public OnClickMethod onClickMethod;

    [SerializeField]private AudioClip keySound;
    /// <summary>
    /// Sets up the starting position
    /// </summary>
    void Start()
    {
        startingPosition = transform.localPosition;
        currentDestination = startingPosition;
    }

    /// <summary>
    /// Handles movement after the button has been pressed and executing its function when it reaches the pressed position
    /// </summary>
    void Update()
    {
        //if the button reaches the pressed position it calls its on click method and reverses its direction
        if (currentDestination != startingPosition && transform.localPosition == currentDestination)
        {
            currentDestination = startingPosition;
            AudioManager.Instance.PlaySound(keySound, SoundType.SFX, transform.position);
            onClickMethod();
        }
        //Moves the button towards either the pressed position or its starting position
        else if(transform.localPosition != currentDestination)
        {
            float velocity = 0.5f * Time.deltaTime;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, currentDestination, velocity);
        }
    }
    /// <summary>
    /// Reacts to the player clicking on the button
    /// </summary>
    void OnMouseDown()
    {
        OnClick();
    }

    /// <summary>
    /// if the button is in the starting position it starts to move down
    /// </summary>
    public void OnClick()
    {
        if(transform.localPosition == startingPosition)
        {
            currentDestination = new Vector3(transform.localPosition.x, pressedY, transform.localPosition.z);
        }
    }
}
