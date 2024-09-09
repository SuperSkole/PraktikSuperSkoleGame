using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Scenes._20_MainWorld.Scripts.Car;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReFuelCar : MonoBehaviour
{
    private PlayerData playerData;

    // Cars Fuelamount goes from 0 to 1
    private CarFuelMangent carFuelMa;

    private float incrementIncrease = 0.2f;
    private int lettersCount;

    [SerializeField] private TextMeshProUGUI letterAmountTxt;
    [SerializeField] private Image imgFillBar;

    private void Awake()
    {
        playerData = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>();
        carFuelMa = GameObject.Find("Prometheus Variant").GetComponent<CarFuelMangent>();
    }
    private void OnEnable()
    {
        UpdateValues();
    }
    private void UpdateValues()
    {
        imgFillBar.fillAmount = carFuelMa.FuelAmount;
        letterAmountTxt.text = (playerData.CollectedLetters.Count
            + ReturnAmountOfLettersInWords(playerData.CollectedWords)).ToString();
    }
    public void OneWordRefill()
    {
        string whereGetLetters;
        if (playerData.CollectedLetters.Count > 0)
        {
            whereGetLetters = "Letters";
            lettersCount = playerData.CollectedLetters.Count;
        }
        else
        {
            whereGetLetters = "Words";
            lettersCount = ReturnAmountOfLettersInWords(playerData.CollectedWords);
        }

        if (lettersCount > 0)
        {
            //var howManyIncrements = Mathf.Ceil(missingAmount / incrementIncrease);
            carFuelMa.FuelAmount += incrementIncrease;
            switch (whereGetLetters)
            {
                case "Letters":
                    playerData.CollectedLetters.RemoveAt(0);
                    break;
                case "Words":
                    playerData.CollectedWords.RemoveAt(0);
                    break;
            }
            UpdateValues();
        }
        else
        {
            print("Not enough letters to refuel");
        }
    }
    /// <summary>
    /// Refules the players car if they have enough words, looks thourgh players collected woreds and letters, to see
    /// exchange rate is 1 letter for 0.2 fuel
    /// </summary>
    public void FullTankRefuelCar()
    {
        string whereGetLetters;
        if (playerData.CollectedLetters.Count > 0)
        {
            whereGetLetters = "Letters";
            lettersCount = playerData.CollectedLetters.Count;
        }
        else
        {
            whereGetLetters = "Words";
            lettersCount = ReturnAmountOfLettersInWords(playerData.CollectedWords);
        }

        if (lettersCount > 0)
        {
            var missingAmount = 1 - carFuelMa.FuelAmount;
            var howManyIncrements = Mathf.Ceil(missingAmount / incrementIncrease);

            for (int i = 0; i < howManyIncrements; i++)
            {
                if (lettersCount > 0)
                {
                    carFuelMa.FuelAmount += incrementIncrease;
                    lettersCount--;
                    switch (whereGetLetters)
                    {
                        case "Letters":
                            playerData.CollectedLetters.RemoveAt(0);
                            break;
                        case "Words":
                            playerData.CollectedWords.RemoveAt(0);
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
            UpdateValues();
        }
        else
        {
            print("Not enough letters to refuel");
        }

    }
    /// <summary>
    /// Returns the amount of chars in a given string list
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private int ReturnAmountOfLettersInWords(List<string> list)
    {
        int lettersCount = 0;

        for (int i = 0; i < list.Count; i++)
        {
            foreach (var item in list[i])
            {
                lettersCount++;
            }
        }
        return lettersCount;
    }
}
