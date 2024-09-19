using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CORE.Scripts
{
    /// <summary>
    /// USed to limit the length of a text input
    /// </summary>
    public class InputLengthLimit : MonoBehaviour
    {
        [SerializeField]private TMP_InputField input;
        [SerializeField]private int maxLength;

        string oldValue = "";
        
        /// <summary>
        /// Checks the length of text of the input field. If it is larger than maxLength it gets replaced with the last value short enough
        /// </summary>
        public void OnChangeValue()
        {
            string value = input.text;
            if (value.Length <= maxLength)
            {
                oldValue = value;
                return;
            }
            input.text = oldValue;
        }
    }
}

