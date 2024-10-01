using Scenes._11_PlayerHouseScene.script.HouseScripts;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveableGridData
{
    public Vector2Int occupiedPosition;

    public int ID;

    public int rotationValue;

    public EnumFloorDataType FloorType;

    public SaveableGridData(Vector2Int occupiedPosition, int ID, int rotationValue, EnumFloorDataType FloorType)
    {
        this.occupiedPosition = occupiedPosition;
        this.ID = ID;
        this.rotationValue = rotationValue;
        this.FloorType = FloorType;
    }
}
