using Import.LeanTween.Framework;
using System.Collections;
using UnityEngine;

namespace Scenes._10_PlayerScene.Scripts
{
    public class IconInteraction : MonoBehaviour
    {
        private Vector3 orginalScale;
        [SerializeField] private bool isPlayer;

        private void Awake()
        {
        }
        private void OnMouseEnter()
        {
            orginalScale = transform.localScale;
            StartCoroutine(JiggleAndLightUp());
            if (isPlayer)
            {
                GetComponentInParent<SpinePlayerMovement>().hoveringOverUI = true;
            }
            else
            {

            }
        }
        private void OnMouseExit()
        {
            if (isPlayer)
            {
                GetComponentInParent<SpinePlayerMovement>().hoveringOverUI = false;
            }
            else
            {

            }
            ResetScale();
        }
        private void OnMouseDown()
        {
            ResetScale();
            if (isPlayer)
            {
                GetComponentInParent<SpinePlayerMovement>().hoveringOverUI = false;
                GetComponentInParent<PlayerEventManager>().InvokeAction();
            }
            else
            {
                GetComponentInParent<CarEventsManager>().InvokeAction();

            }
        }
        private IEnumerator JiggleAndLightUp()
        {
            // St�rre
            LeanTween.scale(gameObject, orginalScale * 1.2f, 0.1f);

            // Jiggle effect
            LeanTween.rotateZ(gameObject, 10f, 0.1f).setLoopPingPong(2);

            yield return new WaitForSeconds(0.2f);
        }
        // Reset
        private void ResetScale()
        {
            LeanTween.scale(gameObject, orginalScale, 0.1f);
            LeanTween.rotateZ(gameObject, 0f, 0f);
        }

    }
}
