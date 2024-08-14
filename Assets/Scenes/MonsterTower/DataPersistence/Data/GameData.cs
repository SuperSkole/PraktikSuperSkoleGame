using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData 
{

    List<BrickData> brickLanes=new List<BrickData>();

    public GameData(List<BrickData> brickLanes)
    {
        this.brickLanes = brickLanes;

    }





}
