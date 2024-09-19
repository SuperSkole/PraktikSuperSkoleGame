using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace LoadSave
{
    /// <summary>
    /// Holds all the information about the player, is used for saving.
    /// </summary>
    public class PlayerData : MonoBehaviour
    {
        // We serialize so we can see values in inspector
        [SerializeField] private string username;
        [SerializeField] private string savefile;
        [SerializeField] private string monsterName;
        [SerializeField] private int monsterTypeID;
        [SerializeField] private string monsterColor;
        [SerializeField] private int currentGoldAmount;
        [SerializeField] private int currentXPAmount;
        [SerializeField] private int pendingGoldAmount;
        [SerializeField] private int pendingXPAmount;
        [SerializeField] private int currentLevel;
        [SerializeField] private Vector3 currentPosition;
        [SerializeField] private string clothMid;
        [SerializeField] private string clothTop;

        // Lists for storing active words
        public List<string> CollectedWords = new List<string>();
        public List<char> CollectedLetters = new List<char>();
        public List<char> CollectedNumbers = new List<char>();

        // Clothing list
        public List<int> BoughtClothes = new List<int>();

        public List<CarInfo> listOfCars = new List<CarInfo>() { new CarInfo("Mustang", "Red", true, new List<MaterialInfo> { new MaterialInfo(true, "Red") }) };


        public string Username { get => username; set => username = value; }
        public string Savefile { get => savefile; set => savefile = value; }
        public string MonsterName { get => monsterName; set => monsterName = value; }
        public int MonsterTypeID { get => monsterTypeID; set => monsterTypeID = value; }
        public string MonsterColor { get => monsterColor; set => monsterColor = value; }
        public int CurrentGoldAmount { get => currentGoldAmount; set => currentGoldAmount = value; }
        public int CurrentXPAmount { get => currentXPAmount; set => currentXPAmount = value; }
        public int PendingGoldAmount { get => pendingGoldAmount; set => pendingGoldAmount = value; }
        public int PendingXPAmount { get => pendingXPAmount; set => pendingXPAmount = value; }
        public int CurrentLevel { get => currentLevel; set => currentLevel = value; }

        public string ClothMid { get => clothMid; set => clothMid = value; }
        public string ClothTop { get => clothTop; set => clothTop = value; }

        public Vector3 CurrentPosition { get => currentPosition; set => currentPosition = value; }

        public Vector3 LastInteractionPoint;

        //For the Car
        public Vector3 CarPos { get; set; } = new Vector3(-13, 0, 35);
        public quaternion CarRo { get; set; } = new Quaternion(0, 180, 0, 1);
        public float FuelAmount { get; set; } = 1f;


        /// <summary>
        /// Initializes the game character with provided attributes.
        /// </summary>
        /// <param name="username">The name of the user.</param>
        /// <param name="monsterName">The name of the monster associated with the user.</param>
        /// <param name="monsterColor">The color of the monster.</param>
        /// <param name="goldAmount">Initial amount of gold.</param>
        /// <param name="xpAmount">Initial experience points.</param>
        /// <param name="level">Starting level of the character.</param>
        /// <param name="position">Initial position of the character in the game world.</param>
        public void Initialize(string username,
                               string monsterName,
                               string monsterColor,
                               int goldAmount,
                               int xpAmount,
                               int level,
                               Vector3 position,
                               string midCloth,
                               string topCloth,
                               List<CarInfo> listOfCars)
        {
            this.username = username;
            this.monsterName = monsterName;
            this.monsterColor = monsterColor;
            this.currentGoldAmount = goldAmount;
            this.currentXPAmount = xpAmount;
            this.currentLevel = level;
            this.currentPosition = position;
            this.clothMid = midCloth;
            this.clothTop = topCloth;
            this.listOfCars = listOfCars;
        }

        public void SetLastInteractionPoint(Vector3 position)
        {
            LastInteractionPoint = position;
        }
        public string ReturnActiveCarName()
        {

            //foreach (var item in listOfCars)
            //{
            //    if (item.IsActive)
            //    {
            //        return item.Name;
            //    }
            //}
            return GameObject.FindGameObjectWithTag("Car").name;
        }

        public void Initialize(string dtoUsername, string dtoMonsterName, string dtoMonsterColor, int dtoGoldAmount, int dtoXPAmount, int dtoPlayerLevel, Vector3 getVector3, string dtoClothMid, string dtoClothTop) { throw new System.NotImplementedException(); }
    }
}

