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

        public SerializableKeyValuePair(Vector3Int key, int iD)
        {
            Key = key;
            ID = iD;
        }

        public PlacementData CovertToPlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex, EnumFloorDataType floorType)
        {
            return new PlacementData(occupiedPositions, iD, placedObjectIndex, floorType);
        }  
        //public int PlacedObjectIndex;
        //public List<Vector3Int> occupiedPositions;

        //public SerializableKeyValuePair(Vector3Int key, int iD, int placedObjectIndex, List<Vector3Int> occupiedPositions)
        //{
        //    Key = key;
        //    ID = iD;
        //    PlacedObjectIndex = placedObjectIndex;
        //    this.occupiedPositions = occupiedPositions;
        //}

        //public PlacementData CovertToPlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
        //{
        //    return new PlacementData(occupiedPositions, iD, placedObjectIndex);
        //}
    }
}
