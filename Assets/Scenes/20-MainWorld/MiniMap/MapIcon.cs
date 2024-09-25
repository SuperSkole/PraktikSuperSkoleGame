using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapIcon : MonoBehaviour
{
    [SerializeField] Sprite grov;
    [SerializeField] Sprite fabrik;
    [SerializeField] Sprite have;
    [SerializeField] Sprite racerbane;
    [SerializeField] Sprite shop;
    [SerializeField] Sprite tower;
    [SerializeField] Sprite player;
    [SerializeField] Sprite home;

    private Image thisImage;
    void Start()
    {
        thisImage = GetComponent<Image>();

        if (thisImage == null)
        {
            Debug.LogError("No Image component on map Icon.");
        }
    }
    public void ChangeImage(string imageName)
    {
        switch (imageName.ToLower())
        {
            case "grov":
                thisImage.sprite = grov;
                break;
            case "fabrik":
                thisImage.sprite = fabrik;
                break;
            case "have":
                thisImage.sprite = have;
                break;
            case "racerbane":
                thisImage.sprite = racerbane;
                break;
            case "shop":
                thisImage.sprite = shop;
                break;
            case "tower":
                thisImage.sprite = tower;
                break;
            case "player":
                thisImage.sprite = player;
                break;
            case "home":
                thisImage.sprite = home;
                break;
            default:
                Debug.LogError("Invalid image name: " + imageName);
                break;
        }
    }

    public void HighLight()
    {

    }
}
