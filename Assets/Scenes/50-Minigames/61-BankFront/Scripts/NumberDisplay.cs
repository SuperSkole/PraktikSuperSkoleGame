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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNumber(string number)
    {
        if (this.number == "0")
        {
            this.number = number;
        }
        else if(this.number.Length < 8)
        {
            this.number += number;
        }
        numberDisplay.text = this.number;
    }

    public void ClearNumber()
    {
        number = "0";
        numberDisplay.text = number;
    }

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
