using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarFuel : MonoBehaviour
{
    public Image gaugeImg;
    public float fuelAmount { get; set; }

    private void Awake()
    {
        fuelAmount = 1.0f;
    }
    private void Update()
    { 
        gaugeImg.fillAmount = fuelAmount;
    }
}
