using CORE;
using LoadSave;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._01_StartScene.Scripts
{
    public class NewGameSetup : MonoBehaviour
    {
        // Fields required for setting up a new game
        [SerializeField] private Transform spawnCharPoint;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TextMeshProUGUI playerName;

        [SerializeField] private GameObject playerPrefab;

        public string ChosenMonsterColor;
        
        public void OnClick()
        {
            SetupPlayer();
            SceneManager.LoadScene("02-PlayerHouse");
        }

        public void SetupPlayer()
        {
            // instantiate temp object in scene
            GameObject loadedPlayer = Instantiate(playerPrefab, spawnCharPoint.position, Quaternion.identity, spawnCharPoint);

            PlayerData player = loadedPlayer.AddComponent<PlayerData>();

            // Init player data
            player.Initialize(
                GameManager.Instance.CurrentUsername,
                nameInput.text, 
                ChosenMonsterColor,
                0,
                0,
                0,
                spawnCharPoint.position
            );

            // Log for debugging
            Debug.Log("Player setup complete with name: " + player.PlayerName +
                      "Player Name: " + player.PlayerName +
                      "XP: " + player.CurrentXPAmount.ToString() +
                      "Gold: " + player.CurrentGoldAmount.ToString());
            
            GameManager.Instance.PlayerData = player;
        }
    }
}