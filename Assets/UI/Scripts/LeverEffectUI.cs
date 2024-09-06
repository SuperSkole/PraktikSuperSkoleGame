using System.Collections;
using Import.LeanTween.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Scripts
{
    /// <summary>
    /// Handles the lever-like behavior for the lever image.
    /// </summary>
    public class LeverEffectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image leverImage; 
        private Vector3 originalScale; 
        private float originalRotationZ; // Original Z rotation (lever starts at 60 degrees for 2 o'clock)

        private bool isJiggling = false; 
        private bool isPulling = false; 

        private void Awake()
        {
            if (leverImage == null)
            {
                leverImage = GetComponent<Image>();
            }
            
            if (leverImage != null)
            {
                originalScale = leverImage.rectTransform.localScale;
                // Starting at ca. 2 o'clock
                originalRotationZ = -20f; 
                // Set the lever angle to 60°
                leverImage.rectTransform.localEulerAngles = new Vector3(0, 0, originalRotationZ); 
            }
        }

        /// <summary>
        /// Handles the hover enter behavior (nodding and scaling effect).
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Only start jiggle if not already jiggling or pulling
            if (!isJiggling && !isPulling) 
            {
                StartCoroutine(NodAndLightUp());
            }
        }

        /// <summary>
        /// Handles the hover exit behavior (resetting the image).
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            // Only reset if not in the middle of a pull
            if (!isPulling) 
            {
                ResetImage();
            }
        }

        /// <summary>
        /// Handles the click behavior (pulling the lever down).
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            // Only start pull if not already pulling
            if (!isPulling) 
            {
                StartCoroutine(PullLever());
            }
        }

        /// <summary>
        /// Coroutine to create a nodding and light-up effect.
        /// </summary>
        private IEnumerator NodAndLightUp()
        {
            isJiggling = true;

            // Light up and scale up
            LeanTween.scale(leverImage.rectTransform, originalScale * 1.2f, 0.1f);
        
            // Nod effect (rotate slightly forward and back)
            LeanTween.rotateZ(leverImage.gameObject, originalRotationZ + 10f, 0.1f).setLoopPingPong(2);

            yield return new WaitForSeconds(0.2f);

            isJiggling = false;
        }

        /// <summary>
        /// Coroutine to pull the lever down and let it spring back up.
        /// </summary>
        private IEnumerator PullLever()
        {
            isPulling = true;

            // Pull lever down to ca. 4 o'clock, over .5 seconds time
            LeanTween.rotateZ(leverImage.gameObject, -100f, 0.5f); 

            // Wait for pull-down to finish
            yield return new WaitForSeconds(0.5f); 

            // Spring back up to original position
            LeanTween.rotateZ(leverImage.gameObject, originalRotationZ, 1f); 

            // Wait for spring-up to finish
            yield return new WaitForSeconds(1f); 

            isPulling = false;
        }

        /// <summary>
        /// Resets the image to its original state.
        /// </summary>
        private void ResetImage()
        {
            LeanTween.scale(leverImage.rectTransform, originalScale, 0.1f);
            LeanTween.rotateZ(leverImage.gameObject, originalRotationZ, 0.1f); 
        }
    }
}
