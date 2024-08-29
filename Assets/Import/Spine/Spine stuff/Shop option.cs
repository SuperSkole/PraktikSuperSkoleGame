using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shopoption : MonoBehaviour, IPointerClickHandler
{
    private string itemName;
    private int Price;
    [SerializeField] TextMeshProUGUI priceText;
    private Image profilImage;
    private string SpineName;

    public int ID;

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

    public void Initialize(string newItemName, int newPrice, Sprite newImage, string newSpineName, int newID)
    {
        itemName = newItemName;
        Price = newPrice;
        profilImage.sprite = newImage;
        SpineName = newSpineName;

        if (priceText != null)
        {
            priceText.text = Price.ToString();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //skal lige overfører pris også
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
