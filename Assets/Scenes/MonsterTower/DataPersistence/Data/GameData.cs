using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData 
{

   

   public List<BrickData> brickLanes;

    public GameData(List<BrickData> brickLanes)
    {
        this.brickLanes = brickLanes;

    }





}
