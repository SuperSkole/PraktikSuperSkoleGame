using Scenes._50_Minigames._58_MiniRacingGame.Scripts;
using TMPro;
using UnityEngine;

namespace Minigames
{
    public class EndGameUI : MonoBehaviour
    {
        public static EndGameUI Instance;
    
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI xpText;
        public TextMeshProUGUI goldText;
        public TextMeshProUGUI seedText;
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

        /// <summary>
        /// Displays the reward, time taken and seed for the player.
        /// </summary>
        public void DisplayRewards(float XP, float Gold, float time, string seed)
        {
            string updatedTime;
            updatedTime = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(time / 60), Mathf.FloorToInt(time) % 60);
            xpText.text = $"XP: {XP}";
            goldText.text = $"Guld: {Gold}";
            timeText.text = $"{updatedTime}";
            seedText.text = $"Seed: {seed}";
        }

        /// <summary>
        /// Displays the reward and time taken for the player.
        /// </summary>
        public void DisplayRewards(float XP, float Gold, float time)
        {
            string updatedTime;
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
}


