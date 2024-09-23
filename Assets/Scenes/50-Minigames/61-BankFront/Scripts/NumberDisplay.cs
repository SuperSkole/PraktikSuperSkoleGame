using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Script for the number display in bank front minigame
/// </summary>
public class NumberDisplay : MonoBehaviour
{
    [SerializeField]private TextMeshPro numberDisplay;
    string number = "0";
    /// <summary>
    /// Sets the displayed number to 0;
    /// </summary>
    void Start()
    {
        numberDisplay.text = number;
    }

    /// <summary>
    /// Adds a number to the end of the displaynumber
    /// </summary>
    /// <param name="number">the number to be added</param>
    public void AddNumber(string number)
    {
        //Ensures that the highest number cant be 0
        if (this.number == "0")
        {
            this.number = number;
        }
        //Adds the number otherwise to display number as long as the length is below 8 to ensure it can fit on the display
        else if(this.number.Length < 8)
        {
            this.number += number;
        }
        numberDisplay.text = this.number;
    }

    /// <summary>
    /// Resets the current number to 0
    /// </summary>
    public void ClearNumber()
    {
        number = "0";
        numberDisplay.text = number;
    }

    /// <summary>
    /// Returns the number displays number as an int
    /// </summary>
    /// <returns>the number displays number as an int</returns>
    public int GetNumber()
    {
        bool res = Int32.TryParse(this.number, out int number);
        if (res)
        {
            return number;
        }
        else 
        {
            Debug.LogError("Numberdisplay did not contain a number");
            return 0;
        }
    }
}
