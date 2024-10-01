using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.SaveData
{
    [Serializable]
    public class SerializableGridData
    {
        public List<SerializableKeyValuePair> placedObjectsList;

        public SerializableGridData(Dictionary<PlaceableTemporayItemsInfo, SaveableGridData> dictionary)
        {
            placedObjectsList = new List<SerializableKeyValuePair>();
            if (dictionary != null)
            {
                foreach (var item in dictionary)
                {
                    //placedObjectsList.Add(new SerializableKeyValuePair(item.Key, item.Value.ID,item.Value.rotationValue));
                    placedObjectsList.Add(new SerializableKeyValuePair(item.Key,
                        item.Value.ID,
                        item.Value.rotationValue,
                        item.Value.FloorType));
                }
            }
        }
        //public SerializableGridData(Dictionary<Vector3Int, PlacementData> dictionary)
        //{
        //    placedObjectsList = new List<SerializableKeyValuePair>();
        //    if (dictionary != null)
        //    {
        //        foreach (var item in dictionary)
        //        {
        //            //placedObjectsList.Add(new SerializableKeyValuePair(item.Key, item.Value.ID,item.Value.rotationValue));
        //            placedObjectsList.Add(new SerializableKeyValuePair(item.Key, item.Value.ID));
        //        }
        //    }
        //}
    }
}
