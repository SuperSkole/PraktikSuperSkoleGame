using System;
using System.Collections.Generic;
using Scenes._11_PlayerHouseScene.script.HouseScripts;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.SaveData
{
    [Serializable]
    public class SerializableGridData
    {
        public List<SerializableKeyValuePair> placedObjectsList;

        public SerializableGridData(Dictionary<Vector3Int, PlacementData> dictionary)
        {
            placedObjectsList = new List<SerializableKeyValuePair>();
            foreach (var kvp in dictionary)
            {
                placedObjectsList.Add(new SerializableKeyValuePair(kvp.Key, kvp.Value.ID));
            }
        }
        //public Dictionary<Vector3Int, PlacementData> ConvertListToDic(List<SerializableKeyValuePair> list)
        //{
        //    Dictionary<Vector3Int, PlacementData> placedObj = new();
        //    foreach (var item in list) 
        //    {
        //        placedObj.Add(item.Key,item.CovertToPlacementData(item.occupiedPositions,
        //                                                          item.ID,
        //                                                          item.PlacedObjectIndex));
        //    }
        //    return  placedObj;
        //}
        //public List<SerializableKeyValuePair> placedObjectsList;

        //public SerializableGridData(Dictionary<Vector3Int, PlacementData> dictionary)
        //{
        //    placedObjectsList = new List<SerializableKeyValuePair>();
        //    foreach (var kvp in dictionary)
        //    {
        //        placedObjectsList.Add(new SerializableKeyValuePair(kvp.Key, kvp.Value.ID, kvp.Value.PlacedObjectIndex, kvp.Value.occupiedPositions));
        //    }
        //}
        //public Dictionary<Vector3Int, PlacementData> ConvertListToDic(List<SerializableKeyValuePair> list)
        //{
        //    Dictionary<Vector3Int, PlacementData> placedObj = new();
        //    foreach (var item in list) 
        //    {
        //        placedObj.Add(item.Key,item.CovertToPlacementData(item.occupiedPositions,
        //                                                          item.ID,
        //                                                          item.PlacedObjectIndex));
        //    }
        //    return  placedObj;
        //}
    }
}
