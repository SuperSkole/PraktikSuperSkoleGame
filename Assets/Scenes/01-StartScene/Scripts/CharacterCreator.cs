using Player;
using TMPro;
using UnityEngine;

namespace Scenes.StartScene.Scripts
{
    public class CharacterCreator : MonoBehaviour
    {
        [SerializeField] private Transform characterSpawnPoint;
        [SerializeField] private GameObject girlPrefab;
        [SerializeField] private TextMeshProUGUI playerNameText;

        public PlayerData player;
        public string monsterName;

        public void CreateNewChar(string monsterName, string playerName)
        {
            GameObject prefab = girlPrefab;
            Instantiate(prefab, characterSpawnPoint.position, Quaternion.identity);
            //PlayerData newPlayer = new PlayerData(playerName, spawnCharPoint.position);
            playerNameText.text = playerName; // Update the UI element with the new player's name
            PlayerMovement.allowedToMove = true; // Allow movement, assuming player instantiation was successful

        }
        
        public void CreateNewChar(TextMeshProUGUI playerName, Color headColor, Color bodyColor, Color legColor, GameObject spriteHead, GameObject spriteBody, GameObject spriteLeg)
        {
            // player = new PlayerData(
            //     monsterName, playerNameText.text, 0, 0, 1, 
            //     characterSpawnPoint.position,
            //     headColor = Color.green, bodyColor = Color.green, legColor = Color.green,
            //     spriteHead.GetComponent<SpriteRenderer>().sprite,
            //     spriteBody.GetComponent<SpriteRenderer>().sprite,
            //     spriteLeg.GetComponent<SpriteRenderer>().sprite);
            //
            // playerName.text = player.PlayerName;
            // PlayerWorldMovement.allowedToMove = true;
        }
    }
}
