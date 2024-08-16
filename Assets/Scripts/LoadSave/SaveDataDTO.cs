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
        public string HashedUsername;
        public string Savefile;
        
        public string PlayerName; 
        public string MonsterType; 
        public int GoldAmount; 
        public int XPAmount; 
        public int PlayerLevel; 

        // Positional data
        public SavePlayerPosition SavedPlayerStartPostion;  // evt default to house
        
        // achived words
        // Progression
        // collection of words the player has made
        // collection of letter 
        // how many times a word/letter has been done
        // maybe dict so we also can store datetime
        public List<string> CollectedWords;
        public List<char> CollectedLetters;
        

        // spines
        public string CharacterHead;
        public string CharacterBody;
        public string CharacterColor;
        
        public List<string> PurchasedCharactorSkins; // list of purchased 
        
        // Customization colors
        public SerializableColor HeadColor; 
        public SerializableColor BodyColor;
        public SerializableColor LegColor; 

        //Skins data
        public List<SkinData> GirlPurchasedSkins; 
        public List<SkinData> MonsterPurchasedSkins;
        
    }
}