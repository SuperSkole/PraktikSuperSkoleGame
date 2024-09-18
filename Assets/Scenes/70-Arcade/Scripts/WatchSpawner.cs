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



    public string[] minuteList = { "05", "10", "15", "20", "25", "30", "35", "40", "45", "50", "55", "60" };

    public string randoMinute;
    public int randoHour;

    private void Start()
    {
        minuteText = minuteTextObject.GetComponent<TextMeshProUGUI>();
        hourText = hourTextObject.GetComponent<TextMeshProUGUI>();
    }


    public void GetRandoText()
    {
        randoMinute = minuteList[Random.Range(0, minuteList.Length)];
        minuteText.text = randoMinute;
        randoHour = Random.Range(1, 12);
        hourText.text = $"{randoHour}";
    }




}
