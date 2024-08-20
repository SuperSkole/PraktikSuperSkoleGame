using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarMeter : MonoBehaviour
{
    [SerializeField] Image barFill;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI textMeshPro;

    [SerializeField] int maxAmount;

    private float fillSpeed = 0.5f;
    private float tweenDuration = 0.5f;

    private Coroutine changeValueCoroutine;

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

    public void CurrentAmount()
    {

    }

    public void ChangeValue(int amount)
    {
        if (changeValueCoroutine != null)
        {
            StopCoroutine(changeValueCoroutine);
        }

        changeValueCoroutine = StartCoroutine(ChangeValueRoutine(amount));
    }

    private IEnumerator ChangeValueRoutine(int amount)
    {
        //goal amount
        float targetFillAmount = Mathf.Clamp01(barFill.fillAmount + (float)amount / maxAmount);
        //current amount
        float currentFillAmount = barFill.fillAmount;

        //As long as they're apart
        while (!Mathf.Approximately(currentFillAmount, targetFillAmount))
        {
            currentFillAmount = Mathf.MoveTowards(currentFillAmount, targetFillAmount, fillSpeed * Time.deltaTime);
            barFill.fillAmount = currentFillAmount;

            textMeshPro.text = Mathf.RoundToInt(currentFillAmount * 100).ToString();

            // Større
            LeanTween.scale(image.rectTransform, originalScale * 1.2f, 0.1f);

            // Jiggle effect
            LeanTween.rotateZ(image.gameObject, 10f, 0.1f).setLoopPingPong(2);

            yield return null;
        }

        barFill.fillAmount = targetFillAmount;
        changeValueCoroutine = null;

        LeanTween.scale(image.rectTransform, originalScale, 0.1f);
        LeanTween.rotateZ(image.gameObject, 0f, 0f);
    }
}
