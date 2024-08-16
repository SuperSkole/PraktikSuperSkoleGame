using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public class DataPersistenceManager : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private FileDataHandler dataHandler;


    public static DataPersistenceManager instance { get; private set; }

    private GameData gameData;

    List<BrickLane> Level1=new List<BrickLane>();

    [SerializeField]
    private List<Sprite> wrongSprites;

    [SerializeField]
    private Sprite correctSprite;

    [SerializeField]
    private int zPosition;

    [SerializeField]
    private List<Sprite> wrongSprites2;

    [SerializeField]
    private Sprite correctSprite2;

    [SerializeField]
    private int zPosition2;


    private List<BrickLane> defaultBrickLanes=new List<BrickLane>();
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
       

        Level1.Add(new BrickLane(wrongSprites, correctSprite, zPosition, Random.Range(0, wrongSprites2.Count)));

        Level1.Add(new BrickLane(wrongSprites2, correctSprite2, zPosition2, Random.Range(0, wrongSprites2.Count)));


        // Lanes are added with BrickLane containing a dictionary filled with pictures and a coresponding position. 
        defaultBrickLanes = Level1;

    }
       
    

    public void NewGame(List<BrickLane> BrickLanes)
    {
        gameData = new GameData(BrickLanes);
    }


    public void LoadGame()
    {
        gameData = dataHandler.Load();

    
       
        if(gameData==null)
        {
            Debug.Log("No data was found. initialising data to default values");
            NewGame(defaultBrickLanes);

        }


        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

       // Debug.Log("Loaded Lanes=" + gameData.BrickLanes[0].sentence);

       

    
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
