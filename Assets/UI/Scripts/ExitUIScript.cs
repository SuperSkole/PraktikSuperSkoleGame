using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitUIScript : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image yesButton;
    [SerializeField] private Image noButton;
    [SerializeField] private string sceneToLoad;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void ShowPanel()
    {
        //afspil animation
        panel.transform.localScale = Vector3.zero;

        panel.SetActive(!panel.activeSelf);

        LeanTween.scale(panel, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack);

        //afspil exit lyd
        AudioSource audioSource = panel.GetComponent<AudioSource>();
        if (audioSource != null && panel.activeSelf)
        {
            audioSource.Play();
        }
    }

    public void OnYesButton()
    {
        if (sceneToLoad != null)
        {
            //load scene hvis der er en
            SceneManager.LoadScene(sceneToLoad);

            //spil yes lydeffekt
            AudioSource audioSource = yesButton.GetComponent<AudioSource>();
            if (audioSource != null && panel.activeSelf)
            {
                audioSource.Play();
            }
        }
    }

    public  void OnNoButton()
    {
        //luk panlet
        panel.SetActive (false);

        //spil no lydeffekt
        AudioSource audioSource = noButton.GetComponent<AudioSource>();
        if (audioSource != null && panel.activeSelf)
        {
            audioSource.Play();
        }
    }

        

}
