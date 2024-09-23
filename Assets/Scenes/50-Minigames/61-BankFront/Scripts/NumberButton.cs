using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script for a numberbutton in the bank front game
/// </summary>
public class NumberButton : MonoBehaviour
{
    [SerializeField]private NumberDisplay numberDisplay;
    [SerializeField]private int number;
    [SerializeField]private KeyPress keyPress;

    /// <summary>
    /// Sets the keypress onclick method to onclick
    /// </summary>
    void Start ()
    {
        keyPress.onClickMethod = OnClick;
    }

    /// <summary>
    /// Adds the number to the end of the current number in numberdisplay
    /// </summary>
    public void OnClick()
    {
        numberDisplay.AddNumber(number.ToString());
    }
}
