using System.Collections.Generic;
using UnityEngine;

namespace LoadSave
{
    /// <summary>
    /// Holds all the information about the player, is used for saving 
    /// </summary>

    public class PlayerData : MonoBehaviour
    {
        // Player and character Data
        public string Username { get; set; }
        public string Savefile { get; set; }

        public string PlayerName { get; set; }
        
        public int MonsterTypeID { get; set; }
        public string MonsterColor { get; set; }
        public int CurrentGoldAmount { get; set; }
        public int CurrentXPAmount { get; set; }
        public int CurrentLevel { get; set; }

        // Positional data
        public Vector3 CurrentPosition { get; set; }

        // activa words
        // Progression
        // collection of words the player has made
        // collection of letter
        // maybe dict so we also can store datetime
        public List<string> CollectedWords { get; set; }
        public List<char> CollectedLetters { get; set; }

        public void Initialize(string username, string playerName, string monsterColor, int goldAmount, int xpAmount, int level, Vector3 position)
        {
            Username = username;
            PlayerName = playerName;
            MonsterColor = monsterColor;
            CurrentGoldAmount = goldAmount;
            CurrentXPAmount = xpAmount;
            CurrentLevel = level;
            CurrentPosition = position;
        }
    }
}
