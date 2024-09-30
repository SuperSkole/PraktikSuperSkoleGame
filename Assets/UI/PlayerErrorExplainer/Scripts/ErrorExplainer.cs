using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Class to handle the player error explainer
/// </summary>
public class ErrorExplainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// Starts the explanation with the desired parts
    /// </summary>
    /// <param name="explanation">a text explanation</param>
    /// <param name="explanationSound">a sound explanation</param>
    private void InstantiateExplanation(string explanation, AudioClip explanationSound)
    {
        //Checks if the text explanation should be used and sets up the textfield in that case
        float textLength = 0;
        if(explanation != "")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if(text.text != explanation)
            {
                text.text = explanation;
            }
            textLength = explanation.Length / 8;
        }
        //plays the sound explanation if it is used
        float soundLength = 0;
        if(explanationSound != null)
        {
            audioSource.PlayOneShot(explanationSound);
            soundLength = explanationSound.length;
        }
        StartCoroutine(AutomaticClose(textLength, soundLength));
    }

    /// <summary>
    /// Starts the explanation with both text and sound
    /// </summary>
    /// <param name="explanation">the text explanation</param>
    /// <param name="explanationSound">the sound explanation</param>
    public void AddExplanation(string explanation, AudioClip explanationSound)
    {
        InstantiateExplanation(explanation, explanationSound);
    }

    /// <summary>
    /// Starts the explanation with just text
    /// </summary>
    /// <param name="explanation">The text explanation</param>
    public void AddExplanation(string explanation)
    {
        InstantiateExplanation(explanation, null);
    }

    /// <summary>
    /// Starts the explanation with just sound
    /// </summary>
    /// <param name="explanationSound">The sound of the explanation</param>
    public void AddExplanation(AudioClip explanationSound)
    {
        InstantiateExplanation("", explanationSound);
    }

    /// <summary>
    /// Closes the explanation if it is clicked on
    /// </summary>
    public void OnClick()
    {
        CloseExplanation();
    }

    /// <summary>
    /// Closes the explanation and resets it to default
    /// </summary>
    private void CloseExplanation()
    {
        text.gameObject.transform.parent.gameObject.SetActive(false);
        //stops the audio if it is still playing
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Automaticlly closes the explainer after some time based on either the text or the sound, depending on which is longest
    /// </summary>
    /// <param name="textLength">The estimated length of the text in seconds</param>
    /// <param name="soundLength">The length of the sound file in seconds</param>
    /// <returns></returns>
    IEnumerator AutomaticClose(float textLength, float soundLength)
    {   
        //waits until the player has read the text
        if(textLength > soundLength)
        {
            yield return new WaitForSeconds(textLength);
        }
        //Waits until a bit after the sound has finished playing
        else
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);
            yield return new WaitForSeconds(0.25f);
        }
        CloseExplanation();
    }
}
