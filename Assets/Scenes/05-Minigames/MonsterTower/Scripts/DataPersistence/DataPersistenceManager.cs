using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;



namespace Scenes.Minigames.MonsterTower.Scrips
{



    /// <summary>
    /// A manager handling all the data that needs to be saved and loaded.
    /// It also defines the name of the savefile. 
    /// </summary>
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")]
        [SerializeField] private string fileName;
        private FileDataHandler dataHandler;
        public static DataPersistenceManager instance { get; private set; }
        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects;


        private void Awake()
        {
            if (instance != null)
                Debug.Log("ERROR:More than one instance of the class is not allowed");
            instance = this;
        }

        // sets up a newgame which is a new savefile.
        public void NewGame()
        {
            gameData = new GameData();
        }

        // if there is nothing in gameData the newGame Method is used to create an empty instantiated gamedata file.
        // The gamedata file can then be saved to when the save method is used.
        public void LoadGame()
        {
            gameData = dataHandler.Load();
            if (gameData == null)
            {
                Debug.Log("No data was found. initialising data to default values");
                NewGame();
            }

            //Every object inheriting the IDataPersistence interface will be able to read the gamedata. 
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
        }


        // The datahandler class is instantiated and with that the file has a datapath and a filename.
        // Finds all objects that implements the IDataPersistence interface and puts it into a list. 
        private void Start()
        {
            dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            dataPersistenceObjects = FindAlldataPersistenceObjects();
            LoadGame();
        }

        // Gives a reference to the gamedata so the gamedata can be overwritten by the objects that inmplements the IDataPersistence interface.
        // The datahandler is then used to save the data onto the file. 
        public void SaveGame()
        {
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref gameData);
            }
            dataHandler.Save(gameData);
        }


        //Saves the game when player quit the application
        private void OnApplicationQuit()
        {
            SaveGame();
        }


        // Gets all objects that implements the IDataPersistence interface because those af the objects that have to save and load data. 
        private List<IDataPersistence> FindAlldataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }
    }
}
