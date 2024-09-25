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
        // player Stats
        private string username;
        private string monsterName;
        private int monsterTypeID;
        private string monsterColor;
        private int currentGoldAmount;
        private int currentXPAmount;
        private int currentLevel;
        private SerializablePlayerPosition currentPosition;

        // Words and letters
        public List<string> CollectedWords;
        public List<char> CollectedLetters;
        private int lifetimeTotalWords;
        private int lifetimeTotalLetters;

        // Clothing
        private string clothMid;
        private string clothTop;
        private List<int> boughtClothes;

        // Cars
        private List<CarInfo> listOfCars;

        // Properties for encapsulation
        public string Username { get => username; set => username = value; }
        public string MonsterName { get => monsterName; set => monsterName = value; }
        public int MonsterTypeID { get => monsterTypeID; set => monsterTypeID = value; }
        public string MonsterColor { get => monsterColor; set => monsterColor = value; }
        public int CurrentGoldAmount { get => currentGoldAmount; set => currentGoldAmount = value; }
        public int CurrentXPAmount { get => currentXPAmount; set => currentXPAmount = value; }
        public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
        public SerializablePlayerPosition CurrentPosition { get => currentPosition; set => currentPosition = value; }
        public int LifetimeTotalWords { get => lifetimeTotalWords; set => lifetimeTotalWords = value; }
        public int LifetimeTotalLetters { get => lifetimeTotalLetters; set => lifetimeTotalLetters = value; }

        public string ClothMid { get => clothMid; set => clothMid = value; }
        public string ClothTop { get => clothTop; set => clothTop = value; }
        public List<int> BoughtClothes { get => boughtClothes; set => boughtClothes = value; }

        public List<CarInfo> ListOfCars { get => listOfCars; set => listOfCars = value; }
    }
}
