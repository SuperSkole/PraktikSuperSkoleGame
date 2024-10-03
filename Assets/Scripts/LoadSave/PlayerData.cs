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
        [SerializeField] private int languageLevel;
        [JsonIgnore] public ConcurrentDictionary<string, LetterData> LettersWeights = new ConcurrentDictionary<string, LetterData>();
        [JsonIgnore] public ConcurrentDictionary<string, WordData> WordWeights = new ConcurrentDictionary<string, WordData>();
        [JsonIgnore] public ConcurrentDictionary<string, ILanguageUnit> SentenceWeights = new ConcurrentDictionary<string, ILanguageUnit>();
        [JsonIgnore] public List<string> CollectedWords = new List<string>();
        [JsonIgnore] public List<char> CollectedLetters = new List<char>();
        [SerializeField] private int lifetimeTotalWords;
        [SerializeField] private int lifetimeTotalLetters;
        
        // Clothing
        [SerializeField] private string clothMid;
        [SerializeField] private string clothTop;
        [JsonIgnore] public List<int> BoughtClothes = new List<int>();


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
        public int LanguageLevel { get => languageLevel; set => languageLevel = value; }
        public ConcurrentDictionary<string, LetterData> LettersWeightsProperty { get => LettersWeights; set => LettersWeights = value; }
        public ConcurrentDictionary<string, WordData> WordWeightsProperty { get => WordWeights; set => WordWeights = value; }
        //public ConcurrentDictionary<string, ILanguageUnit> SentenceWeightsProperty { get => LettersWeights; set => LettersWeights = value; }
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
        /// <param name="languageLevel">The player's language proficiency level.</param>
        /// <param name="lettersWeights">A dictionary mapping letters to their weights.</param>
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
            int languageLevel,
            ConcurrentDictionary<string, LetterData> lettersWeights,
            // TODO ADD WORDS AND SENTENCES
            List<string> collectedWords,
            List<char> collectedLetters,
            int totalWords,
            int totalLetters,
            string midCloth,
            string topCloth,
            List<int> boughtClothes,
            List<CarInfo> listOfCars,
            List<int> ListOfFurniture)
        {
            this.username = username;
            this.monsterName = monsterName;
            //this.monsterTypeID = monsterTypeID;
            this.monsterColor = monsterColor;
            this.currentGoldAmount = goldAmount;
            this.currentXPAmount = xpAmount;
            this.currentLevel = level;
            this.currentPosition = position;
            
            // words and letters
            this.languageLevel = languageLevel;
            this.LettersWeights.Clear();
            foreach (var kvp in lettersWeights)
            {
                this.LettersWeights.TryAdd(kvp.Key, kvp.Value);
            }
            
            this.CollectedWords.Clear();
            this.CollectedWords.AddRange(collectedWords);
            this.CollectedLetters.Clear();
            this.CollectedLetters.AddRange(collectedLetters);
            this.LifetimeTotalWords = totalWords;
            this.LifetimeTotalLetters = totalLetters;
            
            // cloth
            this.clothMid = midCloth;
            this.clothTop = topCloth;
            this.BoughtClothes.Clear();
            this.BoughtClothes.AddRange(boughtClothes);
            
            // cars
            this.ListOfCars.Clear();
            this.ListOfCars.AddRange(listOfCars);
            
            // Furniture
            this.ListOfFurniture = ListOfFurniture;
        }

        public void SetLastInteractionPoint(Vector3 position)
        {
            LastInteractionPoint = position;
        }
        
        public string ReturnActiveCarName() => GameObject.FindGameObjectWithTag("Car").name;
        
        // Method to update word weights from repository
        public void UpdateWordWeightsFromRepository(IWordRepository wordRepository)
        {
            foreach (var wordLength in Enum.GetValues(typeof(WordLength)).Cast<WordLength>())
            {
                var words = wordRepository.GetWordsByLength(wordLength);
                foreach (var wordData in words)
                {
                    if (!WordWeights.ContainsKey(wordData.Identifier))
                    {
                        WordWeights.TryAdd(wordData.Identifier, wordData);
                    }
                }
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class ExcludeFromSaveAttribute : Attribute
{
}