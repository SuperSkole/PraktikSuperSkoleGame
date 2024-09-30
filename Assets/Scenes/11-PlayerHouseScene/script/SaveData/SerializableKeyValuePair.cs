using System;
using System.Collections.Generic;
using Scenes._11_PlayerHouseScene.script.HouseScripts;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.SaveData
{
    [Serializable]
    public class SerializableKeyValuePair
    {
        public Vector3Int Key;

        public int ID;

//        public int rotationValue;

        //public SerializableKeyValuePair(Vector3Int key, int iD, int rotationValue)
        public SerializableKeyValuePair(Vector3Int key, int iD)
        {
            Key = key;
            ID = iD;
            //this.rotationValue = rotationValue;
        }

        public PlacementData CovertToPlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex, EnumFloorDataType floorType)
        {
            return new PlacementData(occupiedPositions, iD, placedObjectIndex, floorType);
        }  
    }
}
