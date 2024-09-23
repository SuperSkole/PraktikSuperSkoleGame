using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to handle the color of the error display
/// </summary>
public class ErrorDisplay: MonoBehaviour
{
    [SerializeField]private Material defaultMaterial;
    [SerializeField]private Material correctMaterial;
    [SerializeField]private Material partialCorrectMaterial;
    [SerializeField]private Material incorrectMaterial;
    [SerializeField]private MeshRenderer meshRenderer;

    /// <summary>
    /// Resets the error display back to the original color
    /// </summary>
    public void Reset()
    {
        meshRenderer.material = defaultMaterial;
    }

    /// <summary>
    /// changes the material to the correctMaterial
    /// </summary>
    public void Correct()
    {
        meshRenderer.material = correctMaterial;
    }

    /// <summary>
    /// changes the material to the partialCorrectMaterial
    /// </summary>
    public void PartialCorrect()
    {
        meshRenderer.material = partialCorrectMaterial;
    }

    /// <summary>
    /// changes the material to the incorrectMaterial
    /// </summary>
    public void Incorrect()
    {
        meshRenderer.material = incorrectMaterial;
    }
}
