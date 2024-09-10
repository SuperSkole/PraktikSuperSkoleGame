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

        private float fillSpeed = 50f;
        private int currentAmount = 0;

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

            textMeshPro.text = "0";
            currentAmount = GameManager.Instance.PlayerData.CurrentGoldAmount;
            SettingValueAfterScene(GameManager.Instance.PlayerData.CurrentGoldAmount);
            ChangeValue(GameManager.Instance.PlayerData.PendingGoldAmount);
        }

        public void SettingValueAfterScene(int amount)
        {
            textMeshPro.text = amount.ToString();
        }

        public void ChangeValue(int amount)
        {
            StopCoroutine(changeValueCoroutine);
            
            currentAmount += amount;
            GameManager.Instance.PlayerData.CurrentGoldAmount = currentAmount;
            GameManager.Instance.PlayerData.PendingGoldAmount = 0;
            changeValueCoroutine = StartCoroutine(ChangeValueRoutine(amount));
        }

        private IEnumerator ChangeValueRoutine(int amount)
        {
            float currentFillAmount = currentAmount-amount;
            //As long as they're apart
            while (Mathf.RoundToInt(currentFillAmount) != currentAmount)
            {
                currentFillAmount = Mathf.MoveTowards(currentFillAmount, currentAmount, 1);
                //Text showing progress
                textMeshPro.text = Mathf.RoundToInt(currentFillAmount).ToString();

                // St�rre
                LeanTween.scale(image.rectTransform, originalScale * 1.2f, 0.1f);

                // Jiggle effect
                LeanTween.rotateZ(image.gameObject, 10f, 0.1f).setLoopPingPong(2);

                yield return new WaitForSeconds(0.1f);
            }

            changeValueCoroutine = null;

            LeanTween.scale(image.rectTransform, originalScale, 0.1f);
            LeanTween.rotateZ(image.gameObject, 0f, 0f);
        }
    }
}
