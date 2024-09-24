using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterBox : MonoBehaviour
{

    [SerializeField]
    private GameObject letterBoxText;

    public TextMeshProUGUI letterText;

    

    // Start is called before the first frame update
    void Start()
    {
        letterText = letterBoxText.GetComponent<TextMeshProUGUI>();
    }

 
    public void GetLetter(string letter)
    {
        letterText.text = letter;
    }


}
