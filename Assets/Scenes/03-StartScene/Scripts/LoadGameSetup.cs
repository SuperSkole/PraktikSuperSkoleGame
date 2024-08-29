using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes._03_StartScene.Scripts
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
            // Log for debugging
            Debug.Log("Player loaded and UI updated with username: " + saveData.Username +
                      " Player Name: " + saveData.MonsterName +
                      " Monster Color: " + saveData.MonsterColor +
                      " XP: " + saveData.XPAmount.ToString() +
                      " Gold: " + saveData.GoldAmount.ToString());
            
            // Use PlayerManager to load the player from save data
            PlayerManager.Instance.SetupPlayerFromSave(saveData);
        }
    }
}