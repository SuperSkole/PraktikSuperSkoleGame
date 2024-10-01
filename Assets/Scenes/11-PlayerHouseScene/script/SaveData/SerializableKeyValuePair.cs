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
        //public Vector3Int Key;
        //public Vector2Int Key;

        public int ID;

        public int RotationValue;

        public EnumFloorDataType FloorType;

        //public SerializableKeyValuePair(Vector3Int key, int iD)
        public SerializableKeyValuePair(PlaceableTemporayItemsInfo key, int iD, int rotationValue, EnumFloorDataType floorType)
        {
            Key = key;
            ID = iD;
            RotationValue = rotationValue;
            FloorType = floorType;
        }

        public PlacementData CovertToPlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex, EnumFloorDataType floorType)
        {
            return new PlacementData(occupiedPositions, iD, placedObjectIndex, floorType);
        }  
    }
}
