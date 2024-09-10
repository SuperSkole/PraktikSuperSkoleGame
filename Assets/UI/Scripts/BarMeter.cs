using System.Collections;
using CORE;
using Import.LeanTween.Framework;
using Scenes._10_PlayerScene.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Scripts
{
    public class BarMeter : MonoBehaviour
    {
        [SerializeField] private Image barFill;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI textMeshPro;

        [SerializeField] private int maxAmount;

        private float fillSpeed = 0.5f;

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

            textMeshPro.text = 0 + "/" + maxAmount;

            SettingValueAfterScene(GameManager.Instance.PlayerData.CurrentGoldAmount);
        }

        public void SettingValueAfterScene(int amount)
        {
            textMeshPro.text = $"{amount}/{maxAmount}";
            barFill.fillAmount = Mathf.Clamp01(barFill.fillAmount + (float)amount / maxAmount);
        }

        public void ChangeValue(int amount)
        {
            if (changeValueCoroutine != null)
            {
                StopCoroutine(changeValueCoroutine);
            }

            StopAllCoroutines();
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

                //Text showing progress
                textMeshPro.text = Mathf.RoundToInt(currentFillAmount * 100).ToString()+"/"+maxAmount;

                // St�rre
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
}
