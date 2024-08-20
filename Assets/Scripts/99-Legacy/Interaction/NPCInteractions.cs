using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCInteractions : MonoBehaviour
{
    // The target scale to scale up to.
    [SerializeField] private Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);

    // The target scale to scale down to.
    [SerializeField] private Vector3 minScale = new Vector3(1.0f, 1.0f, 1.0f);

    // Duration of the scaling up or down.
    private float scaleDuration = 0.25f;



    public void StartScaling()
    {
        StartCoroutine(ScaleUpAndDown());
    }
    private IEnumerator ScaleUpAndDown()
    {
        // Scale up from the current scale to the maxScale
        yield return StartCoroutine(ScaleObject(transform.localScale, maxScale, scaleDuration));

        // Scale down from the maxScale to the minScale
        yield return StartCoroutine(ScaleObject(transform.localScale, minScale, scaleDuration));
    }

    // Coroutine to handle the scaling between two scales
    private IEnumerator ScaleObject(Vector3 fromScale, Vector3 toScale, float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            // Interpolate between the two scales based on the elapsed time
            transform.localScale = Vector3.Lerp(fromScale, toScale, elapsedTime / duration);

            // Increase elapsed time by the time between frames
            elapsedTime += Time.deltaTime;

            // Yield and wait for the next frame
            yield return null;
        }

        // Ensure the final scale is exactly the target scale
        transform.localScale = toScale;
    }
}
