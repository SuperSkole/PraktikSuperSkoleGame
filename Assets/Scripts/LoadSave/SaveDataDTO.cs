using System.Collections.Generic;

namespace LoadSave
{
    /// <summary>
    /// This class acts as a Data Transfer Object for game state serialization.
    /// It contains fields that represent the state to be saved.
    /// </summary>
    [System.Serializable]
    public class SaveDataDTO 
    {
        // Player and character Data
        public string Username { get; set; }
        public string Savefile { get; set; }

        public string PlayerName { get; set; }
        
        public int MonsterTypeID { get; set; }
        public string MonsterColor { get; set; }
        public int GoldAmount { get; set; } 
        public int XPAmount { get; set; } 
        public int PlayerLevel { get; set; } 

        // Positional data
        public SavePlayerPosition SavedPlayerStartPostion { get; set; }  // evt default to house
        
        // achived words
        // Progression
        // collection of words the player has made
        // collection of letter 
        // how many times a word/letter has been done
        // maybe dict so we also can store datetime
        public List<string> CollectedWords { get; set; }
        public List<char> CollectedLetters { get; set; }
    }
}