using Scenes.Minigames.MonsterTower.Scrips;
using Scenes.Minigames.MonsterTower.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scenes.Minigames.MonsterTower
{
    public class DataPersistanceManager2 : MonoBehaviour
    {




        // Start is called before the first frame update

        [Header("File Storage Config")]
        [SerializeField] private string fileName;

        private FileDataHandler1 dataHandler;


        public static DataPersistanceManager2 instance { get; private set; }

        private GameData1 gameData;


        private List<IDataPersistence1> dataPersistenceObjects;




        private void Awake()
        {
            if (instance != null)
            {
                Debug.Log("ERROR:More than one instance of the class is not allowed");

            }

            instance = this;




        }




        // sets up a newgame which is a new savefile.
        public void NewGame()
        {
            gameData = new GameData1();
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

            foreach (IDataPersistence1 dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }



        }


        // The datahandler class is instantiated and with that the file has a datapath and a filename.
        // Finds all objects that implements the IDataPersistence interface and puts it into a list. 

        private void Start()
        {

            this.dataHandler = new FileDataHandler1(Application.persistentDataPath, fileName);
            this.dataPersistenceObjects = FindAlldataPersistenceObjects();



        }

        // Gives a reference to the gamedata so the gamedata can be overwritten by the objects that inmplements the IDataPersistence interface.
        // The datahandler is then used to save the data onto the file. 

        public void SaveGame()
        {

            foreach (IDataPersistence1 dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref gameData);
            }



            dataHandler.Save(gameData);
        }


        private void OnApplicationQuit()
        {

            SaveGame();


        }


        private List<IDataPersistence1> FindAlldataPersistenceObjects()
        {
            IEnumerable<IDataPersistence1> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence1>();

            return new List<IDataPersistence1>(dataPersistenceObjects);
        }


    }
}


