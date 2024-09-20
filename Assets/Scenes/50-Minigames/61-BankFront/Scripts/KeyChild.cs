using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script to handle keypress for child objects of objects with the Keypress class
/// </summary>
public class KeyChild : MonoBehaviour
{
    [SerializeField]private KeyPress parentsKeyPress;
    /// <summary>
    /// Sends the click to their parent
    /// </summary>
    void OnMouseDown()
    {
        parentsKeyPress.OnClick();
    }   
}
