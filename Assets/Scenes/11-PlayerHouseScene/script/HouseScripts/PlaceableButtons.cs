using Scenes._11_PlayerHouseScene.script.HouseScripts;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaceableButtons : MonoBehaviour
{
    public EnumFloorDataType FloorType;
    public int ID;

    public int amount = 0;

    public TextMeshProUGUI amountTxt;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>().StartPlacement(GetComponent<PlaceableButtons>()));
    }
    public void StartUpValues(int value)
    {
        var tmpAmount = value;

        amount = tmpAmount;
        amountTxt.text = tmpAmount.ToString();
    }
    public void ChangeValue(int value)
    {
        var tmpAmount = Convert.ToInt32(amountTxt.text);
        tmpAmount += value;

        amount = tmpAmount;
        amountTxt.text = tmpAmount.ToString();
        if (tmpAmount <= 0)
        {
            Destroy(gameObject);
        }

    }


}
