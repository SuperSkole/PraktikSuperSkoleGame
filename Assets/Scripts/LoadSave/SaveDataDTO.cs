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
        // Change properties to fields by removing { get; set; }
        public string Username;
        public string Savefile;
        public string MonsterName;
        public int MonsterTypeID;
        public string MonsterColor;
        public int GoldAmount; 
        public int XPAmount; 
        public int PlayerLevel; 
        public SerializablePlayerPosition SavedPlayerStartPostion; 
        public List<string> CollectedWords;
        public List<char> CollectedLetters; 
        public List<int> BoughtClothes;
        public List<CarInfo> listOfCars;
        public string clothMid;
        public string clothTop;
    }
}