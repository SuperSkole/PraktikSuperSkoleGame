using System.Collections;
using Import.LeanTween.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class XPBar : MonoBehaviour
    {
        [SerializeField] Image barFill;
        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI amount;

        [SerializeField] int maxAmount;

        [SerializeField] TextMeshProUGUI level;

        private float fillSpeed = 0.5f;

        private int currentXP = 0;
        private int currentLevel = 1;

        private Coroutine changeValueCoroutine;

        private Vector3 originalScale;


        void Awake()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }
            originalScale = image.rectTransform.localScale;

            amount.text = 0 + "/" + maxAmount;
        }

        public void AddXP(int xp)
        {
            currentXP += xp;

            UpdateXPBar();
        }

        private void LevelUp()
        {

            currentLevel++;

            //exponential raise of maxamount
            maxAmount = Mathf.FloorToInt(maxAmount * 1.5f);


            //animation
            LeanTween.scale(image.rectTransform, new Vector3(1.5f, 1.5f, 1.5f), 0.5f).setEase(LeanTweenType.easeOutBack).setOnComplete(ShakeImage);

            //reset the texts and bar
            level.text = currentLevel.ToString();
            amount.text = 0 + "/" + maxAmount;

            barFill.fillAmount = 0;


        }

        private void UpdateXPBar()
        {
            //Coroutine handling
            if (changeValueCoroutine != null)
            {
                StopCoroutine(changeValueCoroutine);
            }

            changeValueCoroutine = StartCoroutine(ChangeValueCoroutine(currentXP));
 
        }


        void ShakeImage()
        {
            //LeanTween animation used in LevelUp
            LeanTween.rotateZ(image.gameObject, 10f, 0.1f).setLoopPingPong(2)
                .setOnComplete(() =>
                {
                    LeanTween.scale(image.rectTransform, originalScale, 0.1f);
                    LeanTween.rotateZ(image.gameObject, 0f, 0f);
                });
        }

        private IEnumerator ChangeValueCoroutine(float targetXP)
        {
            //current
            float currentFillAmount = barFill.fillAmount;

            //goal
            float targetFillAmount = Mathf.Clamp01((float)targetXP / maxAmount);

            //As l�ng as the two values are apart
            while (!Mathf.Approximately(currentFillAmount, targetFillAmount))
            {
                currentFillAmount = Mathf.MoveTowards(currentFillAmount, targetFillAmount, fillSpeed * Time.deltaTime);
                barFill.fillAmount = currentFillAmount;

                //Text showing progress
                amount.text = Mathf.RoundToInt(currentFillAmount*maxAmount) + "/" + maxAmount;


                yield return null;
            }

            barFill.fillAmount = targetFillAmount;

            //if during the transition, xp exceeds maxamount
            if (currentXP >= maxAmount)
            {
                int overflowXP = currentXP - maxAmount;
                currentXP = 0;

                LevelUp();

                AddXP(overflowXP);
            }

            changeValueCoroutine = null;

        }
    }
}
