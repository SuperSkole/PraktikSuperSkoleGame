using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shopoption : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] string itemName;

    private ShopManager shopManager;

    private void Awake()
    {
        shopManager = FindObjectOfType<ShopManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        shopManager.Click(itemName);
    }

}
