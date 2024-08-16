using System.Collections.Generic;
using UnityEngine;

namespace Player
{
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
    
        // spines
        public string CharacterHead;
        public string CharacterBody;
        public string CharacterColor;
        
        public List<string> PurchasedCharactorSkins;

    
    
        // --decaprecated
        // Customization 
        public Color currentHeadColor;
        public Color currentBodyColor;
        public Color currentLegColor;
    
        public Sprite spriteHead;
        public Sprite spriteBody;
        public Sprite spriteLeg;
    
        // testing
        public void Initialize(string playerName, int goldAmount, int xpAmount, int level, Vector3 position)
        {
            PlayerName = playerName;
            CurrentGoldAmount = goldAmount;
            CurrentXPAmount = xpAmount;
            CurrentLevel = level;
            CurrentPosition = position;
        }
    
    
    
        // --- decapricated as we use mono now
        public PlayerData(
            int monsterType, 
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
            this.PlayerName = playerName;
            this.MonsterTypeID = monsterType;
            this.CurrentGoldAmount = currentGoldAmount;
            this.CurrentXPAmount = currentXPAmount;
            this.CurrentLevel = currentLevel;

            this.CurrentPosition = currentPosition;

            // this.currentHeadColor = currentHeadColor;
            // this.currentBodyColor = currentBodyColor;
            // this.currentLegColor = currentLegColor;
            //
            // this.spriteHead = spriteHead;
            // this.spriteBody = spriteBody;
            // this.spriteLeg = spriteLeg;
        }
    }
}
