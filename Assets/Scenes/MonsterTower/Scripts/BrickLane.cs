using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class BrickLane
{
    // Start is called before the first frame update

    public List<BrickData>  bricks;

    public BrickData correctBrick;

    public int zPosition;

    public int correctImageIndex;

    


    public BrickLane(List<BrickData> bricks, BrickData correctBrick,int zPosition,int correctImageIndex )
    {

        this.bricks = bricks;

        this.correctBrick = correctBrick;

        this.zPosition = zPosition;



        this.correctImageIndex = correctImageIndex;

        Debug.Log(correctImageIndex);



    }

   
}
