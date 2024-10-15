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

        private GameObject coinPrefab;

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

            currentAmount = GameManager.Instance.PlayerData.CurrentGoldAmount;

            textMeshPro.text = currentAmount.ToString();
            barFill.fillAmount = Mathf.Clamp01((float)currentAmount / maxAmount);
            coinPrefab =  GameManager.Instance.PlayerManager.coinPrefab;
            StartCoroutine(AddGold(GameManager.Instance.PlayerData.PendingGoldAmount));
            ChangeValue(GameManager.Instance.PlayerData.PendingGoldAmount);
        }
        private void OnEnable()
        {
            currentAmount = GameManager.Instance.PlayerData.CurrentGoldAmount;
            textMeshPro.text = currentAmount.ToString();
            barFill.fillAmount = Mathf.Clamp01((float)currentAmount / maxAmount);
        }

        public void SettingValueAfterScene(int amount)
        {
            textMeshPro.text = amount.ToString();
            barFill.fillAmount = Mathf.Clamp01((float)amount / maxAmount);
            currentAmount = amount;
               
        }

        /// <summary>
        /// Instantiatese a given amount of coins to fly towards the corner
        /// </summary>
        /// <param name="amount">how many coins should be instantiated</param>
        /// <returns></returns>
        private IEnumerator AddGold(int amount)
        {
            //Creates a coin waits a bit and then creates a new coin until the given amount of coins have been created
            while (amount > 0)
            {
                Instantiate(coinPrefab);
                amount--;
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            }
        }

        public void ChangeValue(int amount)
        {
            if (changeValueCoroutine != null)
            {
                StopCoroutine(changeValueCoroutine);
            }
            
            currentAmount += amount;

            GameManager.Instance.PlayerData.CurrentGoldAmount = currentAmount;
            GameManager.Instance.PlayerData.PendingGoldAmount = 0;

            if(coinPrefab == null && GameManager.Instance.PlayerManager != null)
            {

                coinPrefab = GameManager.Instance.PlayerManager.coinPrefab;
            }
            changeValueCoroutine = StartCoroutine(ChangeValueRoutine(amount));
        }

        private IEnumerator ChangeValueRoutine(int amount)
        {
            float startingAmount = currentAmount-amount;
            float targetAmount = currentAmount;

            float duration = 1.5f; 
            float elapsed = 0f;

            //bigger
            LeanTween.scale(image.rectTransform, originalScale * 1.2f, 0.1f);

            // Jiggle effect
            LeanTween.rotateZ(image.gameObject, 10f, 0.1f).setLoopPingPong(2);

            //As long as they're apart
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                // Calculate interpolation factor (0 to 1 over the duration)
                float t = Mathf.Clamp01(elapsed / duration);

                // Interpolate the current fill amount
                float currentFillAmount = Mathf.Lerp(startingAmount, targetAmount, t);

                barFill.fillAmount = Mathf.Clamp01(currentFillAmount/ maxAmount);
                // Update the text
                textMeshPro.text = Mathf.RoundToInt(currentFillAmount).ToString();

                            
                // Wait until the next frame
                yield return null;
                
            }

            textMeshPro.text = currentAmount.ToString();

            LeanTween.scale(image.rectTransform, originalScale, 0.1f);
            LeanTween.rotateZ(image.gameObject, 0f, 0f);

            changeValueCoroutine = null;
        }
    }
}
