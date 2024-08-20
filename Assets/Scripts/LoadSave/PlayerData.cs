using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all the information about the player, is used for saving 
/// </summary>

public class PlayerData : MonoBehaviour
{
    // Player and character Data
    public string HashedUsername;
    public string Savefile;

    public string PlayerName;
    public int MonsterTypeID;
    public string MonsterColor;
    public int CurrentGoldAmount;
    public int CurrentXPAmount;
    public int CurrentLevel;

    // Positional data
    public Vector3 CurrentPosition;

    // activa words
    // Progression
    // collection of words the player has made
    // collection of letter
    // maybe dict so we also can store datetime
    public List<string> CollectedWords;
    public List<char> CollectedLetters;

    public void Initialize(string playerName, string monsterColor, int goldAmount, int xpAmount, int level, Vector3 position)
    {
        PlayerName = playerName;
        MonsterColor = monsterColor;
        CurrentGoldAmount = goldAmount;
        CurrentXPAmount = xpAmount;
        CurrentLevel = level;
        CurrentPosition = position;
    }
}
