using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shopoption : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] string itemName;
    [SerializeField] int Price;
    [SerializeField] TextMeshProUGUI priceText;

    private ShopManager shopManager;

    private void Awake()
    {
        shopManager = FindObjectOfType<ShopManager>();

        priceText.text = Price.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //skal lige overfører pris også
        shopManager.Click(itemName, Price, this);
    }

}
