using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the clear button in Bank front minigame
/// </summary>
public class ClearButton : MonoBehaviour
{
    [SerializeField]private KeyPress keyPress;
    [SerializeField]private NumberDisplay numberDisplay;
    /// <summary>
    /// Sets the onclick method of keypress
    /// </summary>
    void Start()
    {
        keyPress.onClickMethod = OnClick;
    }

    /// <summary>
    /// Clears the current number of the number display
    /// </summary>
    public void OnClick()
    {
        numberDisplay.ClearNumber();
    }
}
