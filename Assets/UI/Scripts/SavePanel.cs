using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SavePanel : MonoBehaviour
{
    private Image thisImage = null;
    private Image thisName = null;
    private string thisText = null;
    private bool thisSaved = false;

    [SerializeField] Image ProfilImage;
    [SerializeField] Image Playername;
    [SerializeField] TextMeshProUGUI PlayerText;
    [SerializeField] Image BlockingImage;

    public void SetSaveData(Image image, Image name, string text, bool isSaved)
    {
        //UISaveManager bruger denne funktion til at udfylde SavePanelets variabler
        thisImage = image;
        thisName = name;
        thisText = text;
        thisSaved = isSaved;

        UpdatePanel();

    }

    public void UpdatePanel()
    {
        //Hvis saved er sand, åbnes panelet og opdatere UI save Panelet.
        if (thisSaved)
        {
            BlockingImage.enabled = false;

            ProfilImage.sprite = thisImage.sprite;
            Playername.sprite = thisName.sprite;
            PlayerText.text = thisText;
        }
    }
}
