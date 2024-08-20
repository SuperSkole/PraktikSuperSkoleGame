using System.Collections.Generic;
using UnityEngine;

namespace LoadSave
{
    /// <summary>
    /// Holds all the information about the player, is used for saving.
    /// </summary>
    public class PlayerData : MonoBehaviour
    {
        // Serialize properties to make them visible in the Inspector.
        [SerializeField] private string username;
        [SerializeField] private string savefile;
        [SerializeField] private string playerName;
        [SerializeField] private int monsterTypeID;
        [SerializeField] private string monsterColor;
        [SerializeField] private int currentGoldAmount;
        [SerializeField] private int currentXPAmount;
        [SerializeField] private int currentLevel;
        [SerializeField] private Vector3 currentPosition;

        // Lists can be shown directly if they are public or marked with [SerializeField].
        public List<string> CollectedWords = new List<string>();
        public List<char> CollectedLetters = new List<char>();

        // Public properties to access private serialized fields.
        public string Username { get => username; set => username = value; }
        public string Savefile { get => savefile; set => savefile = value; }
        public string PlayerName { get => playerName; set => playerName = value; }
        public int MonsterTypeID { get => monsterTypeID; set => monsterTypeID = value; }
        public string MonsterColor { get => monsterColor; set => monsterColor = value; }
        public int CurrentGoldAmount { get => currentGoldAmount; set => currentGoldAmount = value; }
        public int CurrentXPAmount { get => currentXPAmount; set => currentXPAmount = value; }
        public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
        public Vector3 CurrentPosition { get => currentPosition; set => currentPosition = value; }

        public void Initialize(string username, string playerName, string monsterColor, int goldAmount, int xpAmount, int level, Vector3 position)
        {
            this.username = username;
            this.playerName = playerName;
            this.monsterColor = monsterColor;
            this.currentGoldAmount = goldAmount;
            this.currentXPAmount = xpAmount;
            this.currentLevel = level;
            this.currentPosition = position;
        }
    }
}
