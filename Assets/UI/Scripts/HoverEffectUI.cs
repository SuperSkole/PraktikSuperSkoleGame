using System.Collections;
using Import.LeanTween.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class HoverEffectUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        private Vector3 originalScale;

        private void Awake()
        {
            //tjek om billede er blevet udfyldt i Inspektoren 
            if (image == null)
            {
                image = GetComponent<Image>();
            }
            if (image != null)
            {
                originalScale = image.rectTransform.localScale;
            }
        }

        public void HoverEnter()
        {
            //N�r mus/finger er over knappen
            StartCoroutine(JiggleAndLightUp());

            AudioSource audioSource = this.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }


        public void HoverExit()
        {
            //N�r mus/finger ikke l�ngere er over knappen
            ResetImage();
        }

        //LeanTween animation for HoverEnter
        private IEnumerator JiggleAndLightUp()
        {
            // St�rre
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
}
