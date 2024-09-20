using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the validate button in the bank front minigame
/// </summary>
public class ValidateButton : MonoBehaviour
{
    [SerializeField]private KeyPress keyPress;
    [SerializeField]private BankManager bankManager;

    /// <summary>
    /// sets the onclickmethod of keypress to OnClick
    /// </summary>
    void Start()
    {
        keyPress.onClickMethod = OnClick;
    }
    
    /// <summary>
    /// Asks the bank manager to begin validating the players guess
    /// </summary>
    public void OnClick()
    {
        bankManager.Validate();
    }
}
