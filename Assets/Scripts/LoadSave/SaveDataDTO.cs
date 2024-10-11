using System.Collections.Concurrent;
using System.Collections.Generic;
using Analytics;
using Letters;
using Newtonsoft.Json;
using UnityEngine;
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

        [SerializeField] private bool tutorialHouse;
        [SerializeField] private bool tutorialMainWorldFirstTime;
        [SerializeField] private bool tutorialLetterGarden;
        [SerializeField] private bool tutorialSymbolEater;
        [SerializeField] private bool tutorialBankFront;
        [SerializeField] private bool tutorialBankBack;
        [SerializeField] private bool tutorialRace;
        [SerializeField] private bool tutorialPathOfDanger;
        [SerializeField] private bool tutorialFactory;
        [SerializeField] private bool tutorialMosterTower;
        [SerializeField] private bool tutorialTransportbond;
        [SerializeField] private bool tutorialCar;
        [SerializeField] private bool tutorialDecorHouse;

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
        public float FuelAmount { get; set; } = 1f;

        //Furniture
        public List<int> ListOfFurnitureBought { get => ListOfFurniture; set => ListOfFurniture = value; }
        public bool TutorialHouse { get => tutorialHouse; set => tutorialHouse = value; }
        public bool TutorialMainWorldFirstTime { get => tutorialMainWorldFirstTime; set => tutorialMainWorldFirstTime = value; }
        public bool TutorialLetterGarden { get => tutorialLetterGarden; set => tutorialLetterGarden = value; }
        public bool TutorialSymbolEater { get => tutorialSymbolEater; set => tutorialSymbolEater = value; }
        public bool TutorialBankFront { get => tutorialBankFront; set => tutorialBankFront = value; }
        public bool TutorialBankBack { get => tutorialBankBack; set => tutorialBankBack = value; }
        public bool TutorialRace { get => tutorialRace; set => tutorialRace = value; }
        public bool TutorialPathOfDanger { get => tutorialPathOfDanger; set => tutorialPathOfDanger = value; }
        public bool TutorialFactory { get => tutorialFactory; set => tutorialFactory = value; }
        public bool TutorialMosterTower { get => tutorialMosterTower; set => tutorialMosterTower = value; }
        public bool TutorialTransportbond { get => tutorialTransportbond; set => tutorialTransportbond = value; }
        public bool TutorialCar { get => tutorialCar; set => tutorialCar = value; }
        public bool TutorialDecorHouse { get => tutorialDecorHouse; set => tutorialDecorHouse = value; }
    }
}