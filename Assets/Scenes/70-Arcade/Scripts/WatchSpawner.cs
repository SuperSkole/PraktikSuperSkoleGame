using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WatchSpawner : MonoBehaviour
{
    [SerializeField] 
    private GameObject hourTextObject, minuteTextObject;

    public TextMeshProUGUI hourText;

    public TextMeshProUGUI minuteText;



    private int[] minuteList = { 05, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 00 };

    public int randoMinute;
    public int randoHour;

    private void Start()
    {
        minuteText = minuteTextObject.GetComponent<TextMeshProUGUI>();
        hourText = hourTextObject.GetComponent<TextMeshProUGUI>();

        GetRandoText();
    }


    public void GetRandoText()
    {
        randoMinute = minuteList[Random.Range(0, minuteList.Length)];
        minuteText.text = randoMinute.ToString();
        randoHour = Random.Range(1, 12);
        if (randoHour < 10)
        {
            string addZero = $"0{randoHour}";
            hourText.text = $"{addZero}";
        }
        else
        {
          hourText.text = $"{randoHour}";  
        }
        
    }




}
