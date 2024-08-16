using CORE;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class NewGameSetup : MonoBehaviour
{
    // Fields required for setting up a new game
    [SerializeField] private Transform spawnCharPoint;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TextMeshProUGUI playerName;

    [SerializeField] private GameObject playerPrefab;

    public void SetupPlayer()
    {
        // instantiate temp object in scene
        GameObject loadedPlayer = Instantiate(playerPrefab, spawnCharPoint.position, Quaternion.identity, spawnCharPoint);

        PlayerData player = loadedPlayer.AddComponent<PlayerData>();

        // Init player data
        player.Initialize(
            "Randomness",
            0,
            0,
            0,
            spawnCharPoint.position
        );

        // // add UI info
        // playerNameText.text = "Player Name: " + player.PlayerName;
        // playerXPText.text = "XP: " + player.CurrentXPAmount.ToString();
        // playerGoldText.text = "Gold: " + player.CurrentGoldAmount.ToString();

        // Log for debugging
        Debug.Log("Player setup complete with name: " + player.PlayerName +
                  "Player Name: " + player.PlayerName +
                  "XP: " + player.CurrentXPAmount.ToString() +
                  "Gold: " + player.CurrentGoldAmount.ToString());
        GameManager.Instance.PlayerData = player;
    }

    public void OnClick()
    {
        SetupPlayer();
        SceneManager.LoadScene($"02-PlayerHouse");
    }
    // Method to create a new character
    //public PlayerData CreateNewChar()
    // {
    //     PlayerData player = new PlayerData(
    //         "NewMonster", 
    //         nameInput.text, 
    //         0, 0, 1, // Gold, XP, Level
    //         spawnCharPoint.position,
    //         headColor, bodyColor, legColor,
    //         spriteHead.GetComponent<SpriteRenderer>().sprite,
    //         spriteBody.GetComponent<SpriteRenderer>().sprite,
    //         spriteLeg.GetComponent<SpriteRenderer>().sprite);
    //
    //     playerName.text = player.PlayerName;
    //     return player;
    // }
}