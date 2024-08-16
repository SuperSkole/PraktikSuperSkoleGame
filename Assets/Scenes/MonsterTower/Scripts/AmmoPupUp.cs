using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoPupUp : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textFeld;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    /// <summary>
    /// sets the tooltip and shows it
    /// </summary>
    /// <param name="text">the text the tooltip is set to</param>
    public void SetAndShowToolTip(string text)
    {
        textFeld.text = text;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// hides the tooltip
    /// </summary>
    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
