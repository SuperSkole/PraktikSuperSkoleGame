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

    void Start ()
    {
        keyPress.onClickMethod = OnClick;
    }

    public void OnClick()
    {
        numberDisplay.AddNumber(number.ToString());
    }
}
