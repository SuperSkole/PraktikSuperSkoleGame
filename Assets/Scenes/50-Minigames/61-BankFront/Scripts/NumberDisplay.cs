using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Script for the number display in bank front minigame
/// </summary>
public class NumberDisplay : MonoBehaviour
{
    [SerializeField]private TextMeshPro numberDisplay;
    string number = "0";
    [SerializeField]float actualNumber;
    int decimalsWritten;
    bool writingDecimals;
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
        if (this.number == "0" && !writingDecimals)
        {
            this.number = number;
            actualNumber = float.Parse(number);
        }
        //Adds the number otherwise to display number as long as the length is below 8 to ensure it can fit on the display
        else if(this.number.Length < 8 && !writingDecimals)
        {
            this.number += number;
            actualNumber *= 10;
            actualNumber += float.Parse(number);
        }
        //Writes a decimal up to the second decimal place
        else if (writingDecimals && decimalsWritten < 2 && this.number.Length < 9)
        {
            this.number += number;
            switch(decimalsWritten)
            {
                case 0:
                    actualNumber += float.Parse(number) / 10;
                    break;
                case 1:
                    actualNumber += float.Parse(number) / 100;
                    break;
            }
            decimalsWritten++;

        }
        numberDisplay.text = this.number;
    }

    /// <summary>
    /// Resets the current number to 0
    /// </summary>
    public void ClearNumber()
    {
        number = "0";
        actualNumber = 0;
        decimalsWritten = 0;
        writingDecimals = false;
        numberDisplay.text = number;
    }

    /// <summary>
    /// Returns actual number
    /// </summary>
    /// <returns>actual number</returns>
    public float GetNumber()
    {
        return actualNumber;
    }

    /// <summary>
    /// switches to writing decimal numbers
    /// </summary>
    public void ChangeToDecimals()
    {
        if(number.Length <= 6)
        {
            number+= ",";
            numberDisplay.text = number;
            decimalsWritten = 0;
            writingDecimals = true;
        }
    }
}
