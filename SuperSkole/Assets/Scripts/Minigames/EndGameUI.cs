using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndGameUI : MonoBehaviour
{
    public static EndGameUI Instance;
    
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI goldText;
    public GameObject endGameUIPanel; // Parent GameObject for all end-game UI elements
    

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Initially hide the end game UI
        ToggleEndGameUI(false);
    }

    public void DisplayRewards(float XP, float Gold, float time)
    {
        string updatedTime;
        //updatedTime = time.ToString("0.0"); 
        updatedTime = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(time / 60), Mathf.FloorToInt(time) % 60);
        xpText.text = $"XP: {XP}";
        goldText.text = $"Guld: {Gold}";
        timeText.text = $"{updatedTime}";

    }

    public void ToggleEndGameUI(bool visible)
    {
        endGameUIPanel.SetActive(visible);
    }
}


