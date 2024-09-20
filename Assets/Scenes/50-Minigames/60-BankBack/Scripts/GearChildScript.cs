using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Script to make gear child objects act as part of their parent object when clicked on
/// </summary>
public class GearChildScript : MonoBehaviour
{
    public GearScript parentScript;
    
    /// <summary>
    /// Sends the mouseclick to their parent
    /// </summary>
    void OnMouseDown()
    {
        parentScript.OnMouseDown();
    }
}
