using UnityEngine;
/// <summary>
/// Holds all the information about the player, is used for saving 
/// </summary>
[System.Serializable]
public class PlayerData
{
    // Player and character Data
    public string HashedUsername;
    public string playerName;
    public string monsterName;
    public int currentGoldAmount;
    public int currentXPAmount;
    public int currentLevel;

    // Positional data
    public Vector3 currentPosition;

    // Customization 
    public Color CurrentHeadColor;
    public Color CurrentBodyColor;
    public Color CurrentLegColor;

    public Sprite spriteHead;
    public Sprite spriteBody;
    public Sprite spriteLeg;
    
    public PlayerData(
        string monsterName, 
        string playerName, 
        int currentGoldAmount, 
        int currentXPAmount, 
        int currentLevel, 
        Vector3 currentPosition,        
        Color CurrentHeadColor, 
        Color CurrentBodyColor, 
        Color CurrentLegColor, 
        Sprite spriteHead, 
        Sprite spriteBody, 
        Sprite spriteLeg)
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
