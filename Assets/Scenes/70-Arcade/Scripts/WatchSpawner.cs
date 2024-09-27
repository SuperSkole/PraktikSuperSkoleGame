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



    private string[] minuteList = { "05", "10", "15", "20", "25", "30", "35", "40", "45", "50", "55", "00" };

    public string randoMinute;
    public int randoHour;

    private void Start()
    {
        minuteText = minuteTextObject.GetComponent<TextMeshProUGUI>();
        hourText = hourTextObject.GetComponent<TextMeshProUGUI>();

        GetRandoText();
    }

    /// <summary>
    /// Gets random text for the digiclock.
    /// </summary>
    public void GetRandoText()
    {
        randoMinute = minuteList[Random.Range(0, minuteList.Length)];
        minuteText.text = randoMinute;
        randoHour = Random.Range(0, 12);
        if (randoHour < 10)
        {
            string addZero = $"0{randoHour}";
            hourText.text = $"{addZero}";
            if(Random.Range(0, 2) == 1)
            {
                hourText.text = $"{addZero + 12}";
            }
        }
        else
        {
          hourText.text = $"{randoHour}";  
        }
        
    }




}
