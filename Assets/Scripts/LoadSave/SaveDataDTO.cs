using System.Collections.Concurrent;
using System.Collections.Generic;
using Analytics;
using Letters;
using Newtonsoft.Json;
using Words;

namespace LoadSave
{
    /// <summary>
    /// This class acts as a Data Transfer Object for game state serialization.
    /// It contains fields that represent the state to be saved.
    /// </summary>
    [System.Serializable]
    public class SaveDataDTO : IDataTransferObject
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
        private int playerLanguageLevel;
        [JsonIgnore] public ConcurrentDictionary<string, LetterData> LettersWeights = new ConcurrentDictionary<string, LetterData>();
        [JsonIgnore] public ConcurrentDictionary<string, WordData> WordWeights = new ConcurrentDictionary<string, WordData>();
        //[JsonIgnore] public ConcurrentDictionary<string, SentenceData> SentenceWeights = new ConcurrentDictionary<string, SentenceData>();
        [JsonIgnore] public List<string> CollectedWords = new List<string>();
        [JsonIgnore] public List<char> CollectedLetters = new List<char>();
        private int lifetimeTotalWords;
        private int lifetimeTotalLetters;

        // Clothing
        private string clothMid;
        private string clothTop;
        [JsonIgnore] public List<int> BoughtClothes = new List<int>();

        // Cars
        [JsonIgnore] public List<CarInfo> ListOfCars = new List<CarInfo>();
        
        //House
        [JsonIgnore] public List<int> ListOfFurniture = new List<int>();

        // Properties for encapsulation
        public string Username { get => username; set => username = value; }
        public string MonsterName { get => monsterName; set => monsterName = value; }
        public int MonsterTypeID { get => monsterTypeID; set => monsterTypeID = value; }
        public string MonsterColor { get => monsterColor; set => monsterColor = value; }
        public int CurrentGoldAmount { get => currentGoldAmount; set => currentGoldAmount = value; }
        public int CurrentXPAmount { get => currentXPAmount; set => currentXPAmount = value; }
        public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
        public SerializablePlayerPosition CurrentPosition { get => currentPosition; set => currentPosition = value; }
        
        // Words and letters
        public int PlayerLanguageLevel { get => playerLanguageLevel; set => playerLanguageLevel = value; }
        public ConcurrentDictionary<string, LetterData> LettersWeightsProperty { get => LettersWeights; set => LettersWeights = value; }
        public ConcurrentDictionary<string, WordData> WordWeightsProperty { get => WordWeights; set => WordWeights = value; }
        //public ConcurrentDictionary<string, SentenceData> SentenceWeightsProperty { get => LettersWeights; set => LettersWeights = value; }
        public List<string> CollectedWordsProperty { get => CollectedWords; set => CollectedWords = value; }
        public List<char> CollectedLettersProperty { get => CollectedLetters; set => CollectedLetters = value; }
        public int LifetimeTotalWords { get => lifetimeTotalWords; set => lifetimeTotalWords = value; }
        public int LifetimeTotalLetters { get => lifetimeTotalLetters; set => lifetimeTotalLetters = value; }

        // Clothing
        public string ClothMid { get => clothMid; set => clothMid = value; }
        public string ClothTop { get => clothTop; set => clothTop = value; }
        public List<int> BoughtClothesProperty { get => BoughtClothes; set => BoughtClothes = value; }
        
        // cars
        public List<CarInfo> ListOfCarsProperty { get => ListOfCars; set => ListOfCars = value; }
        
        //Furniture
        public List<int> ListOfFurnitureBought { get => ListOfFurniture; set => ListOfFurniture = value; }
    }
}