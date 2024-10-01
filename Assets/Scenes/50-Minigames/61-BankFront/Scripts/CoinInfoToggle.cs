using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to handle the coin info window in the bank main entrance minigame
/// </summary>
public class CoinInfoToggle : MonoBehaviour
{
    [SerializeField] private GameObject coinWindow;
    [SerializeField] private BankManager bankManager;
    [SerializeField] private TextMeshProUGUI text;
    private RectTransform rectTransform;
    private bool shown = false;

    private bool alwaysShown = false;
    private Vector2 startPos;
    private Vector2 shownPos;
    
    /// <summary>
    /// Sets up various variables
    /// </summary>
    void Start()
    {
        rectTransform = coinWindow.GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        shownPos = new Vector2(startPos.x, -50);
    }

    /// <summary>
    /// Moves the window up and down as needed
    /// </summary>
    void Update()
    {
        if((shown || alwaysShown) && rectTransform.anchoredPosition != shownPos)
        {
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, shownPos, 1);
        }
        else if(!alwaysShown && !shown && rectTransform.anchoredPosition != startPos)
        {
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, startPos, 1);
        }
    }

    /// <summary>
    /// Changes whether the window is shown or hidden. Also hides the coins and disables the validatebutton and the inputfield when it is shown and enables them again when it is hidden
    /// </summary>
    public void OnClick()
    {
        shown = !shown;
        if(!alwaysShown && shown)
        {
            foreach(Coin coin in bankManager.gamemode.GetCurrentCustomersCoins())
            {
                coin.gameObject.SetActive(false);
            }
        }
        else if(!alwaysShown)
        {
            foreach(Coin coin in bankManager.gamemode.GetCurrentCustomersCoins())
            {
                coin.gameObject.SetActive(true);
            }
        }
    }

    public void ToggleAlwaysDisplay()
    {
        alwaysShown = true;
        text.text = "Gyldige m\u00F8nter";
    }
}