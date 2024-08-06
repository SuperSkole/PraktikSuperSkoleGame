using UnityEngine;
/// <summary>
/// Holds all the information about the player, is mostly used for saving 
/// </summary>
[System.Serializable]
public class CharacterController
{

    public string playerName;
    public string monsterName;

    public int currentGoldAmount;
    public int currentXPAmount;
    public int currentLevel;

    public Vector3 currentPosition;


    public Color CurrentHeadColor;
    public Color CurrentBodyColor;
    public Color CurrentLegColor;

    public Sprite spriteHead;
    public Sprite spriteBody;
    public Sprite spriteLeg;

    // Default constructor required for serialization
    public CharacterController() { }
    
    public CharacterController(string monsterName, string playerName, int currentGoldAmount, 
        int currentXPAmount, int currentLevel, Vector3 currentPosition,        
        Color CurrentHeadColor, Color CurrentBodyColor, Color CurrentLegColor, Sprite spriteHead, Sprite spriteBody, Sprite spriteLeg)
    {
        this.playerName = playerName;
        this.monsterName = monsterName;
        this.currentGoldAmount = currentGoldAmount;
        this.currentXPAmount = currentXPAmount;
        this.currentLevel = currentLevel;

        this.currentPosition = currentPosition;

        this.CurrentHeadColor = CurrentHeadColor;
        this.CurrentBodyColor = CurrentBodyColor;
        this.CurrentLegColor = CurrentLegColor;

        this.spriteHead = spriteHead;
        this.spriteBody = spriteBody;
        this.spriteLeg = spriteLeg;
    }
   
}
