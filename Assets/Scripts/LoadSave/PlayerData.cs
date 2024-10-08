using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;

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

        [JsonIgnore] public List<int> ListOfFurniture = new List<int>();


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
        /// Initializes the game character with provided attributes.
        /// </summary>
        /// <param name="username">The name of the user.</param>
        /// <param name="monsterName">The name of the monster associated with the user.</param>
        /// <param name="monsterTypeID">The ID of the monster type.</param>
        /// <param name="monsterColor">The color of the monster.</param>
        /// <param name="goldAmount">Initial amount of gold.</param>
        /// <param name="xpAmount">Initial experience points.</param>
        /// <param name="level">Starting level of the character.</param>
        /// <param name="position">Initial position of the character in the game world.</param>
        /// <param name="collectedWords">The list of collected wordsOrLetters.</param>
        /// <param name="collectedLetters">The list of collected letters.</param>
        /// <param name="totalWords">Total number of wordsOrLetters collected.</param>
        /// <param name="totalLetters">Total number of letters collected.</param>
        /// <param name="midCloth">The middle clothing of the character.</param>
        /// <param name="topCloth">The top clothing of the character.</param>
        /// <param name="boughtClothes">The list of bought clothes.</param>
        /// <param name="listOfCars">The list of cars associated with the character.</param>
        public void Initialize(string username,
            string monsterName,
            //int monsterTypeID,
            string monsterColor,
            int goldAmount,
            int xpAmount,
            int level,
            Vector3 position,
            List<string> collectedWords,
            List<char> collectedLetters,
            int totalWords,
            int totalLetters,
            string midCloth,
            string topCloth,
            List<int> boughtClothes,
            List<CarInfo> listOfCars,
            List<int> ListOfFurniture
            )
        {
            this.username = username;
            this.monsterName = monsterName;
            //this.monsterTypeID = monsterTypeID;
            this.monsterColor = monsterColor;
            this.currentGoldAmount = goldAmount;
            this.currentXPAmount = xpAmount;
            this.currentLevel = level;
            this.currentPosition = position;
            
            // wordsOrLetters and letters
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
            this.ListOfFurniture = ListOfFurniture;
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