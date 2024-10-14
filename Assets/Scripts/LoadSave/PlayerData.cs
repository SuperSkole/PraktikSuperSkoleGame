using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Analytics;
using Letters;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;
using Words;

namespace LoadSave
{
    /// <summary>
    /// Holds all the information about the player, is used for saving.
    /// </summary>
    public class PlayerData : MonoBehaviour
    {
        public Vector3 LastInteractionPoint;
        // We serialize so we can see values in inspector
        
        // player Stats
        [SerializeField] private string username;
        [SerializeField] private string monsterName;
        [SerializeField] private int monsterTypeID;
        [SerializeField] private string monsterColor;
        [SerializeField] private int currentGoldAmount;
        [SerializeField] private int currentXPAmount;
        [SerializeField] private int pendingGoldAmount;
        [SerializeField] private int pendingXPAmount;
        [SerializeField] private int currentLevel;
        [SerializeField] private Vector3 currentPosition;
        
        // Words and letters
        [SerializeField] private int playerLanguageLevel;
        [JsonIgnore] public ConcurrentDictionary<string, LetterData> LettersWeights = new ConcurrentDictionary<string, LetterData>();
        [JsonIgnore] public ConcurrentDictionary<string, WordData> WordWeights = new ConcurrentDictionary<string, WordData>();
        //[JsonIgnore] public ConcurrentDictionary<string, SentenceData> SentenceWeights = new ConcurrentDictionary<string, SentenceData>();
        [JsonIgnore] public List<string> CollectedWords = new List<string>();
        [JsonIgnore] public List<char> CollectedLetters = new List<char>();
        [SerializeField] private int lifetimeTotalWords;
        [SerializeField] private int lifetimeTotalLetters;
        
        // Clothing
        [SerializeField] private string clothMid;
        [SerializeField] private string clothTop;
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


        //For the Car
        [JsonIgnore] public List<CarInfo> ListOfCars = new List<CarInfo>();
        [ExcludeFromSave] public Vector3 CarPos { get; set; } = new Vector3(-13, 0, 35);
        [ExcludeFromSave] public quaternion CarRo { get; set; } = new Quaternion(0, 180, 0, 1);
        [ExcludeFromSave] public float FuelAmount { get; set; } = 1f;

        // Furniture
        [JsonIgnore] public List<int> ListOfFurniture = new List<int>();

        // Properties for encapsulation and dataconvertion reflection

        public string Username { get => username; set => username = value; }
        public string MonsterName { get => monsterName; set => monsterName = value; }
        public int MonsterTypeID { get => monsterTypeID; set => monsterTypeID = value; }
        public string MonsterColor { get => monsterColor; set => monsterColor = value; }
        public int CurrentGoldAmount { get => currentGoldAmount; set => currentGoldAmount = value; }
        public int CurrentXPAmount { get => currentXPAmount; set => currentXPAmount = value; }
        [ExcludeFromSave] public int PendingGoldAmount { get => pendingGoldAmount; set => pendingGoldAmount = value; }
        [ExcludeFromSave] public int PendingXPAmount { get => pendingXPAmount; set => pendingXPAmount = value; }
        public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
        public Vector3 CurrentPosition { get => currentPosition; set => currentPosition = value; }
        
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

        /// <summary>
        /// Initializes the player's data with the provided parameters.
        /// </summary>
        /// <param name="username">The username of the player.</param>
        /// <param name="monsterName">The name of the player's monster.</param>
        /// <param name="monsterColor">The color of the player's monster.</param>
        /// <param name="goldAmount">The amount of gold the player currently has.</param>
        /// <param name="xpAmount">The amount of experience points the player currently has.</param>
        /// <param name="level">The current level of the player.</param>
        /// <param name="position">The current position of the player in the game world.</param>
        /// <param name="playerLanguageLevel">The player's language proficiency level.</param>
        /// <param name="lettersWeights">A dictionary mapping letters to their weights.</param>
        /// <param name="wordWeights">A dictionary mapping words to their weights.</param>
        /// <param name="collectedWords">The list of words the player has collected.</param>
        /// <param name="collectedLetters">The list of letters the player has collected.</param>
        /// <param name="totalWords">The total number of words collected over the player's lifetime.</param>
        /// <param name="totalLetters">The total number of letters collected over the player's lifetime.</param>
        /// <param name="midCloth">The identifier for the middle clothing item the player has equipped.</param>
        /// <param name="topCloth">The identifier for the top clothing item the player has equipped.</param>
        /// <param name="boughtClothes">The list of clothing items the player has bought.</param>
        /// <param name="listOfCars">The list of cars the player owns.</param>
        /// <param name="ListOfFurniture">The list of furniture items the player owns.</param>
        public void Initialize(
            string username,
            string monsterName,
            //int monsterTypeID,
            string monsterColor,
            int goldAmount,
            int xpAmount,
            int level,
            Vector3 position,
            int playerLanguageLevel,
            ConcurrentDictionary<string, LetterData> lettersWeights,
            ConcurrentDictionary<string, WordData> wordWeights,
            // TODO ADD WORDS AND SENTENCES
            List<string> collectedWords,
            List<char> collectedLetters,
            int totalWords,
            int totalLetters,
            string midCloth,
            string topCloth,
            List<int> boughtClothes,
            List<CarInfo> listOfCars,
            List<int> ListOfFurniture,
            bool tutorialHouse,
            bool tutorialMainWorldFirstTime,
            bool tutorialLetterGarden,
            bool tutorialSymbolEater,
            bool tutorialBankFront,
            bool tutorialBankBack,
            bool tutorialRace,
            bool tutorialPathOfDanger,
            bool tutorialFactory,
            bool tutorialMosterTower,
            bool tutorialTransportbond,
            bool tutorialCar,
            bool tutorialDecorHouse)
        {
            this.username = username;
            this.monsterName = monsterName;
            //this.monsterTypeID = monsterTypeID;
            this.monsterColor = monsterColor;
            currentGoldAmount = goldAmount;
            currentXPAmount = xpAmount;
            currentLevel = level;
            currentPosition = position;
            
            // words and letters
            this.playerLanguageLevel = playerLanguageLevel;
            LettersWeights.Clear();
            foreach (var kvp in lettersWeights)
            {
                this.LettersWeights.TryAdd(kvp.Key, kvp.Value);
            }
            
            WordWeights.Clear();
            foreach (var kvp in wordWeights)
            {
                this.WordWeights.TryAdd(kvp.Key, kvp.Value);
            }
            
            CollectedWords.Clear();
            CollectedWords.AddRange(collectedWords);
            CollectedLetters.Clear();
            CollectedLetters.AddRange(collectedLetters);
            LifetimeTotalWords = totalWords;
            LifetimeTotalLetters = totalLetters;
            
            // cloth
            clothMid = midCloth;
            clothTop = topCloth;
            BoughtClothes.Clear();
            BoughtClothes.AddRange(boughtClothes);
            
            // cars
            ListOfCars.Clear();
            ListOfCars.AddRange(listOfCars);
            
            // Furniture
            this.ListOfFurniture = ListOfFurniture;
            this.tutorialHouse = tutorialHouse;
            this.tutorialMainWorldFirstTime = tutorialMainWorldFirstTime;
            this.tutorialLetterGarden = tutorialLetterGarden;
            this.tutorialSymbolEater = tutorialSymbolEater;
            this.tutorialBankFront = tutorialBankFront;
            this.tutorialBankBack = tutorialBankBack;
            this.tutorialRace = tutorialRace;
            this.tutorialPathOfDanger = tutorialPathOfDanger;
            this.tutorialFactory = tutorialFactory;
            this.tutorialMosterTower = tutorialMosterTower;
            this.tutorialTransportbond = tutorialTransportbond;
            this.tutorialCar = tutorialCar;
            this.tutorialDecorHouse = tutorialDecorHouse;
        }

        public void SetLastInteractionPoint(Vector3 position)
        {
            LastInteractionPoint = position;
        }
        
        public string ReturnActiveCarName() => GameObject.FindGameObjectWithTag("Car").name;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class ExcludeFromSaveAttribute : Attribute
{
}