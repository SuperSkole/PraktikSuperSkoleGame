using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavePlayerPosition
{
    public float x;
    public float y;
    public float z;

    public SavePlayerPosition(Vector3 position)
    {
        x = position.x;
        y = position.y;
        z = position.z;
    }
    public Vector3 GetVector3()
    {
        return new Vector3(x, y, z);
    }
    
}
