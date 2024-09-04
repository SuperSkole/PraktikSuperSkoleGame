using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class IconInteraction : MonoBehaviour
{
    private Vector3 orginalScale;

    private void Awake()
    {
        orginalScale = transform.localScale;
    }
    private void OnMouseEnter()
    {
        StartCoroutine(JiggleAndLightUp());
        GetComponentInParent<SpinePlayerMovement>().hoveringOverUI = true;

    }
    private void OnMouseExit()
    {
        GetComponentInParent<SpinePlayerMovement>().hoveringOverUI = false;
        ResetScale();
    }
    private void OnMouseDown()
    {
        GetComponentInParent<PlayerEventManager>().InvokeAction();
    }
    private IEnumerator JiggleAndLightUp()
    {
        // Større
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
