using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private FileDataHandler dataHandler;


    public static DataPersistenceManager instance { get; private set; }

    private GameData gameData;

    [SerializeField] List<GameObject> Level1;


    List<BrickData> defaultBrickLanes=new List<BrickData>();
    private List<IDataPersistence> dataPersistenceObjects;

     

    private void Awake()
    {
        if(instance!=null)
        {
            Debug.Log("ERROR:More than one instance of the class is not allowed");

        }

        instance = this;

        SetDefaultLanes();


    }


    public void SetDefaultLanes()
    {
        foreach (var item in Level1)
        {
            defaultBrickLanes.Add(item.GetComponent<BrickData>());
        }
       
    }

    public void NewGame(List<BrickData> brickLanes )
    {
        gameData = new GameData(brickLanes);
    }


    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if(this.gameData==null)
        {
            Debug.Log("No data was found. initialising data to default values");
            NewGame(defaultBrickLanes);

        }


        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

       // Debug.Log("Loaded Lanes=" + gameData.brickLanes[0].sentence);

       

    
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAlldataPersistenceObjects();
        LoadGame();

    }

    public void SaveGame()
    {

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

       // Debug.Log("Saved lanes=" + gameData.brickLanes);

        dataHandler.Save(gameData);
    }


    private void OnApplicationQuit()
    {
        SaveGame();
    }


    private List<IDataPersistence> FindAlldataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }


}
