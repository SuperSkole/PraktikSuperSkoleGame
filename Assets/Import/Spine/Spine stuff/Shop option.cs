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

    Image imageComponent;
    Outline outlineComponent;

    private ShopManager shopManager;

    private void Awake()
    {
        shopManager = FindObjectOfType<ShopManager>();
        imageComponent = GetComponent<Image>();
        outlineComponent = GetComponent<Outline>();
        priceText.text = Price.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //skal lige overf�rer pris ogs�
        shopManager.Click(itemName, Price, this);
        imageComponent.enabled = true;
        outlineComponent.enabled = true;
    }

    public void UnSelect()
    {
        imageComponent.enabled = false;
        outlineComponent.enabled = false;
    }

}
