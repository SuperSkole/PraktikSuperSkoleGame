using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class HoverEffectUI : MonoBehaviour
{
    [SerializeField] Image image; 
    private Vector3 originalScale;

    private void Awake()
    {
        //tjek om billede er blevet udfyldt i Inspektoren 
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        originalScale = image.rectTransform.localScale;
    }

    public void HoverEnter()
    {
        //Når mus/finger er over knappen
        StartCoroutine(JiggleAndLightUp());

        AudioSource audioSource = this.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void HoverExit()
    {
        //Når mus/finger ikke længere er over knappen
        ResetImage();
    }

    //LeanTween animation for HoverEnter
    private IEnumerator JiggleAndLightUp()
    {
        // Større
        LeanTween.scale(image.rectTransform, originalScale * 1.2f, 0.1f);

        // Jiggle effect
        LeanTween.rotateZ(image.gameObject, 10f, 0.1f).setLoopPingPong(2);

        yield return new WaitForSeconds(0.2f);
    }

    // Reset
    private void ResetImage()
    {
        LeanTween.scale(image.rectTransform, originalScale, 0.1f);
        LeanTween.rotateZ(image.gameObject, 0f, 0f);


    }
}
