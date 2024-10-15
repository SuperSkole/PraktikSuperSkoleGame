using System;
using System.Collections.Generic;
using Scenes._11_PlayerHouseScene.script.HouseScripts;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.SaveData
{
    [Serializable]
    public class SerializableKeyValuePair
    {
        public PlaceableTemporayItemsInfo Key;

        public int ID;

        public int RotationValue;

        public EnumFloorDataType FloorType;

        public SerializableKeyValuePair(PlaceableTemporayItemsInfo key, int iD, int rotationValue, EnumFloorDataType floorType)
        {
            Key = key;
            ID = iD;
            RotationValue = rotationValue;
            FloorType = floorType;
        }
    }
}
