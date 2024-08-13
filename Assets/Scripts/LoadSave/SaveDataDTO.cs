using System.Collections.Generic;

namespace LoadSave
{
    /// <summary>
    /// This class acts as a Data Transfer Object for game state serialization.
    /// It strictly contains fields that represent the state to be saved.
    /// </summary>
    [System.Serializable]
    public class SaveDataDTO 
    {
        // Player and character Data
        public string HashedUsername;
        public string PlayerName; 
        public string MonsterName; 
        public int GoldAmount; 
        public int XPAmount; 
        public int PlayerLevel; 

        // Positional data
        public SavePlayerPosition SavedPlayerStartPostion; 

        // Customization colors
        public SerializableColor HeadColor; 
        public SerializableColor BodyColor;
        public SerializableColor LegColor; 

        // Skins data
        public List<SkinData> PurchasedCharactorSkins;
        // public List<SkinData> GirlPurchasedSkins; 
        // public List<SkinData> MonsterPurchasedSkins;
    }
}