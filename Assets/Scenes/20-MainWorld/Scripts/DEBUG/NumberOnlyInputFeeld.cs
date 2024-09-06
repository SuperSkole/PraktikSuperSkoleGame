using System.Collections;
using System.Collections.Generic;

using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class NumberOnlyInputFeeld : MonoBehaviour
{
    [SerializeField] TMP_InputField textMeshPro;
    string oldValue = "";
    public void OnChangeValue()
    {
        string value = textMeshPro.text;
        if (int.TryParse(value,out _))
        {
            oldValue = value;
            return;
        }
        textMeshPro.text = oldValue;
    }
}
