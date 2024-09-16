using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinInfoToggle : MonoBehaviour
{
    [SerializeField] private GameObject coinWindow;
    [SerializeField] private BankManager bankManager;
    [SerializeField] private Button validateButton;
    [SerializeField] TMP_InputField inputField;
    private RectTransform rectTransform;
    private bool shown = false;
    private Vector2 startPos;
    private Vector2 shownPos;
    
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = coinWindow.GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        shownPos = new Vector2(startPos.x, -50);
    }

    // Update is called once per frame
    void Update()
    {
        if(shown && rectTransform.anchoredPosition != shownPos)
        {
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, shownPos, 1);
        }
        else if(!shown && rectTransform.anchoredPosition != startPos)
        {
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, startPos, 1);
        }
    }

    public void OnClick()
    {
        shown = !shown;
        if(shown)
        {
            foreach(Coin coin in bankManager.currentCustomersCoins)
            {
                coin.gameObject.SetActive(false);
                validateButton.interactable = false;
                inputField.interactable = false;
            }
        }
        else
        {
            foreach(Coin coin in bankManager.currentCustomersCoins)
            {
                coin.gameObject.SetActive(true);
                validateButton.interactable = true;
                inputField.interactable = true;
            }
        }
    }
}