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
        
        public void SetupPlayer(PlayerData saveData)
        {
            // Use PlayerManager to load the player from save data
            PlayerManager.Instance.SetupPlayerFromSave(saveData);
        }
    }
}