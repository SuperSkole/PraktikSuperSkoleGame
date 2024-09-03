using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WardrobeOption : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] Image profilImage;

    //private Image profilImage;
    private string SpineName;

    Image imageComponent;
    Outline outlineComponent;

    private Wardrop wardrop;

    private void Awake()
    {
        wardrop = FindObjectOfType<Wardrop>();
        imageComponent = GetComponent<Image>();
        outlineComponent = GetComponent<Outline>();
    }

    public void Initialize(string newItemName, Sprite newImage, string newSpineName)
    {
        profilImage.sprite = newImage;
        SpineName = newSpineName;
        Name.text = newItemName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        wardrop.Click(SpineName,this);
        imageComponent.enabled = true;
        outlineComponent.enabled = true;
    }

    public void UnSelect()
    {
        imageComponent.enabled = false;
        outlineComponent.enabled = false;
    }
}
