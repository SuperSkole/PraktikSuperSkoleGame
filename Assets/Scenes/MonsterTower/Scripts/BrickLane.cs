using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BrickLane
{
    // Start is called before the first frame update

    public List<Sprite>  wrongSprites;

    public Sprite correctSprite;

    public int zPosition;

    public int correctImageIndex;




    public BrickLane(List<Sprite> wrongSprites, Sprite correctSprite,int zPosition,int correctImageIndex )
    {

        this.wrongSprites = wrongSprites;

        this.correctSprite = correctSprite;

        this.zPosition = zPosition;



        this.correctImageIndex = correctImageIndex;

        Debug.Log(correctImageIndex);



    }

   
}
