using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HouseItemsBuying : MonoBehaviour
{
    public int ID;
    public int Price;

    [SerializeField] private TextMeshProUGUI priceTxt;

    private void Start()
    {
        priceTxt.text = Price.ToString();
    }
}
