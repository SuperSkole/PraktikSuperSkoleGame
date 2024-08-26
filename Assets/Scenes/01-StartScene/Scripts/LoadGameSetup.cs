using CORE;
using LoadSave;
using Scenes.PlayerScene.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes._01_StartScene.Scripts
{
    public class LoadGameSetup : MonoBehaviour
    {
        [SerializeField] private Transform spawnCharPoint;
        [SerializeField] private Text playerNameText; 
        [SerializeField] private GameObject playerPrefab; 
        [SerializeField] private Text playerXPText;   
        [SerializeField] private Text playerGoldText; 

        public string ChosenMonsterColor;
        
        public void SetupPlayer(SaveDataDTO saveData)
        {
            // Use PlayerManager to load the player from save data
            PlayerManager.Instance.SetupPlayerFromSave(saveData);

            // Update UI with player details
            playerNameText.text = saveData.MonsterName;
            playerXPText.text = saveData.XPAmount.ToString();
            playerGoldText.text = saveData.GoldAmount.ToString();

            // Log for debugging
            Debug.Log("Player loaded and UI updated with username: " + saveData.Username +
                      " Player Name: " + saveData.MonsterName +
                      " Monster Color: " + saveData.MonsterColor +
                      " XP: " + saveData.XPAmount.ToString() +
                      " Gold: " + saveData.GoldAmount.ToString());
        }
    }
}