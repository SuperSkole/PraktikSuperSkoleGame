using CORE;
using LoadSave;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.StartScene.Scripts
{
    public class LoadGameSetup : MonoBehaviour
    {
        public Transform spawnCharPoint;
        public GameObject playerPrefab; 
        public Text playerNameText; 
        public Text playerXPText;   
        public Text playerGoldText; 

        public void SetupPlayer(SaveDataDTO saveData)
        {
            // instantiate temp object in scene
            GameObject loadedPlayer = Instantiate(playerPrefab, spawnCharPoint.position, Quaternion.identity, spawnCharPoint);

            PlayerData player = loadedPlayer.AddComponent<PlayerData>();

            // Init player data
            player.Initialize(
                saveData.Username,
                saveData.PlayerName, 
                saveData.MonsterColor,
                saveData.GoldAmount,
                saveData.XPAmount,
                saveData.PlayerLevel,
                saveData.SavedPlayerStartPostion.GetVector3()
            );
            
            GameManager.Instance.PlayerData = player;

            // Log for debugging
            Debug.Log("Player setup complete with username: " + player.Username +
                      " Player Name: " + player.PlayerName +
                      " Monster Color: " + player.MonsterColor +
                      " XP: " + player.CurrentXPAmount.ToString() +
                      " Gold: " + player.CurrentGoldAmount.ToString());
        }
    }
}