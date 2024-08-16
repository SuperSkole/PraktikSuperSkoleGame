using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickData 
{
    public Sprite sprite;
    public bool correctSprite;
    public Vector2 position;

    public BrickData(Vector2 pos,Sprite sprite, bool correctSprite)
    {
        this.sprite = sprite;
        this.correctSprite = correctSprite;
        this.position = pos;
    }


  
}
