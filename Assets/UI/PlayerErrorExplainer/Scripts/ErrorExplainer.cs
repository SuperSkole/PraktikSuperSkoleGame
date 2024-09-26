using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorExplainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private AudioSource audioSource;

    private void InstantiateExplanation(string explanation, AudioClip explanationSound)
    {
        float textLength = 0;
        if(explanation != "")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if(text.text != explanation)
            {
                text.text = explanation;
            }
            textLength = explanation.Length / 4;
        }
        float soundLength = 0;
        if(explanationSound != null)
        {
            audioSource.PlayOneShot(explanationSound);
            soundLength = explanationSound.length;
        }
        StartCoroutine(AutomaticClose(textLength, soundLength));
    }

    public void AddExplanation(string explanation, AudioClip explanationSound)
    {
        InstantiateExplanation(explanation, explanationSound);
    }

    public void AddExplanation(string explanation)
    {
        InstantiateExplanation(explanation, null);
    }

    public void AddExplanation(AudioClip explanationSound)
    {
        InstantiateExplanation("", explanationSound);
    }

    public void OnClick()
    {
        CloseExplanation();
    }

    private void CloseExplanation()
    {
        text.gameObject.transform.parent.gameObject.SetActive(false);
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        gameObject.SetActive(false);
    }

    IEnumerator AutomaticClose(float textLength, float soundLength)
    {
        if(textLength > soundLength)
        {
            yield return new WaitForSeconds(textLength);
        }

        else
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);
            yield return new WaitForSeconds(0.25f);
        }
        CloseExplanation();
    }
}
