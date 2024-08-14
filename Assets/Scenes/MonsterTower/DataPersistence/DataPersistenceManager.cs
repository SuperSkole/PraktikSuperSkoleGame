using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    // Start is called before the first frame update
   

    public static DataPersistenceManager instance { get; private set; }

    private GameData gameData;




    private void Awake()
    {
        if(instance!=null)
        {
            Debug.Log("ERROR:More than one instance of the class is not allowed");

        }

        instance = this;



    }


    public void NewGame(List<BrickData> brickLanes )
    {
        gameData = new GameData(brickLanes);
    }


    public void LoadGame()
    { 

        if(this.gameData==null)
        {
            Debug.Log("No data was found. initialising data to default values");
        }


    
    }

    public void SaveGame()
    {

    }



}
