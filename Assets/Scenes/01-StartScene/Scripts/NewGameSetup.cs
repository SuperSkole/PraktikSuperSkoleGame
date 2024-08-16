using UnityEngine;
using TMPro;

public class NewGameSetup
{
    // Fields required for setting up a new game
    private Transform spawnCharPoint;
    private TMP_InputField nameInput;
    private TextMeshProUGUI playerName;
    private Color headColor, bodyColor, legColor;
    private GameObject spriteHead, spriteBody, spriteLeg;

    // Constructor
    public NewGameSetup(Transform spawnCharPoint, TMP_InputField nameInput, 
        TextMeshProUGUI playerName, Color headColor, 
        Color bodyColor, Color legColor, 
        GameObject spriteHead, GameObject spriteBody, GameObject spriteLeg)
    {
        this.spawnCharPoint = spawnCharPoint;
        this.nameInput = nameInput;
        this.playerName = playerName;
        this.headColor = headColor;
        this.bodyColor = bodyColor;
        this.legColor = legColor;
        this.spriteHead = spriteHead;
        this.spriteBody = spriteBody;
        this.spriteLeg = spriteLeg;
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