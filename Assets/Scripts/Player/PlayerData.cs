using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Holds all the information about the player, is used for saving 
/// </summary>
[System.Serializable]
public class PlayerData
{
    // Player and character Data
    public string hashedUsername;
    public string playerName;
    public string monsterName;
    public int currentGoldAmount;
    public int currentXPAmount;
    public int currentLevel;

    // Positional data
    public Vector3 currentPosition;

    // Customization 
    public Color currentHeadColor;
    public Color currentBodyColor;
    public Color currentLegColor;

    public Sprite spriteHead;
    public Sprite spriteBody;
    public Sprite spriteLeg;
    
    // Progression
    // collection of words the player has made
    // collection of letter
    // maybe dict so we also can store datetime
    public List<string> CollectedWords;
    public List<char> CollectedLetters;
    
    
    
    public PlayerData(
        string monsterName, 
        string playerName, 
        int currentGoldAmount, 
        int currentXPAmount, 
        int currentLevel, 
        Vector3 currentPosition,        
        Color currentHeadColor, 
        Color currentBodyColor, 
        Color currentLegColor, 
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

        this.currentHeadColor = currentHeadColor;
        this.currentBodyColor = currentBodyColor;
        this.currentLegColor = currentLegColor;

        this.spriteHead = spriteHead;
        this.spriteBody = spriteBody;
        this.spriteLeg = spriteLeg;
    }
}
