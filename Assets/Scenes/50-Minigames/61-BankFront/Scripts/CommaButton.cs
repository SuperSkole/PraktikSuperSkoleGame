using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommaButton : MonoBehaviour
{
    [SerializeField] private NumberDisplay numberDisplay;
    [SerializeField] private KeyPress keyPress;
    /// <summary>
    /// Sets the keyRress onClickMethod to OnClick
    /// </summary>
    void Start()
    {
        keyPress.onClickMethod = OnClick;
    }

    /// <summary>
    /// Calls the numberDisplay methd ChangeToDecimals
    /// </summary>
    public void OnClick()
    {
        numberDisplay.ChangeToDecimals();
    }

    
}
