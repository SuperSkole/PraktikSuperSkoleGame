using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class BlinkEffectUI : MonoBehaviour
    {
        [SerializeField] private Image panel; 
        private Color originalColor; 

        private void Awake()
        {
            if (panel == null)
            {
                panel = GetComponent<Image>(); 
            }
            originalColor = panel.color; 
        }

        // Method to trigger the blink effect
        public void TriggerBlink(Color color)
        {
            StartCoroutine(BlinkRed(color));
        }

        private IEnumerator BlinkRed(Color color)
        {
            int numBlinks = 3;
            float duration = 0.2f; // Duration for one single blink

            for (int i = 0; i < numBlinks; i++)
            {
                // Lerp to color
                yield return LerpColor(color, duration / 2);
                // Lerp back to original color
                yield return LerpColor(originalColor, duration / 2);
            }
        }

        private IEnumerator LerpColor(Color targetColor, float duration)
        {
            float time = 0;
            Color startColor = panel.color;

            while (time < duration)
            {
                panel.color = Color.Lerp(startColor, targetColor, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            panel.color = targetColor;
        }
    }
}